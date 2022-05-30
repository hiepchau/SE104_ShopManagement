using MaterialDesignThemes.Wpf;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Components.Controls;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.Network.Insert_database;
using SE104_OnlineShopManagement.Network.Update_database;
using SE104_OnlineShopManagement.Services;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using SE104_OnlineShopManagement.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Xml;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Selling_functions
{
    public interface IUpdateSelectedList
    {
        void UpdateSelectedList(ProductsInformation pro);
        void UpdateBoughtList(ProductsInformation pro);
        void isCanExecute();
    }
    public class SellingViewModel:BaseFunction, IUpdateSelectedList
    {
        #region Properties
        public ObservableCollection<POSProductControlViewModel> listProducts { get; set; }
        public ObservableCollection<ImportPOSProductControlViewModel> listbought { get; set; }
        public ProductsInformation selectedProduct { get; set; }
        public string CurrentName { get; set; }
        public string CurrentID { get; set; }
        public string today { get; set; }
        public string clock { get; set; }
        public string totalPay { get; set; }
        public string CustomerPhoneNumber { get; set; }
        private AppSession _session;
        private MongoConnect _connection;
        public AppSession Session { get => _session; }
        public MongoConnect Connection { get => _connection; }
        public string searchString { get; set; }
        #endregion

        #region Commands
        public ICommand SearchCommand { get; set; }
        public ICommand PurchaseCommand { get; set; }
        public ICommand ReloadCommand { get; set; }
        public ICommand TextChangedCommand { get; set; }
        public ICommand PrintBillCommand { get; set; }
        #endregion
        public SellingViewModel(AppSession session, MongoConnect client) : base(session, client)
        {
            _session = session;
            _connection = client;
            CurrentName = _session.CurrnetUser.FirstName + " " + _session.CurrnetUser.LastName;
            CurrentID = _session.CurrnetUser.ID;
            listProducts = new ObservableCollection<POSProductControlViewModel>();
            listbought = new ObservableCollection<ImportPOSProductControlViewModel>();
            totalPay = "0";
            getTotalPay();
            today = DateTime.Now.ToString("dd/MM/yyyy");

            clockTicking();
            SearchCommand = new RelayCommand<Object>(null, search);
            PurchaseCommand = new RelayCommand<Object>(IsValidPurchase, purchase);
            //PrintBillCommand = new RelayCommand<BillInformation>(null, PrintBill);
            TextChangedCommand = new RelayCommand<Object>(null, TextChangedHandle);
            ReloadCommand = new RelayCommand<Object>(null,async t=> { 
                listProducts.Clear();
                await getdata(); });
        }
        #region Function
        private async void purchase(object o)
        {
            var result = CustomMessageBox.Show("Xác nhận thanh toán?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                long total = 0;
                if (listbought.Count > 0)
                {
                    foreach (var item in listbought)
                    {
                        total += ConvertToNumber(item.price) * item.GetDetailNum();
                    }
                }
                BillInformation billinfo = new BillInformation(await new AutoBillIDGenerator(_session, _connection.client).Generate(),
                    DateTime.Now, _session.CurrnetUser.ID, CustomerPhoneNumber, total);
                RegisterBills registbill = new RegisterBills(billinfo, _connection.client, _session);
                Task<string> registertask = registbill.register();
                string billid = "";
               

                billid = await registertask;
                Task.WaitAll(registertask);
                //Refresh

                if (listbought.Count > 0)
                {
                    foreach (var item in listbought)
                    {
                        BillDetails tmpdetail = new BillDetails("", item.product.ID, billid, item.GetDetailNum(), item.GetDetailNum() * item.product.price);
                        RegisterBillDetails regist = new RegisterBillDetails(tmpdetail, _connection.client, _session);
                        var task1 = UpdateAmount(item);
                        var task2 = regist.register();
                        await task1;
                        await task2;
                        Task.WaitAll(task1, task2);
                    }
                }

                
                CustomMessageBox.Show("Thanh toán thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);

                var printBill = CustomMessageBox.Show("Bạn có muốn in hóa đơn?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (printBill == MessageBoxResult.Yes)
                {
                    billinfo.ID = billid;
                    PrintBill(billinfo, listbought);
                }

            }
            else
            {
                return;
            }

            listbought.Clear();
            listProducts.Clear();
            getdata();
            OnPropertyChanged(nameof(listbought));

        }
        public bool IsValidPurchase(Object o = null)
        {
            if (CustomerPhoneNumber==null||CustomerPhoneNumber.Length != 10)
                return false;
            return true;
        }

        public void PrintBill(BillInformation billinfo, ObservableCollection<ImportPOSProductControlViewModel> boughtls)
        {
            PrintDialog printDlg = new PrintDialog();
            if (printDlg.ShowDialog() != true) return;
            BillTemplate billTemplate = new BillTemplate();
            BillTemplateViewModel billTemplateViewModel = new BillTemplateViewModel(billinfo,_session, boughtls);
            billTemplate.DataContext = billTemplateViewModel;
            DialogHost.Show(billTemplate);
            FixedDocument document = new FixedDocument();
            PageContent temp;
            document.DocumentPaginator.PageSize = new Size(billTemplate.grdPrint.ActualWidth, billTemplate.grdPrint.ActualHeight);
            billTemplate.grdPrint.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            billTemplate.grdPrint.Arrange(new Rect(0, 0, billTemplate.grdPrint.DesiredSize.Width, billTemplate.grdPrint.DesiredSize.Height));
            temp = ConvertToPage(billTemplate.grdPrint);
            document.Pages.Add(temp);

            printDlg.PrintDocument(document.DocumentPaginator, "Hehe");
            CustomMessageBox.Show("In hóa đơn thành công", "Thông tin", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        public PageContent ConvertToPage(Grid grid)
        {
            FixedPage page = new FixedPage();
            page.Width = grid.ActualWidth; ;
            page.Height = grid.ActualHeight;
            string gridXaml = XamlWriter.Save(grid);
            gridXaml = gridXaml.Replace("Name=\"txbOrderNum\"", "");
            gridXaml = gridXaml.Replace("Name=\"txbUnitPrice\"", "");
            gridXaml = gridXaml.Replace("Name=\"txbName\"", "");
            gridXaml = gridXaml.Replace("Name=\"txbQuantity\"", "");
            gridXaml = gridXaml.Replace("Name=\"txbUnit\"", "");
            gridXaml = gridXaml.Replace("Name=\"txbTotal\"", "");
            StringReader stringReader = new StringReader(gridXaml);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            Grid newGrid = (Grid)XamlReader.Load(xmlReader);
            page.Children.Add(newGrid);
            PageContent pageContent = new PageContent();
            ((IAddChild)pageContent).AddChild(page);
            return pageContent;
        }
        public void TextChangedHandle(Object o = null)
        {
            (PurchaseCommand as RelayCommand<Object>).OnCanExecuteChanged();
        }
        public void UpdateSelectedList(ProductsInformation pro)
        {
            if (pro.quantity > 0)
            {
                if (listbought.Count > 0)
                {
                    foreach (ImportPOSProductControlViewModel pr in listbought)
                    {
                        if (pr.product.ID.Equals(pro.ID))
                        {
                            pr.GetIncreaseQuantityByClick();
                            return;
                        }
                    }
                }
                listbought.Add(new ImportPOSProductControlViewModel(pro, this));
                OnPropertyChanged(nameof(listbought));
            }
            else return;
        }

        public void UpdateBoughtList(ProductsInformation pro)
        {
            int i = 0;
            if (listbought.Count > 0)
            {
                foreach (ImportPOSProductControlViewModel pr in listbought)
                {
                    if (pr.product.ID.Equals(pro.ID))
                    {
                        break;
                    }
                    i++;
                }
                listbought.RemoveAt(i);
                OnPropertyChanged(nameof(listbought));
            }
            else
            {
                return;
            }
            if(listbought.Count == 0)
            {
                getTotalPay();
            }
        }
        public void getTotalPay()
        {
            if (listbought.Count == 0)
            {
                totalPay = "0";
                OnPropertyChanged(nameof(totalPay));
            }    
            if (listbought.Count > 0)
            {
                totalPay = "0";
                long sum = 0;
                foreach (ImportPOSProductControlViewModel pr in listbought)
                {
                    sum += ConvertToNumber(pr.sum.ToString());
                }
                totalPay = SeparateThousands(sum.ToString());
                OnPropertyChanged(nameof(totalPay));
            }
            else
            {
                return;
            }

        }
        public void clockTicking()
        {
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(changeTime);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void changeTime(Object seneder, EventArgs e)
        {
            clock = DateTime.Now.ToString("HH:mm:ss");
            OnPropertyChanged(nameof(clock));
        }
        private async void search(object o)
        {
            searchString = o.ToString();
            if (string.IsNullOrEmpty(searchString))
            {
                listProducts.Clear();
                await getdata();
            }
            else
            {
                await getsearchdata();
            }
        }
        public void isCanExecute()
        {
            return;
        }
        public long ConvertToNumber(string str)
        {
            string[] s = str.Split(',');
            string tmp = "";
            foreach (string a in s)
            {
                tmp += a;
            }

            return long.Parse(tmp);
        }
        public string SeparateThousands(String text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                ulong valueBefore = ulong.Parse(ConvertToNumber(text).ToString(), System.Globalization.NumberStyles.AllowThousands);
                string res = String.Format(culture, "{0:N0}", valueBefore);
                return res;
            }
            return "";
        }
        #endregion

        #region DB
        private async Task getdata()
        {
            FilterDefinition<ProductsInformation> filter = Builders<ProductsInformation>.Filter.Eq("isActivated", true);
            var tmp = new GetProducts(_connection.client, _session, filter);
            var ls = await tmp.Get();

            foreach(ProductsInformation pro in ls)
            {
                var lscheck = await CheckInactiveCategory.listInactiveCategory(_connection.client, _session, pro);
                if (lscheck.Count == 0)
                {
                    listProducts.Add(new POSProductControlViewModel(pro, this));

                }
            }
            OnPropertyChanged(nameof(listProducts));
        }
        private async Task getsearchdata()
        {
            listProducts.Clear();
            OnPropertyChanged(nameof(listProducts));
            FilterDefinition<ProductsInformation> filter = Builders<ProductsInformation>.Filter.Eq(x => x.name, searchString);
            var tmp = new GetProducts(_connection.client, _session, filter);
            var ls = await tmp.Get();
            foreach (ProductsInformation pr in ls)
            {
                listProducts.Add(new POSProductControlViewModel(pr, this));

            }
            OnPropertyChanged(nameof(listProducts));
        }
        private async Task UpdateAmount(ImportPOSProductControlViewModel item)
        {
            int newQuantity = item.product.quantity - item.GetDetailNum();
            var filter = Builders<ProductsInformation>.Filter.Eq("ID", item.product.ID);
            var update = Builders<ProductsInformation>.Update.Set("ProductQuantity", newQuantity);
            UpdateProductsInformation updater = new UpdateProductsInformation(_connection.client, _session, filter, update);
            var s = await updater.update();
            Console.WriteLine("Update Successfull: quantity = " + newQuantity);
        }
        #endregion
    }
}

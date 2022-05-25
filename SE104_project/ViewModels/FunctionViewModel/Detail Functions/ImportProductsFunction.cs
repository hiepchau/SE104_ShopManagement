using MaterialDesignThemes.Wpf;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Components;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.Network.Insert_database;
using SE104_OnlineShopManagement.Network.Update_database;
using SE104_OnlineShopManagement.Services;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Selling_functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{

    class ImportProductsFunction : BaseFunction, IUpdateSelectedList
    {
        #region Properties
        private MongoConnect _connection;
        private AppSession _session;
        public string totalReceipt { get; set; }
        public double discount { get; set; }
        public string MoneyToPay { get; set; }
        public ObservableCollection<POSProductControlViewModel> listProducts { get; set; }
        public ObservableCollection<ImportProductsControlViewModel> listItemsImportProduct { get; set; }
        public string searchString { get; set; }
        #endregion

        #region ICommand
        public ICommand OpenAddReceiptControlCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        //AddReceiptControl
        public ICommand ExitCommand { get; set; }
        public ICommand PayBillCommand { get; set; }
        public ICommand OpenAddSupplierControlCommand { get; set; }
        #endregion
        public ImportProductsFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            _connection = connect;
            _session = session;
            listItemsImportProduct = new ObservableCollection<ImportProductsControlViewModel>();
            listProducts = new ObservableCollection<POSProductControlViewModel>();
 
            _ = getdata();
            SearchCommand = new RelayCommand<Object>(null, search);
            
            OpenAddReceiptControlCommand = new RelayCommand<Object>(isEmptyImportList, OpenAddReceiptControl);
        }

        #region Function
        public void OpenAddReceiptControl(Object o = null)
        {
            AddReceiptControl addReceiptControl = new AddReceiptControl();
            addReceiptControl.DataContext = this;
            DialogHost.Show(addReceiptControl);
            totalReceipt = "0";
            long sum = 0;
            discount = 10;
            foreach (ImportProductsControlViewModel pr in listItemsImportProduct)
            {
                sum += ConvertToNumber(pr.sum);
            }
            totalReceipt = SeparateThousands(sum.ToString());
            double displayMoneytoPay = (double)ConvertToNumber(totalReceipt) - (double)(ConvertToNumber(totalReceipt) * (discount / 100));
            Console.WriteLine("money to pay: " + displayMoneytoPay);
            MoneyToPay = SeparateThousands(displayMoneytoPay.ToString());

            PayBillCommand = new RelayCommand<Object>(null, payBill);
            ExitCommand = new RelayCommand<Object>(null, exit =>
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
            });

            OpenAddSupplierControlCommand = new RelayCommand<Object>(null, OpenAddSupplierControl);
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
        public void OpenAddSupplierControl(Object o = null)
        {
            AddSupplierControl addSupplierControl = new AddSupplierControl();
            addSupplierControl.DataContext = this;
            DialogHost.Show(addSupplierControl);
            ExitCommand = new RelayCommand<Object>(null, exit =>
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
            });

        }
        public void isCanExecute()
        {
            (OpenAddReceiptControlCommand as RelayCommand<Object>).OnCanExecuteChanged();
        }
        public bool isEmptyImportList(Object o)
        {
            if (listItemsImportProduct.Count > 0)
            {
                foreach (ImportProductsControlViewModel pr in listItemsImportProduct)
                {
                    if (pr.ImportQuantityNumeric.GetDetailNum() > 0)
                    {
                        return true; 
                    }
                    return false;
                }
            }
            OnPropertyChanged(nameof(listItemsImportProduct));
            return false;
        }
        public void UpdateSelectedList(ProductsInformation pro)
        {
            if (pro.quantity >= 0)
            {
                if (listItemsImportProduct.Count >= 0)
                {
                    foreach (ImportProductsControlViewModel pr in listItemsImportProduct)
                    {
                        if (pr.product.ID.Equals(pro.ID))
                        {
                            pr.GetIncreaseQuantityByClick();
                            return;
                        }
                    }
                }
                listItemsImportProduct.Add(new ImportProductsControlViewModel(pro, this));
                OnPropertyChanged(nameof(listItemsImportProduct));
            }
            else
            {
                return;
            }
        }
        public void UpdateBoughtList(ProductsInformation pro)
        {
            int i = 0;
            if (listItemsImportProduct.Count > 0)
            {
                foreach (ImportProductsControlViewModel pr in listItemsImportProduct)
                {
                    if (pr.product.ID.Equals(pro.ID))
                    {
                        break;
                    }
                    i++;
                }
                listItemsImportProduct.RemoveAt(i);
                OnPropertyChanged(nameof(listItemsImportProduct));
            }
            else
            {
                return;
            }
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
        private async void payBill(object o)
        {
            var result = CustomMessageBox.Show("Xác nhận thanh toán?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                listProducts.Clear();
                await getdata();
                StockInformation stockInfo = new StockInformation(await new AutoStockingIDGenerator(_session, _connection.client).Generate(),
                    DateTime.Now, _session.CurrnetUser.ID, "CustomerID", ConvertToNumber(MoneyToPay));

                RegisterStocking registbill = new RegisterStocking(stockInfo, _connection.client, _session);
                Task<string> registertask = registbill.register();
                string stockID = "";
                //registertask.ContinueWith(async _ =>
                //   {
                //       foreach (var item in listItemsImportProduct)
                //       {
                //           StockDetails tmpdetail = new StockDetails("", item.product.ID, stockID,
                //               item.ImportQuantityNumeric.GetDetailNum(), item.sum);
                //           RegisterStockingDetail regist = new RegisterStockingDetail(tmpdetail, _connection.client, _session);
                //           Task.WaitAll(UpdateAmount(item), regist.register());

                //           foreach (var itemonsale in listProducts)
                //           {
                //               if (item.product.ID.Equals(itemonsale.product.ID))
                //               {
                //                   itemonsale.quantity += item.ImportQuantityNumeric.GetDetailNum();
                //                   itemonsale.onQuantityChange();
                //               }
                //           }
                //       }
                //   });
                stockID = await registertask;
                Task.WaitAll(registertask);
                if (listItemsImportProduct.Count > 0)
                {
                    foreach (var item in listItemsImportProduct)
                    {
                        StockDetails tmpdetail = new StockDetails("", item.product.ID, stockID,
                            item.ImportQuantityNumeric.GetDetailNum(), ConvertToNumber(item.sum));
                        RegisterStockingDetail regist = new RegisterStockingDetail(tmpdetail, _connection.client, _session);
                        var task1 = UpdateAmount(item);
                        var task2 = regist.register();
                        await task1;
                        await task2;
                        Task.WaitAll(task1, task2);
                        //foreach (var itemonsale in listProducts)
                        //{
                        //    if (item.product.ID.Equals(itemonsale.product.ID))
                        //    {
                        //        itemonsale.quantity -= item.GetDetailNum();
                        //        itemonsale.onQuantityChange();
                        //    }
                        //}
                    }
                }
                //Refresh
                DialogHost.CloseDialogCommand.Execute(null, null);
                listItemsImportProduct.Clear();
                listProducts.Clear();
                await getdata();
                OnPropertyChanged(nameof(listItemsImportProduct));
                CustomMessageBox.Show("Thanh toán thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                return;
            }
        }

        #endregion

        #region DB
        private async Task getdata()
        {
            FilterDefinition<ProductsInformation> filter = Builders<ProductsInformation>.Filter.Eq("isActivated", true);
            var tmp = new GetProducts(_connection.client, _session, filter);
            var task = tmp.Get();
            var ls = await task;
            Task.WaitAll(task);
            foreach (ProductsInformation pr in ls)
            {
                listProducts.Add(new POSProductControlViewModel(pr, this));
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
        private async Task UpdateAmount(ImportProductsControlViewModel item)
        {
            int newQuantity = item.quantity + item.ImportQuantityNumeric.GetDetailNum();
            var filter = Builders<ProductsInformation>.Filter.Eq("ID", item.product.ID);
            var update = Builders<ProductsInformation>.Update.Set("ProductQuantity", newQuantity);
            UpdateProductsInformation updater = new UpdateProductsInformation(_connection.client, _session, filter, update);
            var s = await updater.update();
            Console.WriteLine("Update Successfull: quantity = " + newQuantity);
        }
        #endregion
    }
}

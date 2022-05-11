using MongoDB.Driver;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.Network.Insert_database;
using SE104_OnlineShopManagement.Services;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

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
        public long totalPay { get; set; }
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
        #endregion
        public SellingViewModel(AppSession session, MongoConnect client) : base(session, client)
        {
            _session = session;
            _connection = client;
            CurrentName = _session.CurrnetUser.FirstName + " " + _session.CurrnetUser.LastName;
            CurrentID = _session.CurrnetUser.ID;
            listProducts = new ObservableCollection<POSProductControlViewModel>();
            listbought = new ObservableCollection<ImportPOSProductControlViewModel>();
            totalPay = 0;
            getTotalPay();
            today = DateTime.Now.ToString("dd/MM/yyyy");

            clockTicking();
            getdata();
            SearchCommand = new RelayCommand<object>(null, search);
            PurchaseCommand = new RelayCommand<object>(null, purchase);
            ReloadCommand = new RelayCommand<object>(null,async t=> { 
                listProducts.Clear();
                await getdata(); });
        }
        #region Function
        private async void purchase(object o)
        {
            long total = 0;
            if(listbought.Count > 0)
            {
                foreach(var item in listbought)
                {
                    total += item.price*item.GetDetailNum();
                }
            }
            BillInformation billinfo = new BillInformation(await new AutoBillIDGenerator(_session, _connection.client).Generate(), DateTime.Now, _session.CurrnetUser.ID, "CustomerID", total);
            RegisterBills registbill = new RegisterBills(billinfo, _connection.client, _session);
            Task<string> registertask = registbill.register();
            string billid = "";
            registertask.ContinueWith(async _ =>
            {
                if (listbought.Count > 0)
                {
                    foreach (var item in listbought)
                    {
                        BillDetails tmpdetail = new BillDetails("", item.product.ID, billid, item.GetDetailNum(), item.GetDetailNum() * item.product.price);
                        RegisterBillDetails regist = new RegisterBillDetails(tmpdetail, _connection.client, _session);
                        await regist.register();
                    }
                }
            });

            billid = await registertask;
        }

        public void UpdateSelectedList(ProductsInformation pro)
        {
            if (pro.quantity > 0)
            {
                if (listbought.Count > 0)
                {
                    foreach (ImportPOSProductControlViewModel pr in listbought)
                    {
                        if (pr.product.Equals(pro))
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
                    if (pr.product.Equals(pro))
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
                totalPay = 0;
                OnPropertyChanged(nameof(totalPay));
            }    
            if (listbought.Count > 0)
            {
                totalPay = 0;
                foreach (ImportPOSProductControlViewModel pr in listbought)
                {
                    totalPay += pr.sum;
                }
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
        #endregion

        #region DB
        private async Task getdata()
        {
            var tmp = new GetProducts(_connection.client, _session, FilterDefinition<ProductsInformation>.Empty);
            var ls = await tmp.Get();
            foreach(ProductsInformation pr in ls)
            {
                listProducts.Add(new POSProductControlViewModel(pr,this));
                
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


        #endregion
    }
}

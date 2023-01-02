using MongoDB.Driver;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    public interface IOrdersParent
    {
    }
    class OrdersFunction : BaseFunction, IOrdersParent
    {
        #region Properties
        private MongoConnect _connection;
        private AppSession _session;
        public bool isLoaded { get; set; }
        public string searchString { get; set; }
        public ObservableCollection<OrdersControlViewModel> listOrders { get; set; }
        #endregion

        #region ICommand
        public ICommand SearchCommand { get; set; }
        #endregion
        public OrdersFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._session = session;
            this._connection = connect;
            listOrders = new ObservableCollection<OrdersControlViewModel>();
            isLoaded = true;
            _ = GetData();
            SearchCommand = new RelayCommand<Object>(null, search);
        }

        #region Function
        private async void search(object o)
        {
            searchString = (o.ToString());
            if (string.IsNullOrEmpty(searchString))
            {
                listOrders.Clear();
                await GetData();
            }
            else
            {
                await getsearchdata();
            }
        }
        #endregion

        #region DB
        public async Task GetData()
        {
            var filter = Builders<BillInformation>.Filter.Empty;
            GetBills getter = new GetBills(_connection.client, _session, filter);
            Task<List<BillInformation>> task = getter.Get();
            //task.ContinueWith(t =>
            //{
            //    isLoaded = false;
            //    OnPropertyChanged(nameof(isLoaded));
            //});
            var ls = await task;
            Task.WaitAll(task);
            foreach (BillInformation bill in ls)
            {
                listOrders.Add(new OrdersControlViewModel(bill, this));
            }
            isLoaded = false;
            OnPropertyChanged(nameof(isLoaded));
            Console.Write("Executed");
            List<OrdersControlViewModel> lstmp = new List<OrdersControlViewModel>();
            foreach(OrdersControlViewModel od in listOrders)
                lstmp.Add(od);
            lstmp.Reverse();
            listOrders.Clear();
            foreach (OrdersControlViewModel od in lstmp)
                listOrders.Add(od);
            OnPropertyChanged(nameof(listOrders));
        }
        private async Task getsearchdata()
        {
            listOrders.Clear();
            OnPropertyChanged(nameof(listOrders));
            FilterDefinition<BillInformation> filter = Builders<BillInformation>.Filter.Eq(x => x.displayID, searchString);
            var tmp = new GetBills(_connection.client, _session, filter);
            var ls = await tmp.Get();
            foreach (BillInformation pr in ls)
            {
                listOrders.Add(new OrdersControlViewModel(pr, this));
            }
            OnPropertyChanged(nameof(listOrders));
        }
        #endregion
    }
}

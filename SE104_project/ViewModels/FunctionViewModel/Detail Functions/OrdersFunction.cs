using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    class OrdersFunction : BaseFunction
    {
        #region Properties
        private MongoConnect _connection;
        private AppSession _session;
        public bool isLoaded { get; set; }
        public ObservableCollection<OrdersControlViewModel> listOrders { get; set; }
        public ObservableCollection<UserInfomation> listUser { get; set; }
        #endregion

        #region ICommand
        #endregion
        public OrdersFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._session = session;
            this._connection = connect;
            listOrders = new ObservableCollection<OrdersControlViewModel>();
            listUser = new ObservableCollection<UserInfomation>();
            isLoaded = true;
            GetData();       
        }

        #region DB
        public async void GetData()
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
                listOrders.Add(new OrdersControlViewModel(bill));
            }
            Console.Write("Executed");
            GetEmployeeData();
            OnPropertyChanged(nameof(listOrders));
            OnPropertyChanged(nameof(isLoaded));
        }
        public async void GetEmployeeData()
        {
            var filter = Builders<UserInfomation>.Filter.Empty;
            GetUsers getter = new GetUsers(_connection.client, _session, filter);
            Task<List<UserInfomation>> task = getter.get();
            var ls = await task;
            Task.WaitAll(task);
            foreach(UserInfomation user in ls)
            {
                listUser.Add(user);
            }
            Console.WriteLine("execute employee");
            GetEmployeeName();
            isLoaded = false;            
        }
        public void GetEmployeeName()
        {
            foreach(OrdersControlViewModel order in listOrders)
            {
                foreach(UserInfomation user in listUser)
                {
                    if (order.User==user.ID)
                    {
                        order.User = user.LastName;
                    }
                }
            }
        }
        #endregion
    }
}

using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    class OrdersFunction : BaseFunction
    {
        #region Properties
        private MongoConnect _connection;
        private AppSession _session;
        public ObservableCollection<OrdersControlViewModel> listOrders { get; set; }
        #endregion

        #region ICommand
        #endregion
        public OrdersFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._session = session;
            this._connection = connect;
            listOrders = new ObservableCollection<OrdersControlViewModel>();
            GetData();
        }

        #region DB
        public async void GetData()
        {
            var filter = Builders<BillInformation>.Filter.Empty;
            GetBills getter = new GetBills(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (BillInformation bill in ls)
            {
                listOrders.Add(new OrdersControlViewModel(bill));
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(listOrders));
        }
        #endregion
    }
}

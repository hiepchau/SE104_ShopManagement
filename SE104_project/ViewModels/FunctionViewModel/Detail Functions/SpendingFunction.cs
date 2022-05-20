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
    public interface ISpendingParent
    {

    }
    class SpendingFunction : BaseFunction, ISpendingParent
    {
        #region Properties
        private MongoConnect _connection;
        private AppSession _session;
        public bool isLoaded { get; set; }
        public ObservableCollection<SpendingControlViewModel> listSpending { get; set; }
        #endregion
        public SpendingFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._session = session;
            this._connection = connect;
            listSpending = new ObservableCollection<SpendingControlViewModel>();
            isLoaded = true;
            GetData();
        }
        #region Function
    
        #endregion
        #region DB
        public async void GetData()
        {
            var filter = Builders<StockInformation>.Filter.Empty;
            GetStocking getter = new GetStocking(_connection.client, _session, filter);
            Task<List<StockInformation>> task = getter.Get();
            var ls = await task;
            Task.WaitAll(task);
            foreach (StockInformation stock in ls)
            {
                listSpending.Add(new SpendingControlViewModel(stock, this));
            }
            isLoaded = false;
            OnPropertyChanged(nameof(isLoaded));
            Console.Write("Executed");
            OnPropertyChanged(nameof(listSpending));
        }
        #endregion
    }
}

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
    public interface ISpendingParent
    {

    }
    class SpendingFunction : BaseFunction, ISpendingParent
    {
        #region Properties
        private MongoConnect _connection;
        private AppSession _session;
        public bool isLoaded { get; set; }
        public string searchString { get; set; }
        public ObservableCollection<SpendingControlViewModel> listSpending { get; set; }
        #endregion

        #region ICommand
        public ICommand SearchCommand { get; set; }
        #endregion
        public SpendingFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._session = session;
            this._connection = connect;
            listSpending = new ObservableCollection<SpendingControlViewModel>();
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
                listSpending.Clear();
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
            var filter = Builders<StockInformation>.Filter.Empty;
            GetStocking getter = new GetStocking(_connection.client, _session, filter);
            Task<List<StockInformation>> task = getter.Get();
            var ls = await task;
            Task.WaitAll(task);
            foreach (StockInformation stock in ls)
            {
                listSpending.Add(new SpendingControlViewModel(stock, this));
            }
            OnPropertyChanged(nameof(listSpending));
            isLoaded = false;
            OnPropertyChanged(nameof(isLoaded));
        }
        private async Task getsearchdata()
        {
            listSpending.Clear();
            OnPropertyChanged(nameof(listSpending));
            FilterDefinition<StockInformation> filter = Builders<StockInformation>.Filter.Eq(x => x.displayID, searchString);
            var tmp = new GetStocking(_connection.client, _session, filter);
            var ls = await tmp.Get();
            foreach (StockInformation pr in ls)
            {
                listSpending.Add(new SpendingControlViewModel(pr, this));
            }
            OnPropertyChanged(nameof(listSpending));
        }
        #endregion
    }
}

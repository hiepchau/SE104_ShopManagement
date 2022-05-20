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
    public interface IIncomeParent
    {

    }
    class IncomeFunction : BaseFunction, IIncomeParent
    {
        #region Properties
        private MongoConnect _connection;
        private AppSession _session;
        public bool isLoaded { get; set; }
        public string searchString { get; set; }
        public ObservableCollection<IncomeControlViewModel> listIncome { get; set; }
        #endregion

        #region ICommand
        public ICommand SearchCommand { get; set; }
        #endregion

        public IncomeFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._session = session;
            this._connection = connect;
            isLoaded = true;
            listIncome = new ObservableCollection<IncomeControlViewModel>();
            _ = GetData();
            SearchCommand = new RelayCommand<Object>(null, search);

        }

        #region Function
        private async void search(object o)
        {
            searchString = (o.ToString());
            if (string.IsNullOrEmpty(searchString))
            {
                listIncome.Clear();
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
            var ls = await task;
            Task.WaitAll(task);
            foreach (BillInformation bill in ls)
            {
                listIncome.Add(new IncomeControlViewModel(bill, this));
            }
            isLoaded = false;
            OnPropertyChanged(nameof(isLoaded));
            Console.Write("Executed");
            OnPropertyChanged(nameof(listIncome));
        }
        private async Task getsearchdata()
        {
            listIncome.Clear();
            OnPropertyChanged(nameof(listIncome));
            FilterDefinition<BillInformation> filter = Builders<BillInformation>.Filter.Eq(x => x.displayID, searchString);
            var tmp = new GetBills(_connection.client, _session, filter);
            var ls = await tmp.Get();
            foreach (BillInformation pr in ls)
            {
                listIncome.Add(new IncomeControlViewModel(pr, this));
            }
            OnPropertyChanged(nameof(listIncome));
        }
        #endregion
    }
}

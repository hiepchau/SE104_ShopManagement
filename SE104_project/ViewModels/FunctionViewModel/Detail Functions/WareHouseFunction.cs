using MongoDB.Driver;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Components.Controls;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.MenuViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    public class WareHouseFunction : BaseFunction
    {
        #region Properties
        private MongoConnect _connection;
        private AppSession _session;
        private ManagingFunctionsViewModel managingFunction;
        private ManagementMenu ManagementMenu;
        public string searchString { get; set; }
        public ObservableCollection<WareHouseControlViewModel> listItemWareHouse { get; set; }
        #endregion

        #region Icommand
        public ICommand OpenImportProductsCommand { get; set; }
        public ICommand PreviousWareHousePageCommand { get; set; }
        public ICommand NextWareHousePageCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        #endregion
        public WareHouseFunction(AppSession session, MongoConnect connect, ManagingFunctionsViewModel managingFunctionsViewModel, ManagementMenu managementMenu) : base(session, connect)
        {
            this._session = session;
            this._connection = connect;
            listItemWareHouse = new ObservableCollection<WareHouseControlViewModel>();

            managingFunction = managingFunctionsViewModel;
            ManagementMenu = managementMenu;

            _ = GetData();
            OpenImportProductsCommand = new RelayCommand<Object>(null, OpenImportProducts);
            SearchCommand = new RelayCommand<Object>(null, search);

        }

        #region Function
        public void OpenImportProducts(Object o = null)
        {
            managingFunction.Currentdisplaying = new ImportProductsFunction(Session, Connect);
            ManagementMenu.changeSelectedItem(4);
            managingFunction.CurrentDisplayPropertyChanged();
        }
        private async void search(object o)
        {
            searchString = (o.ToString());
            if (string.IsNullOrEmpty(searchString))
            {
                await GetData();
            }
            else
            {
                await getsearchdata();
            }
        }
        #endregion

        #region DB
        private async Task GetData()
        {
            var filter = Builders<ProductsInformation>.Filter.Eq("isActivated", true);
            GetProducts getter = new GetProducts(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (ProductsInformation pro in ls)
            {
                listItemWareHouse.Add(new WareHouseControlViewModel(pro));
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(listItemWareHouse));
        }
        private async Task getsearchdata()
        {
            listItemWareHouse.Clear();
            OnPropertyChanged(nameof(listItemWareHouse));
            FilterDefinition<ProductsInformation> filter = Builders<ProductsInformation>.Filter.Eq(x => x.name, searchString);
            var tmp = new GetProducts(_connection.client, _session, filter);
            var ls = await tmp.Get();
            foreach (ProductsInformation pr in ls)
            {
                listItemWareHouse.Add(new WareHouseControlViewModel(pr));
            }
            OnPropertyChanged(nameof(listItemWareHouse));
        }
        #endregion
    }
}

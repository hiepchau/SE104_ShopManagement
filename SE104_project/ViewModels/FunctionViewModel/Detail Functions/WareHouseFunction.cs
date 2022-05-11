using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Components.Controls;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.MenuViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    public class WareHouseFunction : BaseFunction
    {
        #region Properties
        private ManagingFunctionsViewModel managingFunction;
        private ManagementMenu ManagementMenu;
        public ObservableCollection<ProductsInformation> listItemsWareHouse { get; set; }
        #endregion

        #region Icommand
        public ICommand OpenImportProductsCommand { get; set; }
        public ICommand PreviousWareHousePageCommand { get; set; }
        public ICommand NextWareHousePageCommand { get; set; }
        #endregion
        public WareHouseFunction(AppSession session, MongoConnect connect, ManagingFunctionsViewModel managingFunctionsViewModel, ManagementMenu managementMenu) : base(session, connect)
        {
            listItemsWareHouse = new ObservableCollection<ProductsInformation>();

            managingFunction = managingFunctionsViewModel;
            ManagementMenu = managementMenu;
            OpenImportProductsCommand = new RelayCommand<Object>(null, OpenImportProducts);
        }

        #region Function
        public void OpenImportProducts(Object o = null)
        {
            managingFunction.Currentdisplaying = new ImportProductsFunction(Session, Connect);
            ManagementMenu.changeSelectedItem(4);
            managingFunction.CurrentDisplayPropertyChanged();
        }
        #endregion
    }
}

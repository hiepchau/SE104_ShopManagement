using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.MenuViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    public class WareHouseFunction : BaseFunction
    {
        private ManagingFunctionsViewModel managingFunction;
        private ManagementMenu ManagementMenu;

        #region Icommand
        public ICommand OpenImportProductsCommand { get; set; }
        #endregion
        public WareHouseFunction(AppSession session, MongoConnect connect, ManagingFunctionsViewModel managingFunctionsViewModel, ManagementMenu managementMenu) : base(session, connect)
        {
            managingFunction = managingFunctionsViewModel;
            ManagementMenu = managementMenu;
            OpenImportProductsCommand = new RelayCommand<Object>(null, OpenImportProducts);
        }

        public void OpenImportProducts(Object o = null)
        {
            managingFunction.Currentdisplaying = new ImportProductsFunction(Session, Connect);
            ManagementMenu.changeSelectedItem(3);
            managingFunction.CurrentDisplayPropertyChanged();
        }
    }
}

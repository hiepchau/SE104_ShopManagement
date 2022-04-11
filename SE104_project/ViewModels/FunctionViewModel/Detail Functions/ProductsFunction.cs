using MaterialDesignThemes.Wpf;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Components;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.MenuViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    class ProductsFunction : BaseFunction
    {
        private ManagingFunctionsViewModel managingFunction;
        private ManagementMenu ManagementMenu;

        #region ICommand
        //Product
        public ICommand OpenAddProductControlCommand { get; set; }
        public ICommand OpenImportProductsCommand { get; set; }
        //AddProduct
        public ICommand SaveCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        #endregion
        public ProductsFunction(AppSession session, MongoConnect connect, ManagingFunctionsViewModel managingFunctionsViewModel, ManagementMenu managementMenu) : base(session, connect)
        {
            managingFunction = managingFunctionsViewModel;
            ManagementMenu = managementMenu;
            OpenAddProductControlCommand = new RelayCommand<Object>(null, OpenAddProductControl);
            OpenImportProductsCommand = new RelayCommand<Object>(null, OpenImportProducts);
        }
        public void OpenAddProductControl(Object o = null)
        {
            AddProductControl addProductControl = new AddProductControl();
            addProductControl.DataContext = this;
            DialogHost.Show(addProductControl);

            ExitCommand = new RelayCommand<Object>(null, exit =>
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
            });
        }
        public void OpenImportProducts(Object o = null)
        {
            managingFunction.Currentdisplaying = new ImportProductsFunction(Session, Connect);
            ManagementMenu.changeSelectedItem(4);
            managingFunction.CurrentDisplayPropertyChanged();
        }
    }
}

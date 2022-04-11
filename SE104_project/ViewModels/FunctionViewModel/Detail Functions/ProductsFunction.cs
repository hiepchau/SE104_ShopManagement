using MaterialDesignThemes.Wpf;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Components;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    class ProductsFunction : BaseFunction
    {
        #region ICommand
        //Product
        public ICommand OpenAddProductControlCommand { get; set; }
        //AddProduct
        public ICommand SaveCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        #endregion
        public ProductsFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            OpenAddProductControlCommand = new RelayCommand<Object>(null, OpenAddProductControl);

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
    }
}

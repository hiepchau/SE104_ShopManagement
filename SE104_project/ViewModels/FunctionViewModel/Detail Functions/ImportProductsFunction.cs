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
    class ImportProductsFunction : BaseFunction
    {
        public List<ProductsInformation> listItemsImportProduct { get; set; }
        #region ICommand
        public ICommand OpenAddReceiptControlCommand { get; set; }
        //AddReceiptControl
        public ICommand ExitCommand { get; set; }
        #endregion
        public ImportProductsFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            listItemsImportProduct = new List<ProductsInformation>();
            //Test

            listItemsImportProduct.Add(new ProductsInformation("1", "hip", 12, 1000, 900, "ohye", "ohye", "nguoi"));
            //
            OpenAddReceiptControlCommand = new RelayCommand<Object>(null, OpenAddReceiptControl);
        }
        public void OpenAddReceiptControl(Object o = null)
        {
            AddReceiptControl addReceiptControl = new AddReceiptControl();
            addReceiptControl.DataContext = this;
            DialogHost.Show(addReceiptControl);

            ExitCommand = new RelayCommand<Object>(null, exit =>
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
            });
        }
    }
}

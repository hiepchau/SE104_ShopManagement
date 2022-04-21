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
<<<<<<< HEAD
            listItemsImportProduct.Add(new ProductsInformation("1", "hip", 12, 1000, 900, "ohye", "ohye",""));
=======
            listItemsImportProduct.Add(new ProductsInformation("1", "hip", 12, 1000, 900, "ohye", "ohye", "nguoi"));
>>>>>>> 13736bdc6f887259e95b8c5e4af1c2603c32ab73
            //
            OpenAddReceiptControlCommand = new RelayCommand<Object>(null, OpenAddAddReceiptControl);
        }
        public void OpenAddAddReceiptControl(Object o = null)
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

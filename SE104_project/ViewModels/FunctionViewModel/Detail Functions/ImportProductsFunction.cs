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
        #region ICommand
        public ICommand OpenAddReceiptControlCommand { get; set; }
        //AddReceiptControl
        public ICommand ExitCommand { get; set; }
        #endregion
        public ImportProductsFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
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

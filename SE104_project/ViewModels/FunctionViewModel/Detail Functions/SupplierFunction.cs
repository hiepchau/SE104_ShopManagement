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
    class SupplierFunction : BaseFunction
    {
        #region ICommand
        //Supplier
        public ICommand OpenAddSupplierControlCommand { get; set; }
        //AddSupplier
        public ICommand SaveCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        #endregion
        public SupplierFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            OpenAddSupplierControlCommand = new RelayCommand<Object>(null, OpenAddSupplierControl); 
        }
        public void OpenAddSupplierControl(Object o = null)
        {
            AddSupplierControl addSupplierControl = new AddSupplierControl();
            addSupplierControl.DataContext = this;
            DialogHost.Show(addSupplierControl);

            ExitCommand = new RelayCommand<Object>(null, exit =>
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
            });

        }
    }
}

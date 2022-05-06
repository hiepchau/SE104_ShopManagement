using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    class SupplierControlViewModel : ViewModelBase
    {
        #region Properties
        public ProducerInformation producer { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string displayID { get; set; }
        private IUpdateSuplierList _parent;
        #endregion

        #region ICommand
        public ICommand DeleteSupplierCommand { get; set; }
        #endregion

        public SupplierControlViewModel(ProducerInformation producer, IUpdateSuplierList parent)
        {
            this.producer = producer;
            ID = producer.ID;
            Name = producer.Name;
            PhoneNumber = producer.PhoneNumber;
            displayID = producer.displayID;
            _parent = parent;

            DeleteSupplierCommand = new RelayCommand<Object>(null, DeleteSupplier);
        }

        #region Function
        public void DeleteSupplier(Object o)
        {
            _parent.UpdateSuplierList(producer);
        }
        #endregion
    }
}

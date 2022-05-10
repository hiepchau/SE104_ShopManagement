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
        public string Address { get; set; }
        public bool isActivated { get; set; }
        private IUpdateSuplierList _parent;
        #endregion

        #region
        public ICommand EditSupplierCommand { get; set; }
        public ICommand DeleteSupplierCommand { get; set; }
        #endregion

        public SupplierControlViewModel(ProducerInformation producer, IUpdateSuplierList parent)
        {
            this.producer = producer;
            ID = producer.ID;
            Name = producer.Name;
            PhoneNumber = producer.PhoneNumber;
            displayID = producer.displayID;
            isActivated = producer.isActivated;
            Address = producer.Address;
            Email = producer.Email;
            _parent = parent;
            EditSupplierCommand = new RelayCommand<Object>(null, EditSupplier);
            DeleteSupplierCommand = new RelayCommand<Object>(null, DeleteSupplier);
        }

        #region Function
        public void DeleteSupplier(Object o)
        {
            _parent.UpdateSuplierList(producer);
        }
        public void EditSupplier(Object o)
        {
            _parent.EditSupplier(producer);
        }
        #endregion
    }
}

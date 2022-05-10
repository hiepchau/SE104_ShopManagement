using MaterialDesignThemes.Wpf;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Components;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Insert_database;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.MenuViewModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using SE104_OnlineShopManagement.Services;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using SE104_OnlineShopManagement.Network.Update_database;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    public interface IUpdateCustomerList
    {
        void UpdateCustomerList(CustomerInformation cus);
        void EditCustomer(CustomerInformation cus);
    }
    class CustomerFunction : BaseFunction, IUpdateCustomerList
    {
        #region Properties
        public string customerName { get; set; }
        public string customerPhone { get; set; }
        public string customerCMND { get; set; }
        public string customerAddress { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        public int selectedItem { get; set; }
        public CustomerControlViewModel selectedCus { get; set; }
        public ObservableCollection<CustomerControlViewModel> listItemsCustomer { get; set; }
        public ObservableCollection<CustomerControlViewModel> listAllCustomer { get; set; }

        private ManagingFunctionsViewModel managingFunction;
        private CustomerSelectMenu customerSelectMenu;
        #endregion

        #region ICommand
        //Customer
        public ICommand LoadCustomerCommand { get; set; }
        public ICommand ExportExcelCommand { get; set; }
        public ICommand GoToNextPageCommandCus { get; set; }
        public ICommand GoToPreviousPageCommandCus { get; set; }
        public ICommand FindCustomerCommand { get; set; }
        public ICommand OpenAddCustomerControlCommand { get; set; }
        public ICommand SortCustomerCommand { get; set; }
        public ICommand CountCustomerCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand TextChangedCommand { get; set; }

        //Membership
        public ICommand OpenMemberShipControlCommand { get; set; }


        //AddCustomer
        public ICommand SaveCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        #endregion

        public CustomerFunction(AppSession session, MongoConnect connect, ManagingFunctionsViewModel managingFunctionsViewModel, CustomerSelectMenu _customerSelectMenu) : base(session, connect)
        {
            this._connection = connect;
            this._session=session;
            managingFunction = managingFunctionsViewModel;
            customerSelectMenu = _customerSelectMenu;
            listItemsCustomer = new ObservableCollection<CustomerControlViewModel>();
            listAllCustomer = new ObservableCollection<CustomerControlViewModel>();
            //Get Data
            GetData();
            GetAllData();
            //
            TextChangedCommand = new RelayCommand<Object>(null, TextChangedHandle);
            
            OpenAddCustomerControlCommand = new RelayCommand<Object>(null, OpenAddCustomerControl);
            OpenMemberShipControlCommand = new RelayCommand<Object>(null, OpenMemberShipControl);
        }

        #region Function
        public void OpenAddCustomerControl(Object o = null)
        {
            AddCustomerControl addCustomerControl = new AddCustomerControl();
            addCustomerControl.DataContext = this;
            DialogHost.Show(addCustomerControl);
            SaveCommand = new RelayCommand<Object>(CheckValidSave, SaveCustomer);
            ExitCommand = new RelayCommand<Object>(null, exit =>
            {
                SetNull();
                DialogHost.CloseDialogCommand.Execute(null, null);
            });

        }
        public void OpenMemberShipControl(Object o = null)
        {
            managingFunction.Currentdisplaying = new MembershipFunction(Session, Connect);
            customerSelectMenu.changeSelectedItem(1);
            managingFunction.CurrentDisplayPropertyChanged();
        }
        public void TextChangedHandle(Object o = null)
        {
            (SaveCommand as RelayCommand<Object>).OnCanExecuteChanged();
        }
        public bool CheckValidSave(Object o = null)
        {
            if(String.IsNullOrEmpty(customerName) || String.IsNullOrEmpty(customerPhone) || String.IsNullOrEmpty(customerAddress) || String.IsNullOrEmpty(customerCMND))
            {
                return false;
            }
            return true;
        }
        public void SetNull(Object o = null)
        {
            customerAddress = "";
            customerCMND = "";
            customerName = "";
            customerPhone = "";
            OnPropertyChanged(nameof(customerCMND));
            OnPropertyChanged(nameof(customerPhone));
            OnPropertyChanged(nameof(customerName));
            OnPropertyChanged(nameof(customerAddress));
        }
        public async void SaveCustomer(Object o = null)
        {
            if (selectedCus != null)
            {
                var filter = Builders<CustomerInformation>.Filter.Eq("ID", selectedCus.ID);
                var update = Builders<CustomerInformation>.Update.Set("Name", customerName).Set("Phone", customerPhone).Set("CMND",customerCMND).Set("Address",customerAddress);
                UpdateCustomerInformation updater = new UpdateCustomerInformation(_connection.client, _session, filter, update);
                var s = await updater.update();
                listItemsCustomer.Clear();
                GetData();
                OnPropertyChanged(nameof(listItemsCustomer));
            }
            else if (CheckExist() == false)
            {
                CustomerInformation info = new CustomerInformation("", customerName, customerPhone, "1", customerCMND, customerAddress, true, await new AutoCustomerIDGenerator(_session, _connection.client).Generate());
                RegisterCustomer regist = new RegisterCustomer(info, _connection.client, _session);
                string s = await regist.register();
                listItemsCustomer.Add(new CustomerControlViewModel(info, this));
                listAllCustomer.Add(new CustomerControlViewModel(info, this));
                OnPropertyChanged(nameof(listItemsCustomer));
                Console.WriteLine(s);
            }
            else
            {
                SetActive(selectedCus);
            }
            DialogHost.CloseDialogCommand.Execute(null, null);
            //Set Null
            SetNull();
        }

        public void UpdateCustomerList(CustomerInformation cus)
        {
            int i = 0;
            if (listItemsCustomer.Count > 0)
            {
                foreach (CustomerControlViewModel ls in listItemsCustomer)
                {
                    if (ls.customer.Equals(cus))
                    {
                        SetUnactive(ls);
                        listAllCustomer.Clear();
                        GetAllData();
                        break;
                    }
                    i++;
                }
                listItemsCustomer.RemoveAt(i);
                OnPropertyChanged(nameof(listItemsCustomer));
            }
            else
            {
                return;
            }
        }
        public void EditCustomer(CustomerInformation cus)
        {
            if (listItemsCustomer.Count > 0)
            {
                foreach (CustomerControlViewModel ls in listItemsCustomer)
                {
                    if (ls.customer.Equals(cus))
                    {
                        selectedCus = ls;
                        break;
                    }
                }
            }
            else
            {
                return;
            }
            customerName = selectedCus.Name;
            customerAddress = selectedCus.address;
            customerCMND = selectedCus.cmnd;
            customerPhone = selectedCus.PhoneNumber;
            OnPropertyChanged(nameof(customerCMND));
            OnPropertyChanged(nameof(customerPhone));
            OnPropertyChanged(nameof(customerName));
            OnPropertyChanged(nameof(customerAddress));
            OpenAddCustomerControl();
        }
        public async void SetActive(CustomerControlViewModel cusinfo)
        {
            var filter = Builders<CustomerInformation>.Filter.Eq("ID", cusinfo.ID);
            var update = Builders<CustomerInformation>.Update.Set("isActivated", true);
            UpdateCustomerInformation updater = new UpdateCustomerInformation(_connection.client, _session, filter, update);
            var s = await updater.update();
            listItemsCustomer.Add(selectedCus);
            OnPropertyChanged(nameof(listItemsCustomer));
            selectedCus=null;
            Console.WriteLine(s);
        }
        public async void SetUnactive(CustomerControlViewModel cusinfo)
        {
            if (cusinfo.isActivated == true)
            {
                var filter = Builders<CustomerInformation>.Filter.Eq("ID", cusinfo.ID);
                var update = Builders<CustomerInformation>.Update.Set("isActivated", false);
                UpdateCustomerInformation updater = new UpdateCustomerInformation(_connection.client, _session, filter, update);
                var s = await updater.update();
                Console.WriteLine(s);
                selectedCus = null;
            }
            else Console.WriteLine("Cant execute");
        }
        public bool CheckExist()
        {
            foreach (CustomerControlViewModel ls in listAllCustomer)
            {
                if (customerName == ls.Name && customerAddress == ls.address && customerCMND == ls.cmnd && customerPhone == ls.PhoneNumber)
                {
                    selectedCus = ls;
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region DB
        public async void GetData()
        {
            var filter = Builders<CustomerInformation>.Filter.Eq("isActivated",true);
            GetCustomer getter = new GetCustomer(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (CustomerInformation cus in ls)
            {
                listItemsCustomer.Add(new CustomerControlViewModel(cus, this));
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(listItemsCustomer));
        }
        public async void GetAllData()
        {
            var filter = Builders<CustomerInformation>.Filter.Empty;
            GetCustomer getter = new GetCustomer(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (CustomerInformation cus in ls)
            {
                listAllCustomer.Add(new CustomerControlViewModel(cus, this));
            }
            Console.Write("Executed");
        }

        #endregion
    }
}

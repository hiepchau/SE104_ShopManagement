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

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    class CustomerFunction : BaseFunction
    {
        #region Properties
        public string customerName { get; set; }
        public string customerPhone { get; set; }
        public string customerCMND { get; set; }
        public string customerAddress { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        public int selectedItem { get; set; }
        public ObservableCollection<CustomerInformation> listItemsCustomer { get; set; }


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
            listItemsCustomer = new ObservableCollection<CustomerInformation>();
            //Test
            GetData();
            listItemsCustomer.Add(new CustomerInformation("12", "Hip", "0123456789", "1","123"));
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

        public async void SaveCustomer(Object o = null)
        {
            CustomerInformation info = new CustomerInformation(await new AutoCustomerIDGenerator(_session,_connection.client).Generate(), 
                customerName,customerPhone,"1",customerCMND);
            RegisterCustomer regist = new RegisterCustomer(info, _connection.client, _session);
            string s = await regist.register();
            listItemsCustomer.Add(info);
            OnPropertyChanged(nameof(listItemsCustomer));
            Console.WriteLine(s);
        }
        #endregion

        #region DB
        public async void GetData()
        {
            var filter = Builders<CustomerInformation>.Filter.Empty;
            GetCustomer getter = new GetCustomer(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (CustomerInformation pro in ls)
            {
                listItemsCustomer.Add(pro);
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(listItemsCustomer));
        }
        #endregion
    }
}

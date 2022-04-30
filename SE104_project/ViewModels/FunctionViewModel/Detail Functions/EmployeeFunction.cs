using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Insert_database;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using SE104_OnlineShopManagement.Components;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using MongoDB.Driver;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    class EmployeeFunction : BaseFunction
    {
        #region Properties
        public string name { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string Password { get; set; }
        public string phoneNumber { get; set; }
        public string role { get; set; }
        public bool isMen { get; set; }
        public bool isGirl { get; set; }
        public long salary { get; set; }
        public DateTime BeginDate { get; set; }
        public ObservableCollection<UserInfomation> listItemsUserInfo { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        #endregion

        #region ICommand
        public ICommand OpenAddEmployeeControlCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        //AddEmployee
        public ICommand ExitCommand { get; set; }
        #endregion
        public EmployeeFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._connection = connect;
            this._session = session;
            isMen = true;
            //Item
            listItemsUserInfo = new ObservableCollection<UserInfomation>();
            GetData();
            listItemsUserInfo.Add(new UserInfomation("1", "Nguyen Huy Tri", "Dung", "dungxautrai@gmail.com", "1234556", "012345678", "None", 0, 0, 123456, new DateTime(2002, 2, 22)));
            //
            OpenAddEmployeeControlCommand = new RelayCommand<Object>(null, OpenAddEmployeeControl);
            CancelCommand = new RelayCommand<Object>(null, Cancel);
            SaveCommand= new RelayCommand<Object>(null,SaveUser);
        }
        public void OpenAddEmployeeControl(object o = null)
        {
            AddEmployeeControl addEmployeeControl = new AddEmployeeControl();
            addEmployeeControl.DataContext = this;
            DialogHost.Show(addEmployeeControl);
            ExitCommand = new RelayCommand<Object>(null, exit =>
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
            });
        }
        public void Cancel(object o = null)
        {
            Console.WriteLine("Executed!");
        }
        public async void SaveUser(object o)
        {
            var pass = o as PasswordBox;
            Password = pass.Password;
            var userRole = Role.Employee;
            var userGender = Gender.male;
            if(isMen==true) { userGender = Gender.male; }
            else if (isGirl==true) { userGender = Gender.female; }
            if(role== "Chủ sở hữu") { userRole = Role.Owner; }
            else if (role== "Quản lí") { userRole = Role.Manager; }
            else if (role== "Nhân viên") { userRole= Role.Employee; }
            UserInfomation info = new UserInfomation("",name,"","",Password,phoneNumber,"None",userRole,userGender,salary,BeginDate);
            RegisterUser regist = new RegisterUser(info, _connection.client);
            string s = await regist.registerUser();
            listItemsUserInfo.Add(info);
            OnPropertyChanged(nameof(listItemsUserInfo));
            Console.WriteLine(s);
        }
        public async void GetData()
        {
            var filter = Builders<UserInfomation>.Filter.Empty;
            GetUsers getter = new GetUsers(_connection.client, _session, filter);
            var ls = await getter.get();
            foreach (UserInfomation pro in ls)
            {
                listItemsUserInfo.Add(pro);
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(listItemsUserInfo));
        }
    }
}

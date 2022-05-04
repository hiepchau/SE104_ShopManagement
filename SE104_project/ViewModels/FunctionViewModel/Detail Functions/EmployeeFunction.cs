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
using SE104_OnlineShopManagement.Services;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using SE104_OnlineShopManagement.Models;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    public interface IUpdateEmployeeList
    {
        void UpdateEmployeeList(UserInfomation user);
    }
    class EmployeeFunction : BaseFunction, IUpdateEmployeeList
    {
        #region Properties
        public string name { get; set; }
        public string userName { get; set; }
        public string userEmail { get; set; }
        public string Password { get; set; }
        public string userPhoneNumber { get; set; }
        public string role { get; set; }
        public int SelectedRoleIndex { get; set; }
        public bool isMen { get; set; }
        public bool isGirl { get; set; }
        public long userSalary { get; set; }
        public BitmapImage employeeImage { get; set; }
        public string BeginDate { get; set; }
        public ObservableCollection<EmployeeControlViewModel> listItemsUserInfo { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        #endregion

        #region ICommand
        public ICommand OpenAddEmployeeControlCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        //AddEmployee
        public ICommand ExitCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand TextChangedCommand { get; set; }
        #endregion
        public EmployeeFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._connection = connect;
            this._session = session;
            SelectedRoleIndex = -1;
            isMen = true;
            
            //Item
            listItemsUserInfo = new ObservableCollection<EmployeeControlViewModel>();
            GetData();
            listItemsUserInfo.Add(new EmployeeControlViewModel(new UserInfomation("1", "Nguyen Huy Tri", "Dung", "dungxautrai@gmail.com",
                "1234556", "012345678", "None", 0, 0, 123456, new DateTime(2002, 2, 22)), this));
            //
            OpenAddEmployeeControlCommand = new RelayCommand<Object>(null, OpenAddEmployeeControl);
            CancelCommand = new RelayCommand<Object>(null, Cancel);
            SaveCommand= new RelayCommand<Object>(CheckValidSave, SaveUser);
            TextChangedCommand = new RelayCommand<Object>(null, TextChangedHandle);
            SelectImageCommand = new RelayCommand<Object>(null, SaveImage);
        }
        #region Function
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
        public void TextChangedHandle(Object o)
        {
            (SaveCommand as RelayCommand<Object>).OnCanExecuteChanged();
        }
        public bool CheckValidSave(object o)
        {
            var pass = o as PasswordBox;
            Password = pass.Password;
   
            

            if (String.IsNullOrEmpty(userEmail) || String.IsNullOrEmpty(userName)
                || String.IsNullOrEmpty(Password)
                || String.IsNullOrEmpty(BeginDate)
                || SelectedRoleIndex == -1
                || String.IsNullOrEmpty(userPhoneNumber) || String.IsNullOrEmpty(userSalary.ToString()))
            {
                return false;
            }
            return true;
        }
        public async void SaveUser(object o)
        {
            var pass = o as PasswordBox;
            Password = pass.Password;
            var userRole = Role.Employee;
            var userGender = Gender.male;
            if(isMen==true) { userGender = Gender.male; }
            else if (isGirl==true) { userGender = Gender.female; }
            if(role == "Chủ sở hữu") { userRole = Role.Owner; }
            else if (role == "Quản lí") { userRole = Role.Manager; }
            else if (role == "Nhân viên") { userRole= Role.Employee; }

            //Split Lastname and name
            string splitName = name.Trim();
            userName = splitName.Substring(splitName.LastIndexOf(' ') + 1);
            string _name = splitName.Substring(0, splitName.LastIndexOf(' '));

            UserInfomation info = new UserInfomation(await new AutoEmployeeIDGenerator(_session, _connection.client).Generate()
                , _name, userName, userEmail, Password, userPhoneNumber, _session.CurrnetUser.companyInformation, userRole, userGender, userSalary, DateTime.Parse(BeginDate));
            RegisterUser regist = new RegisterUser(info, _connection.client);
            string s = await regist.registerUser();
            listItemsUserInfo.Add(new EmployeeControlViewModel(info ,this));
            OnPropertyChanged(nameof(listItemsUserInfo));
            Console.WriteLine(s);
        }

        public async void SaveImage(object o = null)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "image jpeg(*.jpg)|*.jpg|image png(*.png)|*.png";
            ofd.DefaultExt = ".jpeg";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                BitmapImage tmp = new BitmapImage(new Uri(ofd.FileName));
                ByteImage bimg = new ByteImage(name, tmp);
                RegisterByteImage regist = new RegisterByteImage(bimg, _connection.client, _session);
                employeeImage = tmp;
                await regist.register();
                OnPropertyChanged(nameof(employeeImage));
            }
        }
        public void UpdateEmployeeList(UserInfomation user)
        {
            int i = 0;
            if (listItemsUserInfo.Count > 0)
            {
                foreach (EmployeeControlViewModel ls in listItemsUserInfo)
                {
                    if (ls.user.Equals(user))
                    {
                        break;
                    }
                    i++;
                }
                listItemsUserInfo.RemoveAt(i);
                OnPropertyChanged(nameof(listItemsUserInfo));
            }
            else
            {
                return;
            }
        }

        #endregion
        #region DB
        public async void GetData()
        {
            var filter = Builders<UserInfomation>.Filter.Empty;
            GetUsers getter = new GetUsers(_connection.client, _session, filter);
            var ls = await getter.get();
            foreach (UserInfomation user in ls)
            {
                listItemsUserInfo.Add(new EmployeeControlViewModel(user, this));
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(listItemsUserInfo));
        }
        #endregion
    }
}

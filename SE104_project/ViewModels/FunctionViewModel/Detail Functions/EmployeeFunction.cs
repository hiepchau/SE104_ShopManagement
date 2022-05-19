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

using SE104_OnlineShopManagement.Models;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using SE104_OnlineShopManagement.Network.Update_database;
using System.Linq;
using System.Windows;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    public interface IUpdateEmployeeList
    {
        void UpdateEmployeeList(UserInfomation user);
        void EditEmployee(UserInfomation user);
    }
    class EmployeeFunction : BaseFunction, IUpdateEmployeeList
    {
        #region Properties
        public string userName { get; set; }
        public string userEmail { get; set; }
        public string Password { get; set; }
        public string userPhoneNumber { get; set; }
        public string role { get; set; }
        public int IsSelectedIndex { get; set; }
        public bool isMen { get; set; }
        public bool isGirl { get; set; }
        public long userSalary { get; set; }
        public BitmapImage employeeImage { get; set; }
        public string BeginDate { get; set; }
        private Role userRole { get; set; }
        private Gender userGender { get; set; }
        public ObservableCollection<EmployeeControlViewModel> listItemsUserInfo { get; set; }
        public ObservableCollection<EmployeeControlViewModel> listUnactiveUserInfo { get; set; }
        public EmployeeControlViewModel selectedUser { get; set; }
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
        public ICommand SetActiveCommand { get; set; }
        public ICommand SetUnactiveCommand { get; set; }
        #endregion
        public EmployeeFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._connection = connect;
            this._session = session;
            IsSelectedIndex = -1;
            isMen = true;
            
            //Item
            listItemsUserInfo = new ObservableCollection<EmployeeControlViewModel>();
            listUnactiveUserInfo = new ObservableCollection<EmployeeControlViewModel>();
            GetData();
            GetUnactiveData();
            //
            OpenAddEmployeeControlCommand = new RelayCommand<Object>(null, OpenAddEmployeeControl);
            CancelCommand = new RelayCommand<Object>(null, Cancel);
            SaveCommand= new RelayCommand<Object>(CheckValidSave, SaveUser);
            TextChangedCommand = new RelayCommand<Object>(null, TextChangedHandle);
            SelectImageCommand = new RelayCommand<Object>(null, SaveImage);
            SetActiveCommand = new RelayCommand<Object>(null, SetActive);
            SetUnactiveCommand = new RelayCommand<Object>(null, SetUnactive);
        }
        #region Function
        public void OpenAddEmployeeControl(object o = null)
        {
            AddEmployeeControl addEmployeeControl = new AddEmployeeControl();
            addEmployeeControl.DataContext = this;
            DialogHost.Show(addEmployeeControl);
            ExitCommand = new RelayCommand<Object>(null, exit =>
            {
                SetNull();
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
            if (employeeImage == null || String.IsNullOrEmpty(userEmail) || String.IsNullOrEmpty(userName)
                || String.IsNullOrEmpty(Password)
                || String.IsNullOrEmpty(BeginDate)
                || IsSelectedIndex == -1
                || userPhoneNumber.Length != 10
                || String.IsNullOrEmpty(userSalary.ToString())
                )
            {
                return false;
            }
            return true;
        }
        public async void SaveUser(object o)
        {
            var pass = o as PasswordBox;
            Password = pass.Password;
            if(isMen==true) { userGender = Gender.male; }
            else if (isGirl==true) { userGender = Gender.female; }
            if(IsSelectedIndex == 0) { userRole = Role.Owner; }
            else if (IsSelectedIndex==1) { userRole = Role.Manager; }
            else if (IsSelectedIndex == 2) { userRole= Role.Employee; }

            //Split Lastname and name
            string _lastname, _name;
            string splitName = userName.Trim();
            if (splitName.Contains(' ') == false) { _lastname = splitName; _name = ""; }
            else
            {
                _lastname = splitName.Substring(splitName.LastIndexOf(' ') + 1);
                _name = splitName.Substring(0, splitName.LastIndexOf(' '));
            }
            if (selectedUser != null)
            {
                var filter = Builders<UserInfomation>.Filter.Eq("ID", selectedUser.ID);
                var update = Builders<UserInfomation>.Update
                    .Set("UserEmail", userEmail)
                    .Set("UserFirstName", _name)
                    .Set("UserLastName", _lastname)
                    .Set("UserPhoneNumber", userPhoneNumber)
                    .Set("UserRole", userRole)
                    .Set("UserGender", userGender)
                    .Set("UserSalary", userSalary)
                    .Set("UserBirthday", DateTime.Parse(BeginDate));
                UpdateUserInformation updater = new UpdateUserInformation(_connection.client,_session,filter,update);
                var s = updater.update();
                //Update Image
                ByteImage bimg = new ByteImage(selectedUser.ID, employeeImage);
                var filterImage = Builders<ByteImage>.Filter.Eq("ID", selectedUser.ID);
                var updateImage = Builders<ByteImage>.Update.Set("data", bimg);
                UpdateImage updaterImage = new UpdateImage(_connection.client, _session, filterImage, updateImage);
                var p = updaterImage.update();
                listItemsUserInfo.Clear();
                GetData();
                OnPropertyChanged(nameof(listItemsUserInfo));
            }
            else if (employeeImage != null)
            {
                //Register UserInformation
                UserInfomation info = new UserInfomation("", _name, _lastname, userEmail, Password, userPhoneNumber, _session.CurrnetUser.companyInformation, userRole, userGender, userSalary, DateTime.Parse(BeginDate), true, await new AutoEmployeeIDGenerator(_session, _connection.client).Generate());
                RegisterUser regist = new RegisterUser(info, _connection.client);
                string id = await regist.registerUser();
                listItemsUserInfo.Add(new EmployeeControlViewModel(info, this));

                //Register Image
                ByteImage bimg = new ByteImage(id, employeeImage);
                RegisterByteImage registImage = new RegisterByteImage(bimg, _connection.client, _session);
                await registImage.register();
                OnPropertyChanged(nameof(listItemsUserInfo));
                Console.WriteLine(id);
            }
            //Set Null
            SetNull();
            DialogHost.CloseDialogCommand.Execute(null, null);
            CustomMessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);

        }

        public void SaveImage(object o = null)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "image jpeg(*.jpg)|*.jpg|image png(*.png)|*.png";
            ofd.DefaultExt = ".jpeg";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                BitmapImage tmp = new BitmapImage(new Uri(ofd.FileName));               
                employeeImage = tmp;
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
                    if (ls.user.ID.Equals(user.ID))
                    {
                        SetUnactive(ls);
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
        public void EditEmployee(UserInfomation user)
        {
            if (listItemsUserInfo.Count > 0)
            {
                foreach (EmployeeControlViewModel ls in listItemsUserInfo)
                {
                    if (ls.user.Equals(user))
                    {
                        selectedUser = ls;
                        break;
                    }
                }
            }
            else
            {
                return;
            }
            if (selectedUser.role == Role.Owner) { IsSelectedIndex = 0; role = "Chủ sở hữu"; }
            else if (selectedUser.role == Role.Manager) { IsSelectedIndex = 1; role = "Quản lí"; }
            else if (selectedUser.role == Role.Employee) { IsSelectedIndex = 2; role = "Nhân viên"; }
            if(selectedUser.gender == Gender.male) { isMen = true; isGirl = false; }
            else if (selectedUser.gender == Gender.female) { isGirl = true; isMen = false; }
            userName=selectedUser.FirstName + " " + selectedUser.LastName;
            userEmail=selectedUser.Email;
            userPhoneNumber = selectedUser.PhoneNumber;
            userSalary = selectedUser.salary;
            BeginDate = selectedUser.birthday.ToShortDateString();
            GetImage(selectedUser);
            OnPropertyChanged(nameof(userEmail));
            OnPropertyChanged(nameof(userName));
            OnPropertyChanged(nameof(userPhoneNumber));
            OnPropertyChanged(nameof(userSalary));
            OnPropertyChanged(nameof(IsSelectedIndex));
            OnPropertyChanged(nameof(isGirl));
            OnPropertyChanged(nameof(isMen));
            OnPropertyChanged(nameof(BeginDate));
            OpenAddEmployeeControl();
        }
        public async void GetImage(EmployeeControlViewModel emp)
        {
            GetByteImage getter = new GetByteImage(_connection.client, _session, Builders<ByteImage>.Filter.Eq(p => p.obID, emp.ID));
            var ls = await getter.Get();
            if (ls.Count > 0)
            {
                employeeImage = ls.FirstOrDefault().convertByteToImage();
                OnPropertyChanged(nameof(employeeImage));
            }
        }
        public async void SetActive(object o = null)
        {
            var result = CustomMessageBox.Show("Nhân viên này sẽ hoạt động trở lại ?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (selectedUser != null && selectedUser.isActivated == false)
                {
                    var filter = Builders<UserInfomation>.Filter.Eq("ID", selectedUser.ID);
                    var update = Builders<UserInfomation>.Update.Set("isActivated", true);
                    UpdateUserInformation updater = new UpdateUserInformation(_connection.client, _session, filter, update);
                    var s = await updater.update();
                    listItemsUserInfo.Add(selectedUser);
                    selectedUser.isActivated = true;
                    listUnactiveUserInfo.Remove(selectedUser);
                    OnPropertyChanged(nameof(listItemsUserInfo));
                    OnPropertyChanged(nameof(listUnactiveUserInfo));
                    Console.WriteLine(s);
                    selectedUser = null;
                }
                else
                {
                    CustomMessageBox.Show("Nhân viên này đang hoạt động!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    Console.WriteLine("Cant execute");
                }
            }
            else return;
        }
        public async void SetUnactive(object o = null)
        {
            var result = CustomMessageBox.Show("Nhân viên này sẽ ngừng hoạt động ?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (selectedUser != null && selectedUser.isActivated == true)
                {
                    var filter = Builders<UserInfomation>.Filter.Eq("ID", selectedUser.ID);
                    var update = Builders<UserInfomation>.Update.Set("isActivated", false);
                    UpdateUserInformation updater = new UpdateUserInformation(_connection.client, _session, filter, update);
                    var s = await updater.update();
                    listUnactiveUserInfo.Add(selectedUser);
                    selectedUser.isActivated = false;
                    listItemsUserInfo.Remove(selectedUser);
                    OnPropertyChanged(nameof(listItemsUserInfo));
                    OnPropertyChanged(nameof(listUnactiveUserInfo));
                    Console.WriteLine(s);
                    selectedUser = null;
                }
                else
                {
                    CustomMessageBox.Show("Nhân viên này đang ngừng hoạt động!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    Console.WriteLine("Cant execute");
                }
            }
            else return;
        }

        public void SetNull()
        {
            userEmail = "";
            userName = "";
            userPhoneNumber = "";
            userSalary = 0;
            IsSelectedIndex = -1;
            isGirl = false;
            userGender = Gender.Empty;
            userRole = Role.Empty;
            BeginDate = "";
            employeeImage = null;
            OnPropertyChanged(nameof(userEmail));
            OnPropertyChanged(nameof(userName));
            OnPropertyChanged(nameof(userPhoneNumber));
            OnPropertyChanged(nameof(userSalary));
            OnPropertyChanged(nameof(IsSelectedIndex));
            OnPropertyChanged(nameof(isGirl));
            OnPropertyChanged(nameof(userGender));
            OnPropertyChanged(nameof(userRole));
            OnPropertyChanged(nameof(BeginDate));
            OnPropertyChanged(nameof(employeeImage));
        }
        public void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        public void NumberValidationTextBox(object sender, System.Windows.Input.KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }

        #endregion
        #region DB
        public async void GetUnactiveData()
        {
            var filter = Builders<UserInfomation>.Filter.Eq("isActivated", false);
            GetUsers getter = new GetUsers(_connection.client, _session, filter);
            var ls = await getter.get();
            foreach (UserInfomation user in ls)
            {
                listUnactiveUserInfo.Add(new EmployeeControlViewModel(user, this));
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(listUnactiveUserInfo));
        }
        public async void GetData()
        {
            var filter = Builders<UserInfomation>.Filter.Eq("isActivated", true);
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

using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Views.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Insert_database;
using System.Windows.Controls;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Services;
using System.Windows;

namespace SE104_OnlineShopManagement.ViewModels.Authentication
{
    public class RegisterViewModel: ViewModelBase
    {
        #region properties
        private IViewModelFactory _factory;
        private UserInfomation user;
        private CompanyInformation company;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string birthDay { get; set; } 
        public Gender gender { get; set; }
        public long salary;
        public string ComName { get; set; }
        private MongoConnect DBConnection;
        #endregion

        #region Commands
        public ICommand BackCommand { get; private set; }
        public ICommand RegisterCommand { get; private set;}
        public ICommand GenderSelectCommand { get; private set; }
        #endregion

        public RegisterViewModel(IViewModelFactory factory, MongoConnect connect)
        {
            _factory =  factory;
            gender = Gender.Empty;
            this.DBConnection = connect;
            BackCommand = new RelayCommand<object>(null, Back2Login);
            RegisterCommand = new RelayCommand<object>(null, Register);
            GenderSelectCommand=new RelayCommand<object>(null, GenderSelect);
        }
        public void Back2Login(object o=null)
        {
            _factory.CreateViewModel<MainViewModel>().CurrentMainViewModel = _factory.CreateViewModel<LoginViewModel>();
            //authenticationWindow.DataContext = _factory.CreateViewModel<MainViewModel>();
        }

        public async void Register(object o)
        {
            var ob = (object[])o;
            var pass1 = ob[0] as PasswordBox;
            var pass2 = ob[1] as PasswordBox;
            var genbox = ob[3] as ComboBox;
            var datepick = ob[2] as DatePicker;
            if(pass1 != null && pass2 != null)
            {
                if (pass1.Password != pass2.Password)
                {
                    CustomMessageBox.Show("Mật khẩu không trùng khớp", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);

                    Console.WriteLine("Password not matched");
                    return;

                }
                else Password= pass1.Password;
            }
            if (String.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) || String.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ComName)||string.IsNullOrWhiteSpace(birthDay) || gender==Gender.Empty)
            {
                CustomMessageBox.Show("Nhập thiếu thông tin", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine("Update failed");
                return;
            }
            var dblist = DBConnection.client.ListDatabaseNames().ToList();
            foreach (var db in dblist)
            {
                if (String.Equals(db,ComName))
                {
                    Console.WriteLine("Company already existed!");
                    CustomMessageBox.Show("Tên công ty đã tồn tại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;
                }
            }
            company = new CompanyInformation(Guid.NewGuid().ToString(), ComName, "","", "", "", "");
            user = new UserInfomation("Owner",FirstName,LastName,Email,Password,"0",company.Name,Role.Owner,gender,salary,DateTime.ParseExact(birthDay,"dd/mm/yyyy",null),DateTime.Today);
            RegisterUser regist= new RegisterUser(user,DBConnection.client);
            string s = await regist.registerUser();
            FirstName = "";
            LastName = "";
            Email = "";
            pass1.Password = "";
            pass2.Password = "";
            ComName = "";
            genbox.SelectedIndex = -1;
            datepick.Text = "";
            OnPropertyChanged(FirstName,LastName, Email,ComName);
            CustomMessageBox.Show("Đăng ký thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);

        }
        private void GenderSelect(object o)
        {
            if (o != null)
            {
                var tmp = o as TextBlock;
                if (tmp.Name == "MaleBox")
                    gender = Gender.male;
                else if (tmp.Name == "FemaleBox")
                    gender = Gender.female;
                else
                    gender = Gender.other;
            }
        }
    }
}

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
        public string ComName { get; set; }
        private MongoConnect DBConnection;
        #endregion

        #region Commands
        public ICommand BackCommand { get; private set; }
        public ICommand RegisterCommand { get; private set;}
        #endregion

        public RegisterViewModel(IViewModelFactory factory, MongoConnect connect)
        {
            _factory =  factory;
            this.DBConnection = connect;
            BackCommand = new RelayCommand<object>(null, Back2Login);
            RegisterCommand = new RelayCommand<object>(null, Register);
        }
        public void Back2Login(object o=null)
        {
            _factory.CreateViewModel<MainViewModel>().CurrentMainViewModel = _factory.CreateViewModel<LoginViewModel>();
            //authenticationWindow.DataContext = _factory.CreateViewModel<MainViewModel>();
        }

        public void Register(object o)
        {
            var ob = (object[])o;
            var pass1 = ob[0] as PasswordBox;
            var pass2 = ob[1] as PasswordBox;
            if(pass1 != null && pass2 != null)
            {
                if (pass1.Password != pass2.Password)
                {
                    Console.WriteLine("Password not matched");
                    return;

                }
                else Password= pass1.Password;
            }
            if (String.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) || String.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ComName))
            {
                Console.WriteLine("Update failed");
                return;
            }
            var dblist = DBConnection.client.ListDatabaseNames().ToList();
            foreach (var db in dblist)
            {
                if (String.Equals(db,ComName))
                {
                    Console.WriteLine("Company already existed!");
                    return;
                }
            }
            company = new CompanyInformation(Guid.NewGuid().ToString(), ComName);
            user = new UserInfomation(Guid.NewGuid().ToString(),FirstName,LastName,Email,Password,"0",company.Name,Role.Owner);
            RegisterUser regist= new RegisterUser(user,DBConnection.client);
            regist.registerUser();
        }
    }
}

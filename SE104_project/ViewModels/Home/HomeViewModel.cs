using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using System.Windows.Input;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.ViewModels.Authentication;

namespace SE104_OnlineShopManagement.ViewModels.Home
{
    public class HomeViewModel : ViewModelBase
    {
        private ViewModelBase _currentFunction;
        private IViewModelFactory _factory;
        public ICommand testCommand { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        public ViewModelBase CurrentFunction { get => _currentFunction;set => _currentFunction = value; }
        public HomeViewModel(IViewModelFactory factory, AppSession session,MongoConnect connect)
        {
            this._factory= factory;
            this._connection = connect;
            this._session= session;
            testCommand = new RelayCommand<object>(null, testing);
        }
        private void testing(object o = null)
        {
            Console.WriteLine("Testcommand executed!");
        }
    }
}

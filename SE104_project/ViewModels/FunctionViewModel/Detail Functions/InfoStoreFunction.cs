using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Insert_database;
using SE104_OnlineShopManagement.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    class InfoStoreFunction : BaseFunction
    {
        private MongoConnect _connection;
        private AppSession _session;

        #region ICommand
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        #endregion
        public InfoStoreFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._connection = connect;
            this._session = session;
            CancelCommand = new RelayCommand<Object>(null, Cancel);
            SaveCommand = new RelayCommand<Object>(null, SaveUser);
        }
        public void Cancel(object o = null)
        {
            Console.WriteLine("Executed!");
        }
        public void SaveUser(object parameter)
        {
            var values = (object[])parameter;
            Console.WriteLine(values[0].ToString());
            Console.WriteLine(values[1].ToString());
            Console.WriteLine(values[2].ToString());
            Console.WriteLine(values[3].ToString());
        }
    }
}

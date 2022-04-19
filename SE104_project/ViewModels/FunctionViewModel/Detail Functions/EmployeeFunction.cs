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
    class EmployeeFunction : BaseFunction
    {
        public List<UserInfomation> listItemsUserInfo { get; set; }
        private MongoConnect _connection;
        private AppSession _session;

        #region ICommand
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        #endregion
        public EmployeeFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._connection = connect;
            this._session = session;
            //Item
            listItemsUserInfo = new List<UserInfomation>();
            listItemsUserInfo.Add(new UserInfomation("1", "Nguyen Huy Tri", "Dung", "dungxautrai@gmail.com", "1234556", "012345678", "None", 0, 0, new DateTime(2002, 2, 22)));
            //
            CancelCommand = new RelayCommand<Object>(null, Cancel);
            SaveCommand=new RelayCommand<Object>(null,SaveUser);
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
            //Console.WriteLine(values[3].ToString());
        }
    }
}

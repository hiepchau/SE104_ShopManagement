using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Insert_database;
using SE104_OnlineShopManagement.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using SE104_OnlineShopManagement.Components;
using MaterialDesignThemes.Wpf;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    class EmployeeFunction : BaseFunction
    {
        public List<UserInfomation> listItemsUserInfo { get; set; }
        private MongoConnect _connection;
        private AppSession _session;

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
            //Item
            listItemsUserInfo = new List<UserInfomation>();
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

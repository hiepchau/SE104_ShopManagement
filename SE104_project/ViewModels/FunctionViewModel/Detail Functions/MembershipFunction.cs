using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Network.Insert_database;
using SE104_OnlineShopManagement.Network.Get_database;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    class MembershipFunction : BaseFunction
    {
        public string membershipname { get; set; }
        public int priority { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        #region ICommand
        public ICommand SaveCommand { get; set; }
        #endregion
        public MembershipFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._session = session;
            this._connection = connect;
            SaveCommand = new RelayCommand<Object>(null, SaveMemberShip);
        }
        public void SaveMemberShip(object o = null)
        {
            MembershipInformation info = new MembershipInformation("", membershipname, priority);
            RegisterMembership regist = new RegisterMembership(info, _connection.client, _session);
            string s = regist.register();
            Console.WriteLine(s);
            //var filter = Builders<MembershipInformation>.Filter.Empty;
            //GetMembership getter = new GetMembership(_connection.client, _session, filter);
            //var ls = getter.Get();
            //foreach (MembershipInformation pro in ls)
            //{
            //    Console.WriteLine(pro.name);
            //}
        }
    }
}

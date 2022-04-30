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
using System.Collections.ObjectModel;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    class MembershipFunction : BaseFunction
    {
        #region properties
        public string membershipname { get; set; }
        public int priority { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        public ObservableCollection<MembershipInformation> listMemberShip { get; set; }
        #endregion
        #region ICommand
        public ICommand SaveCommand { get; set; }
        #endregion
        public MembershipFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._session = session;
            this._connection = connect;
            listMemberShip = new ObservableCollection<MembershipInformation>();
            GetData();
            SaveCommand = new RelayCommand<Object>(null, SaveMemberShip);
        }
        public async void SaveMemberShip(object o = null)
        {
            MembershipInformation info = new MembershipInformation("", membershipname, priority);
            RegisterMembership regist = new RegisterMembership(info, _connection.client, _session);
            string s = await regist.register();
            listMemberShip.Add(info);
            OnPropertyChanged(nameof(listMemberShip));
            Console.WriteLine(s);
        }
        public async void GetData()
        {
            var filter = Builders<MembershipInformation>.Filter.Empty;
            GetMembership getter = new GetMembership(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (MembershipInformation pro in ls)
            {
                listMemberShip.Add(pro);
            }
            OnPropertyChanged(nameof(listMemberShip));

        }
    }
}

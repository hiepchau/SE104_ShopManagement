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
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    public interface IUpdateMembershipList
    {
        void UpdateMembershipList(MembershipInformation mem);
    }
    class MembershipFunction : BaseFunction, IUpdateMembershipList
    {
        #region Properties
        public string membershipname { get; set; }
        public int priority { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        public ObservableCollection<MembershipControlViewModel> listMembership { get; set; }
        #endregion
        #region ICommand
        public ICommand SaveCommand { get; set; }
        #endregion
        public MembershipFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._session = session;
            this._connection = connect;
            listMembership = new ObservableCollection<MembershipControlViewModel>();
            GetData();
            //Test
            listMembership.Add(new MembershipControlViewModel(new MembershipInformation("1", "Vàng", 10), this));
            //
            SaveCommand = new RelayCommand<Object>(null, SaveMemberShip);
        }
        public bool CheckExist()
        {
            foreach (MembershipControlViewModel ls in listMembership)
            {
                if (membershipname == ls.name)
                {
                    return true;
                }
            }
            return false;
        }

        #region Function
        public async void SaveMemberShip(object o = null)
        {
            if (CheckExist() == false)
            {
                MembershipInformation info = new MembershipInformation("", membershipname, priority);
                RegisterMembership regist = new RegisterMembership(info, _connection.client, _session);
                string s = await regist.register();
                listMembership.Add(new MembershipControlViewModel(info, this));
                OnPropertyChanged(nameof(listMembership));
                Console.WriteLine(s);
            }
            else
            {
                Console.WriteLine("MembershipName has existed!");
            }
        }
        public void UpdateMembershipList(MembershipInformation mem)
        {
            int i = 0;
            if (listMembership.Count > 0)
            {
                foreach (MembershipControlViewModel ls in listMembership)
                {
                    if (ls.membership.Equals(mem))
                    {
                        break;
                    }
                    i++;
                }
                listMembership.RemoveAt(i);
                OnPropertyChanged(nameof(listMembership));
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
            var filter = Builders<MembershipInformation>.Filter.Empty;
            GetMembership getter = new GetMembership(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (MembershipInformation mem in ls)
            {
                listMembership.Add(new MembershipControlViewModel(mem, this));
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(listMembership));

        }

        #endregion
    }
}

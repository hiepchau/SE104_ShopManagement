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
using SE104_OnlineShopManagement.Network.Update_database;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    public interface IUpdateMembershipList
    {
        void UpdateMembershipList(MembershipInformation mem);
        void EditMembership(MembershipInformation mem);
    }
    class MembershipFunction : BaseFunction, IUpdateMembershipList
    {
        #region Properties
        public string membershipname { get; set; }
        public int priority { get; set; }
        private MembershipControlViewModel selectedMembership { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        public ObservableCollection<MembershipControlViewModel> listActiveMembership { get; set; }
        public ObservableCollection<MembershipControlViewModel> listAllMembership { get; set; }
        #endregion
        #region ICommand
        public ICommand SaveCommand { get; set; }
        #endregion
        public MembershipFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._session = session;
            this._connection = connect;
            listActiveMembership = new ObservableCollection<MembershipControlViewModel>();
            listAllMembership = new ObservableCollection<MembershipControlViewModel>();
            GetActiveData();
            GetAllData();
            //Test
            listActiveMembership.Add(new MembershipControlViewModel(new MembershipInformation("1", "Vàng", 10), this));
            //
            SaveCommand = new RelayCommand<Object>(null, SaveMemberShip);
        }
        public bool CheckExist()
        {
            foreach (MembershipControlViewModel ls in listAllMembership)
            {
                if (membershipname == ls.name && priority == ls.prio)
                {
                    selectedMembership = ls;
                    return true;
                }
            }
            return false;
        }

        #region Function
        public async void SaveMemberShip(object o = null)
        {
            if (selectedMembership!=null)
            {
                var filter = Builders<MembershipInformation>.Filter.Eq("ID", selectedMembership.ID);
                var update = Builders<MembershipInformation>.Update.Set("MembershipName", membershipname).Set("Priority", priority);
                UpdateMembershipInformation updater = new UpdateMembershipInformation(_connection.client, _session, filter, update);
                var s = await updater.update();
                listActiveMembership.Clear();
                GetActiveData();
                OnPropertyChanged(nameof(listActiveMembership));
                //Set Null
                selectedMembership = null;
                membershipname = "";
                priority = 0;
                OnPropertyChanged(nameof(membershipname));
                OnPropertyChanged(nameof(priority));
            }
            else if (CheckExist() == false)
            {
                MembershipInformation info = new MembershipInformation("", membershipname, priority);
                RegisterMembership regist = new RegisterMembership(info, _connection.client, _session);
                string s = await regist.register();
                listActiveMembership.Add(new MembershipControlViewModel(info, this));
                listAllMembership.Add(new MembershipControlViewModel(info, this));
                OnPropertyChanged(nameof(listActiveMembership));
                Console.WriteLine(s);
            }
            else
            {
                SetActive(selectedMembership);
                Console.WriteLine("MembershipName.isActivated has been set to True!");
            }
        }
        public void UpdateMembershipList(MembershipInformation mem)
        {
            int i = 0;
            if (listActiveMembership.Count > 0)
            {
                foreach (MembershipControlViewModel ls in listActiveMembership)
                {
                    if (ls.membership.Equals(mem))
                    {
                        SetUnactive(ls);
                        listAllMembership.Clear();
                        GetAllData();
                        break;
                    }
                    i++;
                }
                listActiveMembership.RemoveAt(i);
                OnPropertyChanged(nameof(listActiveMembership));
            }
            else
            {
                return;
            }
        }
        public void EditMembership(MembershipInformation mem)
        {
            if (listActiveMembership.Count > 0)
            {
                foreach (MembershipControlViewModel ls in listActiveMembership)
                {
                    if (ls.membership.Equals(mem))
                    {
                        selectedMembership = ls;
                        break;
                    }
                }
            }
            else
            {
                return;
            }
            membershipname = selectedMembership.name;
            priority = selectedMembership.prio;
            OnPropertyChanged(nameof(membershipname));
            OnPropertyChanged(nameof(priority));
        }
        public async void SetActive(MembershipControlViewModel membershipinfo)
        {       
            var filter = Builders<MembershipInformation>.Filter.Eq("ID", membershipinfo.ID);
            var update = Builders<MembershipInformation>.Update.Set("isActivated", true);
            UpdateMembershipInformation updater = new UpdateMembershipInformation(_connection.client, _session, filter, update);
            var s = await updater.update();
            listActiveMembership.Add(selectedMembership);
            selectedMembership = null;
            OnPropertyChanged(nameof(listActiveMembership));
            Console.WriteLine(s);    
        }
        public async void SetUnactive(MembershipControlViewModel membershipinfo)
        {
            if (membershipinfo.isActivated == true)
            {
                var filter = Builders<MembershipInformation>.Filter.Eq("ID", membershipinfo.ID);
                var update = Builders<MembershipInformation>.Update.Set("isActivated", false);
                UpdateMembershipInformation updater = new UpdateMembershipInformation(_connection.client, _session, filter, update);
                var s = await updater.update();
                selectedMembership = null;
                Console.WriteLine(s);
            }
            else Console.WriteLine("Cant execute");
        }
        #endregion

        #region DB
        public async void GetActiveData()
        {
            var filter = Builders<MembershipInformation>.Filter.Eq("isActivated",true);
            GetMembership getter = new GetMembership(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (MembershipInformation mem in ls)
            {
                listActiveMembership.Add(new MembershipControlViewModel(mem, this));
            }
            Console.WriteLine("Executed");
            OnPropertyChanged(nameof(listActiveMembership));

        }
        public async void GetAllData()
        {
            var filter = Builders<MembershipInformation>.Filter.Empty;
            GetMembership getter = new GetMembership(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (MembershipInformation mem in ls)
            {
                listAllMembership.Add(new MembershipControlViewModel(mem, this));
            }
            Console.WriteLine("Executed");

        }
        #endregion
    }
}

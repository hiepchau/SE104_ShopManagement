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
using SE104_OnlineShopManagement.Services;
using System.Windows;

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
        public long membershipRule { get; set; }
        public int priority { get; set; }
        private MembershipControlViewModel selectedMembership { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        public ObservableCollection<MembershipControlViewModel> listActiveMembership { get; set; }
        public ObservableCollection<MembershipControlViewModel> listAllMembership { get; set; }
        #endregion
        #region ICommand
        public ICommand SaveCommand { get; set; }
        public ICommand ClearViewCommand { get; set; }
        #endregion
        public MembershipFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._session = session;
            this._connection = connect;
            listActiveMembership = new ObservableCollection<MembershipControlViewModel>();
            listAllMembership = new ObservableCollection<MembershipControlViewModel>();
            //Get Data
            GetActiveData();
            GetAllData();
            //
            SaveCommand = new RelayCommand<Object>(null, SaveMemberShip);
            ClearViewCommand = new RelayCommand<Object>(null, SetNull);
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
            }
            else
            {
                int flag = CheckExist();
                switch (flag)
                {
                    case 0:
                        CustomMessageBox.Show("Hạng thành viên đã tồn tại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    case 1:
                        SetActive(selectedMembership);
                        Console.WriteLine("MembershipName.isActivated has been set to True!");
                        break;
                    case 2:
                        MembershipInformation info = new MembershipInformation("", membershipname, priority);
                        RegisterMembership regist = new RegisterMembership(info, _connection.client, _session);
                        string s = await regist.register();
                        listActiveMembership.Clear();
                        listAllMembership.Clear();
                        GetActiveData();
                        GetAllData();
                        OnPropertyChanged(nameof(listActiveMembership));
                        Console.WriteLine(s);
                        break;
                }
            }
           
            CustomMessageBox.Show("Thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);

            //Set Null
            SetNull();
        }
        public void UpdateMembershipList(MembershipInformation mem)
        {
            int i = 0;
            if (listActiveMembership.Count > 0)
            {
                foreach (MembershipControlViewModel ls in listActiveMembership)
                {
                    if (ls.membership.ID.Equals(mem.ID))
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
            listActiveMembership.Clear();
            GetActiveData();
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
                listActiveMembership.Clear();
                GetActiveData();
                OnPropertyChanged(nameof(listActiveMembership));
                selectedMembership = null;
                Console.WriteLine(s);
            }
            else Console.WriteLine("Cant execute");
        }
        public void SetNull(object o = null)
        {
            selectedMembership = null;
            membershipname = "";
            priority = 0;
            OnPropertyChanged(nameof(membershipname));
            OnPropertyChanged(nameof(priority));
        }
        public int CheckExist()
        {
            foreach (MembershipControlViewModel ls in listActiveMembership)
            {
                if (membershipname == ls.name)
                {
                    return 0;
                }
            }
                    
            foreach (MembershipControlViewModel ls1 in listAllMembership)
            {
                if (membershipname == ls1.name)
                {
                    selectedMembership = ls1;
                    //Set Active
                    return 1;
                }
            }
            return 2;
        }
        #endregion

        #region DB
        public async void GetActiveData()
        {
            var filter = Builders<MembershipInformation>.Filter.Eq("isActivated",true);
            GetMembership getter = new GetMembership(_connection.client, _session, filter);
            var ls = await getter.Get();
            int No = 1;
            foreach (MembershipInformation mem in ls)
            {             
                listActiveMembership.Add(new MembershipControlViewModel(mem, this, No.ToString()));
                No++;
            }
            Console.WriteLine("Executed");
            OnPropertyChanged(nameof(listActiveMembership));

        }
        public async void GetAllData()
        {
            var filter = Builders<MembershipInformation>.Filter.Empty;
            GetMembership getter = new GetMembership(_connection.client, _session, filter);
            var ls = await getter.Get();
            int No = 1;
            foreach (MembershipInformation mem in ls)
            {
                listAllMembership.Add(new MembershipControlViewModel(mem, this, No.ToString()));
                No++;
            }
            Console.WriteLine("Executed");

        }
        #endregion
    }
}

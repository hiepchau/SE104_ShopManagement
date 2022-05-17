using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    public class IncomeControlViewModel: ViewModelBase
    {
        #region Properties
        public BillInformation bill { get; set; }
        public string ID { get; set; }
        public DateTime saleDay { get; set; }
        public string User { get; set; }
        public long total { get; set; }
        public string displayID { get; set; }
        private IIncomeParent _parent;
        #endregion
        public IncomeControlViewModel(BillInformation billinfo, IIncomeParent parent)
        {
            this.bill = billinfo;
            ID=billinfo.ID;
            saleDay = billinfo.saleDay;
            total = billinfo.total;
            displayID = billinfo.displayID;
            _parent = parent;
            GetEmployeeName();
        }
        #region Function
        public async void GetEmployeeName()
        {
            var filter = Builders<UserInfomation>.Filter.Eq(x => x.ID, bill.User);
            GetUsers getter = new GetUsers((_parent as BaseFunction).Connect.client, (_parent as BaseFunction).Session, filter);
            var ls = await getter.get();
            if (ls != null && ls.Count > 0)
            {
                User = ls.First().LastName;
                OnPropertyChanged(nameof(User));
            }
            else
            {
                return;
            }
        }
        #endregion
    }
}

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
    public class SpendingControlViewModel: ViewModelBase
    {
        #region Properties
        public StockInformation stock { get; set; }
        public string ID { get; set; }
        public DateTime spendDay { get; set; }
        public string User { get; set; }
        public long total { get; set; }
        public string displayID { get; set; }
        private ISpendingParent _parent { get; set; }

        #endregion
        public SpendingControlViewModel(StockInformation stockinfo, ISpendingParent parent)
        {
            this.stock = stockinfo;
            _parent = parent;
            this.ID= stockinfo.ID;
            this.spendDay = stockinfo.StockDay;
            this.displayID = stockinfo.displayID;
            this.total= stockinfo.total;
            GetEmployeeName();
        }
        #region Function
        public async void GetEmployeeName()
        {
            var filter = Builders<UserInfomation>.Filter.Eq(x => x.ID, stock.User);
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

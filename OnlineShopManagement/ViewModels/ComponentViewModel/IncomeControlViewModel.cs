using MaterialDesignThemes.Wpf;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Components.Controls;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    public class IncomeControlViewModel: ViewModelBase
    {
        #region Properties
        public BillInformation bill { get; set; }
        public string ID { get; set; }
        public DateTime saleDay { get; set; }
        public string User { get; set; }
        public string total { get; set; }
        public string displayID { get; set; }
        public ICommand ViewDetailCommand { get; set; }
        private IIncomeParent _parent;
        #endregion

        public IncomeControlViewModel(BillInformation billinfo, IIncomeParent parent)
        {
            this.bill = billinfo;
            ID=billinfo.ID;
            saleDay = billinfo.saleDay;
            total = SeparateThousands(billinfo.total.ToString());
            displayID = billinfo.displayID;
            _parent = parent;
            GetEmployeeName();
            ViewDetailCommand = new RelayCommand<object>(null, viewdetail);
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
        public string SeparateThousands(String text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                ulong valueBefore = ulong.Parse(text, System.Globalization.NumberStyles.AllowThousands);
                string res = String.Format(culture, "{0:N0}", valueBefore);
                return res;
            }
            return "";
        }
        private void viewdetail(object o)
        {
            ViewDetailDialog viewDetail = new ViewDetailDialog();
            BillTemplateViewModel billTemplate = new BillTemplateViewModel(bill, (_parent as BaseFunction).Connect, (_parent as BaseFunction).Session);
            viewDetail.DataContext = billTemplate;
            DialogHost.Show(viewDetail);
        }
        #endregion
    }
}

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
    public class SpendingControlViewModel: ViewModelBase
    {
        #region Properties
        public StockInformation stock { get; set; }
        public string ID { get; set; }
        public DateTime spendDay { get; set; }
        public string User { get; set; }
        public string total { get; set; }
        public string displayID { get; set; }
        public ICommand ViewDetailCommand { get; set; }
        private ISpendingParent _parent { get; set; }

        #endregion

        public SpendingControlViewModel(StockInformation stockinfo, ISpendingParent parent)
        {
            this.stock = stockinfo;
            _parent = parent;
            this.ID= stockinfo.ID;
            this.spendDay = stockinfo.StockDay;
            this.displayID = stockinfo.displayID;
            this.total = SeparateThousands(stockinfo.total.ToString());
            ViewDetailCommand = new RelayCommand<object>(null, viewdetail);
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

        public void viewdetail(object o = null)
        {
            ViewDetailDialog viewDetail = new ViewDetailDialog();
            StockTemplateViewmodel stocktemplate = new StockTemplateViewmodel(stock, (_parent as BaseFunction).Connect, (_parent as BaseFunction).Session);
            viewDetail.DataContext = stocktemplate;
            DialogHost.Show(viewDetail);

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
        #endregion
    }
}

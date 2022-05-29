using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    public interface IBillTemplateParent
    {

    }
    class BillTemplateViewModel : ViewModelBase, IBillTemplateParent
    {
        #region Properties
        public string saleDay { get; set; }
        public string User { get; set; }
        public string customer { get; set; }
        public string total { get; set; }
        public string BillID { get; set; }
        public ObservableCollection<ImportPOSProductControlViewModel> listBillInfo { get; set; }

        private AppSession _session;
        #endregion

        #region ICommand
        #endregion

        public BillTemplateViewModel(BillInformation billInformation, AppSession session , ObservableCollection<ImportPOSProductControlViewModel> listDetail) 
        {
            this._session = session;
            saleDay = "Day";
            BillID = "akdjsnfkajsdnf";
            User = billInformation.User;
            customer = billInformation.customer;
            this.listBillInfo = listDetail;
        }
    }
}

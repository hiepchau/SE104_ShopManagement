using MaterialDesignThemes.Wpf;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Commands;
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
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    class BillTemplateViewModel : ViewModelBase
    {
        #region Properties
        public BillInformation bill { get; set; }
        public string saleDay { get; set; }
        public string User { get; set; }
        public string customer { get; set; }
        public long total { get; set; }
        public string billID { get; set; }
        public string displayID { get; set; }
        public string state { get; set; }
        public string sateID { get; set; }
        public ObservableCollection<BillTemplateControlViewModel> listDetail { get; set; }
        private AppSession _session;
        private MongoConnect _connection;
        #endregion

        #region ICommand
        public ICommand ExitCommand { get; set; }
        #endregion

        public BillTemplateViewModel(BillInformation billInformation, MongoConnect connect, AppSession session) 
        {
            _connection = connect;
            _session = session;
            this.bill = billInformation;
            listDetail = new ObservableCollection<BillTemplateControlViewModel>();
            saleDay = billInformation.saleDay.ToString("dd/MM/yyyy HH:mm:ss");
            billID =  billInformation.ID;
            displayID = billInformation.displayID;
            User = _session.CurrnetUser.LastName;
            customer = billInformation.customer;
            total = billInformation.total;
            state = "Thông tin khách hàng";
            sateID = "Mã hóa đơn";

            ExitCommand = new RelayCommand<Object>(null, exit =>
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
            });
            getdata();
            GetUser();
        }

        #region Function
        #endregion

        #region DB
        private async void GetUser()
        {
            FilterDefinition<UserInfomation> filter = Builders<UserInfomation>.Filter.Eq(x => x.ID, bill.User);
            var tmp = new GetUsers(_connection.client, _session, filter);
            var ls = await tmp.get();

            foreach (UserInfomation user in ls)
            {
                User = user.FirstName + " " + user.LastName;
            }
            OnPropertyChanged(nameof(User));
        }
        private async void getdata()
        {
            FilterDefinition<BillDetails> filter = Builders<BillDetails>.Filter.Eq(x => x.billID, billID);
            var tmp = new GetBillDetails(_connection.client, _session, filter);
            var ls = await tmp.Get();

            int i = 1;
            foreach (BillDetails bill in ls)
            {
                listDetail.Add(new BillTemplateControlViewModel(bill, _connection, _session, i.ToString()));
                i++;
            }
            OnPropertyChanged(nameof(listDetail));
        }
        #endregion
    }
}

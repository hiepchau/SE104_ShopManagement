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
        public DateTime saleDay { get; set; }
        public string User { get; set; }
        public string customer { get; set; }
        public string total { get; set; }
        public ObservableCollection<BillTemplateControlViewModel> listBillInfo { get; set; }

        private MongoConnect _connection;
        private AppSession _session;
        #endregion

        #region ICommand
        #endregion

        public BillTemplateViewModel(BillInformation billInformation, AppSession session, MongoConnect connect) 
        {
            this._connection = connect;
            this._session = session;
            GetProductData(billInformation);
            saleDay = billInformation.saleDay;
            User = billInformation.User;
            customer = billInformation.customer;
            GetEmployeeName(billInformation);
            GetCustomerName(billInformation);
            total = SeparateThousands(billInformation.total.ToString());
        }

        #region Function

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

        #region DB
        public async Task GetBillInfo()
        {
            var filter = Builders<BillInformation>.Filter.Empty;
            GetBills getter = new GetBills(_connection.client, _session, filter);
            Task<List<BillInformation>> task = getter.Get();
            var ls = await task;
            Task.WaitAll(task);
            long sum = 0;
            foreach (BillInformation bill in ls)
            {
                sum += bill.total;
            }

            Console.Write("Executed");
        }
        public async void GetEmployeeName(BillInformation billInformation)
        {
            var filter = Builders<UserInfomation>.Filter.Eq(x => x.ID, billInformation.User);
            GetUsers getter = new GetUsers(_connection.client, _session, filter);
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
        public async void GetCustomerName(BillInformation billInformation)
        {
            var filter = Builders<CustomerInformation>.Filter.Eq(x => x.PhoneNumber, billInformation.customer);
            GetCustomer getter = new GetCustomer(_connection.client, _session, filter);
            var task = getter.Get();
            List<CustomerInformation> ls = new List<CustomerInformation>();
            ls = await task;
            if (ls.Count > 0)
            {
                customer = ls.FirstOrDefault().Name;
                OnPropertyChanged(nameof(customer));
            }
            else
            {
                return;
            }
        }
        public async void GetProductData(BillInformation billInformation)
        {
            var filter = Builders<BillDetails>.Filter.Eq(x => x.billID, billInformation.ID);
            GetBillDetails getter = new GetBillDetails(_connection.client, _session, filter);
            var ls = await getter.Get();
            int No = 1;
            foreach (BillDetails billDetails in ls)
            {
                listBillInfo.Add(new BillTemplateControlViewModel(billDetails, this, No.ToString()));
                No++;
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(listBillInfo));
        }

        #endregion

    }
}

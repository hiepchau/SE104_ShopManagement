using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    class FinanceOverViewFunction : BaseFunction
    {
        #region Properties
        private MongoConnect _connection { get; set; }
        private AppSession _session { get; set; }
        public string Income { get; set; }
        public string Spending { get; set; }
        public string Profit { get; set; }
        public bool isLoaded { get; set; }
        #endregion
        public FinanceOverViewFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            _connection = connect;
            _session = session;
            _ = GetBillData();
            _ = GetStockData();
        }

        #region Function
        public string SeparateThousands(String text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                long valueBefore;
                long.TryParse(text, out valueBefore);
                string res = String.Format(culture, "{0:N0}", valueBefore);
            }
            return "";
        }
        public long ConvertToNumber(string str)
        {
            string[] s = str.Split(',');
            string tmp = "";
            foreach (string a in s)
            {
                tmp += a;
            }

            return long.Parse(tmp);
        }
        public void GetProfit()
        {
            Profit = SeparateThousands((ConvertToNumber(Income) - ConvertToNumber(Spending)).ToString());
            OnPropertyChanged(nameof(Profit));
        }
        #endregion

        #region DB
        public async Task GetStockData()
        {
            var filter = Builders<StockInformation>.Filter.Empty;
            GetStocking getter = new GetStocking(_connection.client, _session, filter);
            Task<List<StockInformation>> task = getter.Get();
            var ls = await task;
            Task.WaitAll(task);
            long sum = 0;
            foreach (StockInformation stock in ls)
            {
                sum += stock.total;
            }
            isLoaded = false;
            Spending = SeparateThousands(sum.ToString());
            OnPropertyChanged(nameof(isLoaded));
            OnPropertyChanged(nameof(Spending));
            GetProfit();
        }
        public async Task GetBillData()
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
            isLoaded = false;
            Income = SeparateThousands(sum.ToString());
            OnPropertyChanged(nameof(isLoaded));
            OnPropertyChanged(nameof(Income));
        }

        #endregion
    }
}

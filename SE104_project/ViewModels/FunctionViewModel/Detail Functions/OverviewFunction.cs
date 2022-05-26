using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    public class OverviewFunction : BaseFunction
    {
        #region Properties
        private MongoConnect _connection { get; set; }
        private AppSession _session { get; set; }
        public string Income { get; set; }
        public string Spending { get; set; }
        public string Profit { get; set; }
        public bool isLoaded { get; set; }
        #endregion
        public OverviewFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            _connection = connect;
            _session = session;
            _ = GetProfit();
        }
        #region Function
        public async Task GetProfit()
        {
            await GetStockData();
            await GetBillData();
            Profit = SeparateThousands((ConvertToNumber(Income)-ConvertToNumber(Spending)).ToString());
            OnPropertyChanged(nameof(Profit));
        }
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
        #endregion
    }
}

using MongoDB.Driver;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
        private object _selectedIndex;
        public object selectedIndex 
        { 
            get 
            { 
                return this._selectedIndex; 
            } 
            set 
            { 
                this._selectedIndex=value; 
                GetData(); 
                OnPropertyChanged(nameof(selectedIndex));
            } 
        }
        private List<StockInformation> liststock { get; set; }
        private List<BillInformation> listbill { get; set; }
        #endregion
        #region ICommand
        #endregion
        public OverviewFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            _connection = connect;
            _session = session;
            listbill = new List<BillInformation>();
            liststock = new List<StockInformation>();
            selectedIndex = 0;
            GetData();
        }
        #region Function
        public async void GetData(object o = null)
        {
            await GetStockData();
            await GetBillData();
            DateTime today = DateTime.Today;
            long stocksum = 0, billsum = 0;
            switch (selectedIndex)
            {
                case 0:
                    foreach (StockInformation stock in liststock)
                    {
                        string stockday = stock.StockDay.ToShortDateString();
                        if ((today-stock.StockDay).TotalDays<=0)
                        {
                            stocksum += stock.total;
                        }
                    }
                    foreach (BillInformation bill in listbill)
                    {
                        string billday = bill.saleDay.ToShortDateString();                   
                        if ((today-bill.saleDay).TotalDays<=0)
                        {
                            billsum += bill.total;
                        }
                    }
                    Income =SeparateThousands(billsum.ToString());
                    Spending=SeparateThousands(stocksum.ToString());
                    GetProfit();
                    OnPropertyChanged(nameof(Income));
                    OnPropertyChanged(nameof(Spending));
                    listbill.Clear();
                    liststock.Clear();
                    return;
                case 1:
                    foreach (StockInformation stock in liststock)
                    {
                        string stockday = stock.StockDay.ToShortDateString();
                        if ((today - stock.StockDay).TotalDays <= 7)
                        {
                            stocksum += stock.total;
                        }
                    }
                    foreach (BillInformation bill in listbill)
                    {
                        string billday = bill.saleDay.ToShortDateString();
                        if ((today - bill.saleDay).TotalDays <= 7)
                        {
                            billsum += bill.total;
                        }
                    }
                    Income = SeparateThousands(billsum.ToString());
                    Spending = SeparateThousands(stocksum.ToString());
                    GetProfit();
                    OnPropertyChanged(nameof(Income));
                    OnPropertyChanged(nameof(Spending));
                    listbill.Clear();
                    liststock.Clear();
                    return;
                case 2:
                    foreach (StockInformation stock in liststock)
                    {
                        string stockday = stock.StockDay.ToShortDateString();
                        if ((today - stock.StockDay).TotalDays <= 30)
                        {
                            stocksum += stock.total;
                        }
                    }
                    foreach (BillInformation bill in listbill)
                    {
                        string billday = bill.saleDay.ToShortDateString();
                        if ((today - bill.saleDay).TotalDays <= 30)
                        {
                            billsum += bill.total;
                        }
                    }
                    Income = SeparateThousands(billsum.ToString());
                    Spending = SeparateThousands(stocksum.ToString());
                    GetProfit();
                    OnPropertyChanged(nameof(Income));
                    OnPropertyChanged(nameof(Spending));
                    listbill.Clear();
                    liststock.Clear();
                    return;
            }
        }
        public void GetProfit()
        {
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
            foreach (StockInformation stock in ls)
            {
                liststock.Add(stock);
            }
        }
        public async Task GetBillData()
        {
            var filter = Builders<BillInformation>.Filter.Empty;
            GetBills getter = new GetBills(_connection.client, _session, filter);
            Task<List<BillInformation>> task = getter.Get();
            var ls = await task;
            Task.WaitAll(task);
            foreach (BillInformation bill in ls)
            {
                listbill.Add(bill);
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
        public void SetNull()
        {
            Income = "0";
            Profit = "0";
            Spending = "0"; 
        }
        #endregion
    }
}

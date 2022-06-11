using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
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
        public ChartValues<long> GraphValues { get; set; }
        public string[] Label { get; set; }
        #endregion
        #region ICommand
        public ICommand Reload { get; set; }
        #endregion
        public OverviewFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            _connection = connect;
            _session = session;
            listbill = new List<BillInformation>();
            liststock = new List<StockInformation>();
            selectedIndex = 0;
            GraphValues = new ChartValues<long>();
            Reload = new RelayCommand<object>(null,GetData);      
        }
        #region Function
        public async void GetData(object o = null)
        {
            if (GraphValues != null)
            {
                GraphValues.Clear();
            }
            await GetStockData();
            await GetBillData();
            DateTime today = DateTime.Today;
            long stocksum = 0, billsum = 0;
            switch (selectedIndex)
            {
                case 0:
                    foreach (StockInformation stock in liststock)
                    {
                        if ((today-stock.StockDay).TotalDays<=0)
                        {
                            stocksum += stock.total;
                        }
                    }
                    foreach (BillInformation bill in listbill)
                    {                  
                        if ((today-bill.saleDay).TotalDays<=0)
                        {
                            billsum += bill.total;
                        }
                    }
                    Income =SeparateThousands(billsum.ToString());
                    Spending=SeparateThousands(stocksum.ToString());
                    GetProfit();
                    GetChartValue(7);
                    OnPropertyChanged(nameof(Income));
                    OnPropertyChanged(nameof(Spending));
                    listbill.Clear();
                    liststock.Clear();
                    return;
                case 1:                   
                    foreach (StockInformation stock in liststock)
                    {
                        if ((today - stock.StockDay).TotalDays <= 7)
                        {
                            stocksum += stock.total;
                        }
                    }
                    foreach (BillInformation bill in listbill)
                    {
                        if ((today - bill.saleDay).TotalDays <= 7)
                        {
                            billsum += bill.total;
                        }
                    }
                    Income = SeparateThousands(billsum.ToString());
                    Spending = SeparateThousands(stocksum.ToString());
                    GetProfit();
                    GetChartValue(7);
                    OnPropertyChanged(nameof(Income));
                    OnPropertyChanged(nameof(Spending));
                    listbill.Clear();
                    liststock.Clear();
                    return;
                case 2:
                    foreach (StockInformation stock in liststock)
                    {
                        if ((today - stock.StockDay).TotalDays <= 30)
                        {
                            stocksum += stock.total;
                        }
                    }
                    foreach (BillInformation bill in listbill)
                    {
                        if ((today - bill.saleDay).TotalDays <= 30)
                        {
                            billsum += bill.total;
                        }
                    }
                    Income = SeparateThousands(billsum.ToString());
                    Spending = SeparateThousands(stocksum.ToString());
                    GetProfit();
                    GetChartValue(30);
                    OnPropertyChanged(nameof(Income));
                    OnPropertyChanged(nameof(Spending));
                    listbill.Clear();
                    liststock.Clear();
                    return;
            }
        }
        public void GetChartValue(int days)
        {
            Label = new string[days];
            long[] stocksum = new long[days];
            long[] billsum = new long[days];
            DateTime today = DateTime.Today;
            for (int i = 0; i <= days-1; i++)
            {
                foreach (StockInformation stock in liststock)
                {
                    if ((today - stock.StockDay).TotalDays > i - 1 && (today - stock.StockDay).TotalDays <= i)
                    {
                        stocksum[i] += stock.total;
                    }
                }
                foreach (BillInformation bill in listbill)
                {
                    if ((today - bill.saleDay).TotalDays > i - 1 && (today - bill.saleDay).TotalDays <= i)
                    {
                        billsum[i] += bill.total;
                    }
                }
            }
            long profit;
            int count = 0;
            for (int i = days-1; i >= 0; i--)
            {
                Label[count] = (today.AddDays(-i)).Day + "/\n" + (today.AddDays(-i)).Month;
                Console.WriteLine(Label[count]);
                profit = billsum[i] - stocksum[i];
                GraphValues.Add(profit);
                count++;
            }          
            OnPropertyChanged(nameof(Label));
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

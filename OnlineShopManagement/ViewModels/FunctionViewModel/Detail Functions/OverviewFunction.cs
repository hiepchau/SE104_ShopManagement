using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public string LaiGop { get; set; }
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
        private List<BillDetails> listbilldetails { get; set; }
        private List<TopSaleProductControlViewModel> _listTopSaleProduct { get; set; }
        private List<UserInfomation> listEmployee { get; set; }
        public ObservableCollection<TopSaleProductControlViewModel> listTopSaleProduct { get; set; }
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
            listbilldetails = new List<BillDetails>();
            listEmployee = new List<UserInfomation>();
            _listTopSaleProduct = new List<TopSaleProductControlViewModel>();
            listTopSaleProduct = new ObservableCollection<TopSaleProductControlViewModel>();
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
            await GetProductData();
            await GetBillDetailsData();
            await GetStockData();
            await GetBillData();
            await GetEmployee();
            GetTopSaleProduct();
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
                    GetLaiGop();
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
                    GetLaiGop();
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
                    GetLaiGop();
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
        public void GetLaiGop()
        {
            long Salary = 0;
            foreach(UserInfomation user in listEmployee)
            {
                Salary += user.salary;
            }
            Console.Write(Salary);
            LaiGop = SeparateThousands((ConvertToNumber(Income) - ConvertToNumber(Spending) - Salary).ToString());
            OnPropertyChanged(nameof(LaiGop));
        }
        public string SeparateThousands(String text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                long valueBefore;
                long.TryParse(text, out valueBefore);
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
        public void GetTopSaleProduct()
        {   
            foreach (TopSaleProductControlViewModel topproduct in _listTopSaleProduct)
            {
                foreach(BillDetails billdetails in listbilldetails)
                {
                    if (billdetails.productID.Equals(topproduct.ID))
                    {
                        topproduct.amount += billdetails.amount;               
                    }
                }
            }
            _listTopSaleProduct.Sort((y, x) => x.amount.CompareTo(y.amount));
            int count = 0;
            foreach(TopSaleProductControlViewModel topproduct in _listTopSaleProduct)
            {
                if (count == 4) break;
                listTopSaleProduct.Add(topproduct);
                count++;
            }
        }
        #endregion
        #region DB
        public async Task GetProductData()
        {
            var filter = Builders<ProductsInformation>.Filter.Eq("isActivated",true);
            GetProducts getter = new GetProducts(_connection.client, _session, filter);
            Task<List<ProductsInformation>> task = getter.Get();
            var ls = await task;
            Task.WaitAll(task);
            foreach (ProductsInformation pro in ls)
            {
                _listTopSaleProduct.Add(new TopSaleProductControlViewModel(pro,this));
            }
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
        public async Task GetEmployee()
        {
            var filter = Builders<UserInfomation>.Filter.Empty;
            GetUsers getter = new GetUsers(_connection.client, _session, filter);
            Task<List<UserInfomation>> task = getter.get();
            var ls = await task;
            Task.WaitAll(task);
            foreach (UserInfomation user in ls)
            {
                listEmployee.Add(user);
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
        public async Task GetBillDetailsData()
        {
            var filter = Builders<BillDetails>.Filter.Empty;
            GetBillDetails getter = new GetBillDetails(_connection.client, _session, filter);
            Task<List<BillDetails>> task = getter.Get();
            var ls = await task;
            Task.WaitAll(task);
            foreach (BillDetails bill in ls)
            {
                listbilldetails.Add(bill);
            }
        }
        #endregion
    }
}

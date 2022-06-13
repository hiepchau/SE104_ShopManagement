using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    public class StockTemplateControlViewModel : ViewModelBase
    {
        #region Properties
        public StockDetails billDetails { get; set; }
        public string NumberOrder { get; set; }
        public string billID { get; set; }
        public string product { get; set; }
        public long price { get; set; }
        public string unit { get; set; }
        public int amount { get; set; }
        public long sumPrice { get; set; }
        public string Category { get; set; }
        public string CategoryID { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        #endregion

        #region ICommand

        #endregion

        public StockTemplateControlViewModel(StockDetails billDetails, MongoConnect connection, AppSession session, string No)
        {
            this.billDetails = billDetails;
            NumberOrder = No;
            billID = billDetails.stockID;
            amount = billDetails.amount;

            sumPrice = billDetails.sumPrice;
            _connection = connection;
            _session = session;
            GetProductInfo();
            GetTypeName();

        }
        #region Function
        public async void GetProductInfo()
        {
            var filter = Builders<ProductsInformation>.Filter.Eq(x => x.ID, billDetails.productID);
            GetProducts getter = new GetProducts(_connection.client, _session, filter);
            var ls = await getter.Get();
            if (ls != null && ls.Count > 0)
            {
                product = ls.First().name;
                CategoryID = ls.First().Category;
                price = sumPrice / amount;
                unit = ls.First().Unit;
                OnPropertyChanged(nameof(price));
                OnPropertyChanged(nameof(unit));
                OnPropertyChanged(nameof(product));
            }
            else
            {
                return;
            }
        }

        public async void GetTypeName()
        {
            var filter = Builders<ProductTypeInfomation>.Filter.Eq(x => x.ID, CategoryID);
            GetProductType getter = new GetProductType(_connection.client, _session, filter);
            var ls = await getter.Get();
            if (ls != null && ls.Count > 0)
            {
                Category = ls.First().name;
                OnPropertyChanged(nameof(Category));
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
    }
    #endregion
}

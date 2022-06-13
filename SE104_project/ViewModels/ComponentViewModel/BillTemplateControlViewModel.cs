using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel

{
    public class BillTemplateControlViewModel : ViewModelBase
    {
        #region Properties
        public BillDetails billDetails { get; set; }
        public string NumberOrder { get;  set; }
        public string billID { get; set; }
        public string product { get; set; }
        public string price { get; set; }
        public int amount { get; set; }
        public long sumPrice { get; set; }
        public string Category { get; set; }
        public string CategoryID { get; set; }
        private IBillTemplateParent _parent;
        #endregion

        #region ICommand

        #endregion

        public BillTemplateControlViewModel(BillDetails billDetails, IBillTemplateParent parent, string No)
        {
            this.billDetails = billDetails;
            NumberOrder = No;
            billID = billDetails.billID;
            amount = billDetails.amount;
            sumPrice = billDetails.sumPrice;
            _parent = parent;
            GetProductName();
            GetProductCateGory();
            GetTypeName();
            GetProductPrice();
        }
        #region Function
        public async void GetProductName()
        {
            var filter = Builders<ProductsInformation>.Filter.Eq(x => x.ID, billDetails.productID);
            GetProducts getter = new GetProducts((_parent as BaseFunction).Connect.client, (_parent as BaseFunction).Session, filter);
            var ls = await getter.Get();
            if (ls != null && ls.Count > 0)
            {
                product = ls.First().name;
                OnPropertyChanged(nameof(product));
            }
            else
            {
                return;
            }
        }
        private async void GetProductCateGory()
        {
            var filter = Builders<ProductsInformation>.Filter.Eq(x => x.ID, billDetails.productID);
            GetProducts getter = new GetProducts((_parent as BaseFunction).Connect.client, (_parent as BaseFunction).Session, filter);
            var ls = await getter.Get();
            if (ls != null && ls.Count > 0)
            {
                CategoryID = ls.First().Category;
                OnPropertyChanged(nameof(CategoryID));
            }
            else
            {
                return;
            }
        }
        public async void GetTypeName()
        {
            var filter = Builders<ProductTypeInfomation>.Filter.Eq(x => x.ID, CategoryID);
            GetProductType getter = new GetProductType((_parent as BaseFunction).Connect.client, (_parent as BaseFunction).Session, filter);
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

        public async void GetProductPrice()
        {
            var filter = Builders<ProductsInformation>.Filter.Eq(x => x.ID, billDetails.productID);
            GetProducts getter = new GetProducts((_parent as BaseFunction).Connect.client, (_parent as BaseFunction).Session, filter);
            var ls = await getter.Get();
            if (ls != null && ls.Count > 0)
            {
                price = SeparateThousands(ls.First().price.ToString());
                OnPropertyChanged(nameof(price));
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
        #endregion
    }
}

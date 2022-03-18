using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class StockDetails
    {
        public string productID { get; set; }
        public string stockID { get; set; }
        public int amount { get; set; }
        public long sumPrice { get; set; }
        public StockDetails(string product, string stock, int am, long sum)
        {
            this.productID = product;
            this.stockID = stock;
            this.amount = am;
            this.sumPrice = sum;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.Entity
{
    public class SoldProductInfo
    {
        public ProductInfo product { get;private set; }
        public int Amount;
        public SoldProductInfo(ProductInfo info, int am)
        {
            this.product = info;
            this.Amount = am;
        }
    }
}

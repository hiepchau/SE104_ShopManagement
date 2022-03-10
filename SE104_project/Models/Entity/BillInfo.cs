using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.Entity
{
    public class BillInfo
    {
        CustomerInfo customerInfo;
        UserInfo userInfo;
        DateTime SellDate;
        List<SoldProductInfo> productInfos;
        public BillInfo(CustomerInfo cus, UserInfo user, List<SoldProductInfo> ls, DateTime sellDate)
        {
            this.customerInfo = cus;
            this.userInfo = user;
            this.productInfos = new List<SoldProductInfo>();
            this.productInfos.AddRange(ls);
            this.SellDate=sellDate;
        }
    }
}

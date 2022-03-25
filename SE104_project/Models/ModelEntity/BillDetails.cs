using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class BillDetails
    {
        public string BillDetailID { get; set; }
        public string productID { get; set; }
        public string billID { get; set; }
        public int amount { get; set; } 
        public long sumPrice { get; set; }   
        public BillDetails(string BillDe,string product, string bill, int am,long sum)
        {
            this.BillDetailID = BillDe;
            this.productID = product;
            this.billID = bill;
            this.amount = am;
            this.sumPrice = sum;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class BillInformation
    {
        public string ID { get; set; }
        public DateTime saleDay { get; set; }
        public UserInfomation User { get; set; }    
        public CustomerInformation customer { get; set; }
        public long total { get; set; }
        public BillInformation(string id, DateTime day, UserInfomation user, CustomerInformation customer, long total)
        {
            this.ID = id;
            this.saleDay = day;
            this.User = user;
            this.customer = customer;
            this.total = total;
        }
    }
}

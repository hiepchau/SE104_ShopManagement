using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class StockInformation
    {
        public string ID { get; set; }
        public DateTime StockDay { get; set; }
        public UserInfomation User { get; set; }
        public CustomerInformation customer { get; set; }
        public long total { get; set; }
        public StockInformation(string id, DateTime day, UserInfomation user, CustomerInformation customer, long total)
        {
            this.ID = id;
            this.StockDay = day;
            this.User = user;
            this.customer = customer;
            this.total = total;
        }
    }
}

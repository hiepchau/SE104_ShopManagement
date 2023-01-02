using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class BillInformation:EntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        [BsonElement("SaleDay")]
        public DateTime saleDay { get; set; }
        [BsonElement("User")]
        public string User { get; set; }    
        [BsonElement("Customer")]
        public string customer { get; set; }
        [BsonElement("Total")]
        public long total { get; set; }
        [BsonElement("DisplayID")]
        public string displayID { get; set; }
        public BillInformation(string id, DateTime day, string user, string customer, long total, string display ="")
        {
            this.ID = id;
            this.saleDay = day;
            this.User = user;
            this.customer = customer;
            this.total = total;
            this.displayID = display;
        }
    }
}

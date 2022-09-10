using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class StockInformation:EntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        [BsonElement("StockDay")]
        public DateTime StockDay { get; set; }
        [BsonElement("UserID")]
        public string User { get; set; }
        [BsonElement("ProducerID")]
        public string producer { get; set; }
        [BsonElement("Total")]
        public long total { get; set; }
        [BsonElement("DisplayID")]
        public string displayID { get; set; }
        public StockInformation(string id, DateTime day, string user, string producer, long total, string display = "")
        {
            this.ID = id;
            this.StockDay = day;
            this.User = user;
            this.producer = producer;
            this.total = total;
            this.displayID = display;
        }
    }
}

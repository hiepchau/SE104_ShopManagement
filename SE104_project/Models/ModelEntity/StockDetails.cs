using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class StockDetails
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string StockDetailID { get;set;}
        [BsonElement("ProductID")]
        public string productID { get; set; }
        [BsonElement("BillID")]
        public string stockID { get; set; }
        [BsonElement("Amount")]
        public int amount { get; set; }
        [BsonElement("SumPrice")]
        public long sumPrice { get; set; }
        public StockDetails(string thisid,string product, string stock, int am, long sum)
        {
            this.StockDetailID = thisid;
            this.productID = product;
            this.stockID = stock;
            this.amount = am;
            this.sumPrice = sum;
        }

    }
}

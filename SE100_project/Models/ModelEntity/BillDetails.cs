using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class BillDetails:EntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BillDetailID { get; set; }
        [BsonElement("ProductID")]
        public string productID { get; set; }
        [BsonElement("BillID")]
        public string billID { get; set; }
        [BsonElement("Amount")]
        public int amount { get; set; } 
        [BsonElement("SumPrice")]
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

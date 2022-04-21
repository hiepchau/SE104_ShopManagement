using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class ProductsInformation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string ID { get; private set; }
        [BsonElement("ProductName")]
        public string name { get; set; }  
        [BsonElement("ProductQuantity")]
        public int quantity { get; set; }  
        [BsonElement("ProductPrice")]
        public long price { get; set; }
        [BsonElement("ProductStockCost")]
        public long StockCost { get; set; } 
        [BsonElement("ProductCategory")]
        public string Category { get; set; } 
        [BsonElement("ProductProvider")]
        public string ProducerInformation { get; set; }
        [BsonElement("Unit")]
        public string Unit { get; set; }

        public ProductsInformation(string id,string Name, int quan, long price, long cost, string category, string pcinfo, string unit)
        {
            this.ID = id;
            this.name = Name;
            this.quantity = quan;
            this.price = price;
            this.StockCost = cost;
            this.Category = category;
            this.ProducerInformation = pcinfo;
            this.Unit = unit;
        }
    }
}

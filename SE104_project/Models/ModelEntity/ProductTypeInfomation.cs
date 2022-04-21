using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    class ProductTypeInfomation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string ID { get; set; }
        [BsonElement("ProductTypeName")]
        public string name { get; set; }
        [BsonElement("Note")]
        public string note { get; set; }
        public ProductTypeInfomation(string id, string name,string note="")
        {
            this.ID = id;
            this.name = name;
            this.note = note;
        }
    }
}

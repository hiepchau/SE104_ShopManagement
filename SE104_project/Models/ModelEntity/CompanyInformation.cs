using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class CompanyInformation
    {
        [BsonId]
        public Guid ID { get; set; }
        [BsonElement("CompanyName")]
        public string Name { get; set; }
        public CompanyInformation(Guid id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
    }
}

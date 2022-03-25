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
        public string SessionID { get; set; }
        [BsonElement("CompanyName")]
        public string Name { get; set; }
        public CompanyInformation(string id, string name)
        {
            this.SessionID = id;
            this.Name = name;
        }
    }
}

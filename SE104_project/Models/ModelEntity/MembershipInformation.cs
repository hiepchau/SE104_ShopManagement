using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class MembershipInformation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        [BsonElement("MembershipName")]
        public string name { get; set; }
        [BsonElement("Priority")]
        public int priority { get; set; }
        public MembershipInformation(string id, string name, int prio) {
            this.ID = id;
            this.name = name;
            this.priority = prio;
        }
    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class MembershipInformation:EntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        [BsonElement("MembershipName")]
        public string name { get; set; }
        [BsonElement("Priority")]
        public int priority { get; set; }
        [BsonElement("Condition")]
        public long condition;
        [BsonElement("isActivated")]
        public bool isActivated { get; set; }
        public MembershipInformation(string id, string name, int prio, bool isActivated= true, long condition = 0)
        {
            this.ID = id;
            this.name = name;
            this.priority = prio;
            this.isActivated = isActivated;
            this.condition = condition;
        }
    }
}

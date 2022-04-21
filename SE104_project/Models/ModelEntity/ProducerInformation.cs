using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class ProducerInformation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("id")]
        public string ID { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Email")]
        public string Email { get; set; }   
        [BsonElement("Phone")]
        public string PhoneNumber { get; set; } 
        public ProducerInformation(string id, string name, string email, string phonenumber)
        {
            this.ID = id;
            this.Name = name;
            this.Email = email;
            this.PhoneNumber = phonenumber;
        }
    }
}

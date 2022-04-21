using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class CustomerInformation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string ID { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }    
        [BsonElement("Email")]
        public string Email { get; set; }
        [BsonElement("Phone")]
        public string PhoneNumber { get; set; }
        [BsonElement("Level")]
        public string CustomerLevel { get; set; }   
        [BsonElement("CMND")]
        public string CMND { get; set; }
        public CustomerInformation(string id, string name, string mail, string phonenum, string level, string cmnd)
        {
            this.ID = id;
            this.Name = name;   
            this.Email = mail;
            this.PhoneNumber = phonenum;
            this.CustomerLevel = level;
            this.CMND = cmnd;
        }
    }
}

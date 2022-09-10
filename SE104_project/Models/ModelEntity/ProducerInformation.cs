using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class ProducerInformation:EntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Email")]
        public string Email { get; set; }   
        [BsonElement("Phone")]
        public string PhoneNumber { get; set; }
        [BsonElement("Address")]
        public string Address { get; set; }
        [BsonElement("DisplayID")]
        public string displayID { get; set; }
        [BsonElement("isActivated")]
        public bool isActivated { get; set; }
        public ProducerInformation(string id, string name, string email, string phonenumber,string address, bool active = true, string display = "")
        {
            this.ID = id;
            this.Name = name;
            this.Email = email;
            this.PhoneNumber = phonenumber;
            this.Address = address;
            this.displayID = display;
            this.isActivated = active;
        }
    }
}

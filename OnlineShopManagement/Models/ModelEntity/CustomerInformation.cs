using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class CustomerInformation:EntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }    
        [BsonElement("Phone")]
        public string PhoneNumber { get; set; }
        [BsonElement("Level")]
        public string CustomerLevel { get; set; }   
        [BsonElement("CMND")]
        public string CMND { get; set; }
        [BsonElement("isActivated")]
        public bool isActivated { get; set; }
        [BsonElement("DisplayID")]
        public string displayID { get; set; }
        [BsonElement("Address")]
        public string Address { get; set; }
        public CustomerInformation(string id, string name, string phonenum, string level, string cmnd,string address,bool isActivated = true ,string display = "")
        {
            this.ID = id;
            this.Name = name;   
            this.PhoneNumber = phonenum;
            this.CustomerLevel = level;
            this.CMND = cmnd;
            this.isActivated = isActivated;
            this.displayID = display;
            this.Address = address;
        }
    }
}

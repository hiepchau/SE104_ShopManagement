using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class StoreInformation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        [BsonElement("Name")]
        public string name { get; set; }
        [BsonElement("Address")]
        public string address { get; set; }
        [BsonElement("PhoneNumber")]
        public string phonenumber { get; set; }
        [BsonElement("Email")]
        public string email { get; set; }
        [BsonElement("Facebook")]
        public string facebook { get; set; }
        [BsonElement("Instagram")]
        public string instagram { get; set; }
        [BsonElement("TaxNumber")]
        public string taxnumber { get; set; }
        [BsonElement("Website")]
        public string website { get; set; }
        public StoreInformation(string id, string name, string address, string phonenumber, string email, string taxnumber, string facebook="",string instagram="",string website="")
        {
            this.ID = id;
            this.name = name;
            this.address = address;
            this.phonenumber = phonenumber;
            this.email = email;
            this.taxnumber = taxnumber;
            this.facebook = facebook;
            this.instagram = instagram;
            this.website = website;
        }
    }
}

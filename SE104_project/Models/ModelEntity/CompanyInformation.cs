using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class CompanyInformation:EntityBase
    {
        [BsonId]
        public string SessionID { get; set; }
        [BsonElement("CompanyName")]
        public string Name { get; set; }
        [BsonElement("CompanyAddress")]
        public string Address { get; set; }
        [BsonElement("CompanyPhone")]
        public string Phone { get; set; }
        [BsonElement("CompanyEmail")]
        public string Email { get; set; }
        [BsonElement("CompanyFacebook")]
        public string Facebook { get; set; }
        [BsonElement("CompanyInstagram")]
        public string Instagram { get; set; }
        //TIN = Tax identification number
        [BsonElement("CompanyTIN")]
        public string TIN { get; set; }
        [BsonElement("CompanyWebsite")]
        public string Website { get; set; }

        public CompanyInformation(string id, string name, string address,string phone, 
            string email, string tin, string website, string facebook = "", string instagram = "")
        {
            this.SessionID = id;
            this.Name = name;
            this.Address = address;
            this.Phone = phone;
            this.Email = email;
            this.Facebook = facebook;
            this.Instagram = instagram;
            this.TIN = tin;
            this.Website = website;
        }
    }
}

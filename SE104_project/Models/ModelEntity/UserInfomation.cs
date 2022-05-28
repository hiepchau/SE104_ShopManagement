using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public enum Role
    {
        Owner=0,
        Manager=1,
        Employee=2,
        Empty=3,
    }

    public enum Gender
    {
        male =0,
        female =1,
        other =2,
        Empty = -1
    }
    public class UserInfomation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }  
        [BsonElement("UserFirstName")]
        public string FirstName { get; set; }
        [BsonElement("UserLastName")]
        public string LastName { get; set; }  
        [BsonElement("UserEmail")]
        public string Email { get; set; }
        [BsonElement("UserPassword")]
        public string Password { get; set; }
        [BsonElement("UserPhoneNumber")]
        public string PhoneNumber { get; set; } 
        [BsonElement("Company")]
        public string companyInformation { get; set; }
        [BsonElement("UserRole")]
        public Role role { get; set; } 
        [BsonElement("UserGender")]
        public Gender gender { get; set; }
        [BsonElement("UserSalary")]
        public long salary { get; set; }
        [BsonElement("UserBirthday")]
        public DateTime birthDay { get; set; }
        [BsonElement("DisplayID")]
        public string displayID { get; set; }
        [BsonElement("isActivated")]
        public bool isActivated { get; set; }
        [BsonElement("WorkDate")]
        public DateTime workDate { get; set; }
        public UserInfomation(string id, string name, string LastName, string email,string pass, string phonenumer, string compa, Role role, Gender gen, long salary, DateTime birth, DateTime work,bool activated=true, string display = "")
        {
            this.ID = id;
            this.FirstName = name;  
            this.LastName = LastName;
            this.Email = email; 
            this.Password = pass;
            this.PhoneNumber = phonenumer;  
            this.companyInformation = compa;
            this.role = role;
            this.gender = gen;
            this.salary = salary;
            this.birthDay = birth;
            this.isActivated = activated;
            this.displayID = display;
            this.workDate = work;
        }
    }
}

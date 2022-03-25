using System;
using System.Collections.Generic;
using System.Text;
using SE104_OnlineShopManagement.Models.ModelEntity;
using MongoDB.Driver;
using MongoDB.Bson;

namespace SE104_OnlineShopManagement.Network.Insert_database
{
    public class RegisterUser
    {
        private UserInfomation newUser;
        private MongoClient mongoClient;
        public RegisterUser(UserInfomation newUser, MongoClient client)
        {
            this.newUser = newUser;
            this.mongoClient = client;
        }
        public void registerUser()
        {
            var database = mongoClient.GetDatabase(newUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("UserInformation");
            BsonDocument newUserDoc = new BsonDocument{
                { "Company",newUser.companyInformation},
                {"UserEmail",newUser.Email },
                {"UserPassword",newUser.Password },
                {"UserFirstName",newUser.FirstName },
                {"UserLastName",newUser.LastName },
                {"UserPhoneNumber",newUser.PhoneNumber },
                {"UserRole",(int)newUser.role },
                {"UserGender",(int)newUser.gender },
                {"UserBirthday",newUser.birthDay }
            };
            collection.InsertOne(newUserDoc);
            Console.WriteLine("User Inserted into", newUser.companyInformation);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using SE104_OnlineShopManagement.Models.ModelEntity;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;

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
        public async Task<string> registerUser()
        {
            var database = mongoClient.GetDatabase(newUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("UserInformation");
            var projectioncheck = Builders<BsonDocument>.Projection.Include("_id");
            var filtercheck = Builders<BsonDocument>.Filter.Eq("_id", newUser.ID);
            var lscheck = await collection.Find(filtercheck).Project(projectioncheck).ToListAsync();
            if (lscheck.Count > 0 || newUser.ID=="")
            {
                BsonDocument newUserDoc1 = new BsonDocument{
                { "Company",newUser.companyInformation},
                {"UserEmail",newUser.Email },
                {"UserPassword",newUser.Password },
                {"UserFirstName",newUser.FirstName },
                {"UserLastName",newUser.LastName },
                {"UserPhoneNumber",newUser.PhoneNumber },
                {"UserRole",(int)newUser.role },
                {"UserGender",(int)newUser.gender },
                {"UserSalary",(long)newUser.salary},
                {"UserBirthday",newUser.birthDay }
                };
                await collection.InsertOneAsync(newUserDoc1);
                Console.WriteLine("User Inserted into", newUser.companyInformation);
                return newUserDoc1["_id"].ToString();
            }
            BsonDocument newUserDoc = new BsonDocument{
                {"_id", newUser.ID},
                { "Company",newUser.companyInformation},
                {"UserEmail",newUser.Email },
                {"UserPassword",newUser.Password },
                {"UserFirstName",newUser.FirstName },
                {"UserLastName",newUser.LastName },
                {"UserPhoneNumber",newUser.PhoneNumber },
                {"UserRole",(int)newUser.role },
                {"UserGender",(int)newUser.gender },
                {"UserSalary",(long)newUser.salary },
                {"UserBirthday",newUser.birthDay }
            };
            await collection.InsertOneAsync(newUserDoc);
            Console.WriteLine("User Inserted into", newUser.companyInformation);
            return newUserDoc["_id"].ToString();
        }
    }
}

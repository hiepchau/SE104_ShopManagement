using System;
using System.Collections.Generic;
using System.Text;
using SE104_OnlineShopManagement.Models.ModelEntity;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using SE104_OnlineShopManagement.Network.Get_database;

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
            var filtercheck = Builders<UserInfomation>.Filter.Eq(x=>x.displayID,newUser.displayID);
            var filtercheckuser = Builders<UserInfomation>.Filter.Eq(x => x.Email, newUser.Email);
            var task1 =  new GetUsers(mongoClient, new AppSession(newUser), filtercheck).get();
            var task2 = new GetUsers(mongoClient, new AppSession(newUser), filtercheckuser).get();
            var lscheck = await task1;
            var lscheckuser = await task2;
            Task.WaitAll(task1,task2);
            if(lscheckuser == null || (lscheckuser != null && lscheckuser.Count > 0))
            {
                Console.WriteLine("User used!");
                return "Email used";
            }
            if (lscheck.Count > 0 || (string.IsNullOrEmpty(newUser.ID) && string.IsNullOrEmpty(newUser.displayID)))
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
                {"UserBirthday",newUser.birthDay },
                {"DisplayID",Guid.NewGuid().ToString()},
                {"isActivated",true},
                {"WorkDate", newUser.workDate },
                };
                await collection.InsertOneAsync(newUserDoc1);
                Console.WriteLine("User Inserted into", newUser.companyInformation);
                return newUserDoc1["_id"].ToString();
            }
            if (string.IsNullOrEmpty(newUser.displayID))
            {
                BsonDocument newUserDoc = new BsonDocument{
                { "Company",newUser.companyInformation},
                {"UserEmail",newUser.Email },
                {"UserPassword",newUser.Password },
                {"UserFirstName",newUser.FirstName },
                {"UserLastName",newUser.LastName },
                {"UserPhoneNumber",newUser.PhoneNumber },
                {"UserRole",(int)newUser.role },
                {"UserGender",(int)newUser.gender },
                {"UserSalary",(long)newUser.salary },
                {"UserBirthday",newUser.birthDay },
                {"DisplayID",newUser.ID},
                {"isActivated",true },
                {"WorkDate", newUser.workDate },
            };
                await collection.InsertOneAsync(newUserDoc);
                Console.WriteLine("User Inserted into", newUser.companyInformation);
                return newUserDoc["_id"].ToString();
            }
            else
            {
                BsonDocument newUserDoc = new BsonDocument{
                { "Company",newUser.companyInformation},
                {"UserEmail",newUser.Email },
                {"UserPassword",newUser.Password },
                {"UserFirstName",newUser.FirstName },
                {"UserLastName",newUser.LastName },
                {"UserPhoneNumber",newUser.PhoneNumber },
                {"UserRole",(int)newUser.role },
                {"UserGender",(int)newUser.gender },
                {"UserSalary",(long)newUser.salary },
                {"UserBirthday",newUser.birthDay },
                {"DisplayID",newUser.displayID},
                {"isActivated",true },
                {"WorkDate", newUser.workDate },
            };
                await collection.InsertOneAsync(newUserDoc);
                Console.WriteLine("User Inserted into", newUser.companyInformation);
                return newUserDoc["_id"].ToString();
            }
        }
    }
}

using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Models.Model
{
    public class UserInformationModel : BaseModel, IModel<UserInfomation>
    {
        public UserInformationModel(MongoClient client, AppSession session) : base(client, session)
        {
        }

        public async Task<List<UserInfomation>> GetEntity(FilterDefinition<UserInfomation> filter)
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<UserInfomation>("UserInformation");
            var field = Builders<UserInfomation>.Projection
                .Include(p => p.ID)
                .Include(p => p.FirstName)
                .Include(p => p.LastName)
                .Include(p => p.Email)
                .Include(p => p.Password)
                .Include(p => p.PhoneNumber)
                .Include(p => p.companyInformation)
                .Include(p => p.role)
                .Include(p => p.birthDay)
                .Include(p => p.salary)
                .Include(p => p.gender)
                .Include(p => p.displayID)
                .Include(p => p.isActivated)
                .Include(p => p.workDate);
            var ls = await collection.Find<UserInfomation>(filter).Project<UserInfomation>(field).ToListAsync();
            return ls;
        }

        public async Task<(bool isSuccessful, string message)> Register(UserInfomation registob)
        {
            var newUser = registob as UserInfomation;
            var database = _client.GetDatabase(newUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("UserInformation");
            var filtercheck = Builders<UserInfomation>.Filter.Eq(x => x.displayID, newUser.displayID);
            var filtercheckuser = Builders<UserInfomation>.Filter.Eq(x => x.Email, newUser.Email);
            var task1 = GetEntity(filtercheck);
            var task2 = GetEntity(filtercheckuser);
            var lscheck = await task1;
            var lscheckuser = await task2;
            Task.WaitAll(task1, task2);
            if (lscheckuser == null || (lscheckuser != null && lscheckuser.Count > 0))
            {
                Console.WriteLine("User used!");
                return (false,"Email used");
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
                try
                {
                    await collection.InsertOneAsync(newUserDoc1);
                    Console.WriteLine("User Inserted into", newUser.companyInformation);
                    return (true,newUserDoc1["_id"].ToString());
                }
                catch (Exception e)
                {
                    return (false, e.Message);
                }
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
                try
                {
                    await collection.InsertOneAsync(newUserDoc);
                    Console.WriteLine("User Inserted into", newUser.companyInformation);
                    return (true, newUserDoc["_id"].ToString());
                }
                catch(Exception e)
                {
                    return (false, e.Message);
                }
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
                try
                {
                    await collection.InsertOneAsync(newUserDoc);
                    Console.WriteLine("User Inserted into", newUser.companyInformation);
                    return (true, newUserDoc["_id"].ToString());
                }
                catch (Exception e)
                {
                    return (false, e.Message);
                }
            }
        }

        public async Task<(bool isSuccessful, string message)> Update(FilterDefinition<UserInfomation> filter, UpdateDefinition<UserInfomation> updatedata)
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<UserInfomation>("UserInformation");
            try
            {
                object o = await collection.UpdateOneAsync(filter, updatedata);
                string s = o.ToString();
                return (true,s);
            }
            catch(Exception e)
            {
                return(false, e.Message);
            }
            
        }
    }
}

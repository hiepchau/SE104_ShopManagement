using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Network.Get_database
{
    public class Authenticator
    {
        private AuthenticationInformation autheninfo;
        private MongoClient client;
        public Authenticator(AuthenticationInformation authen, MongoClient mongoClient)
        {
            this.autheninfo = authen;
            this.client = mongoClient;
        }
        public async Task<UserInfomation> Authenticate()
        {
            var compalist = client.ListDatabaseNames().ToList();
            Func<Boolean> checkDB = new Func<bool>(() =>
            {
                foreach (var comp in compalist)
                {
                    if(String.Equals(comp,autheninfo.companyName))
                        return true;
                }
                return false;
            });
            bool check = checkDB();
            if (check)
            {
                var database = client.GetDatabase(autheninfo.companyName);
                var collection = database.GetCollection<UserInfomation>("UserInformation");
                var filter = Builders<BsonDocument>.Filter.Eq("UserEmail", autheninfo.UserName);
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
                    .Include(p=> p.isActivated)
                    .Include(p => p.gender);
  
                UserInfomation au = await collection.Find<UserInfomation>(x=>x.Email==autheninfo.UserName && x.Password==autheninfo.Password).Project<UserInfomation>(field).FirstOrDefaultAsync();
                return au;
            }
            return null;
        }

    }
}

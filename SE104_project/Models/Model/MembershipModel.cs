using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Models.Model
{
    public class MembershipModel : BaseModel, IModel<MembershipInformation>
    {
        public MembershipModel(MongoClient client, AppSession session) : base(client, session)
        {
        }

        public async Task<List<MembershipInformation>> GetEntity(FilterDefinition<MembershipInformation> filter)
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<MembershipInformation>("MembershipInformation");
            var field = Builders<MembershipInformation>.Projection
                .Include(x => x.ID)
                .Include(X => X.name)
                .Include(x => x.priority)
                .Include(x => x.condition)
                .Include(x => x.isActivated);

            return await collection.Find<MembershipInformation>(filter).Project<MembershipInformation>(field).ToListAsync();
        }

        public async Task<(bool isSuccessful, string message)> Register(MembershipInformation registob)
        {
            var newShip = registob as MembershipInformation;
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("MembershipInformation");
            BsonDocument newProductDoc = new BsonDocument
            {
                {"MembershipName", newShip.name },
                {"Priority", newShip.priority },
                {"isActivated", newShip.isActivated },
                {"Condition",newShip.condition }
            };
            try
            {
                await collection.InsertOneAsync(newProductDoc);
                Console.WriteLine("Membership Inserted into " + _session.CurrnetUser.companyInformation);
                return (true,newProductDoc["_id"].ToString());
            }
            catch(Exception e)
            {
                return (false,e.Message);
            }
            
        }

        public async Task<(bool isSuccessful, string message)> Update(FilterDefinition<MembershipInformation> filter, UpdateDefinition<MembershipInformation> updatedata)
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<MembershipInformation>("MembershipInformation");
            try
            {
                object o = await collection.UpdateOneAsync(filter, updatedata);
                string s = o.ToString();
                return (true,s);
            }
            catch(Exception e)
            {
                return(false,e.Message);
            }
        }
    }
}

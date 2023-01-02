using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Models.Model
{
    public class StoreInformationModel : BaseModel, IModel<StoreInformation>
    {
        public StoreInformationModel(MongoClient client, AppSession session) : base(client, session)
        {
        }

        public async Task<List<StoreInformation>> GetEntity(FilterDefinition<StoreInformation> filter)
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<StoreInformation>("StoreInformation");
            var field = Builders<StoreInformation>.Projection
                .Include(p => p.name)
                .Include(p => p.phonenumber)
                .Include(p => p.address)
                .Include(p => p.email)
                .Include(p => p.facebook)
                .Include(p => p.instagram)
                .Include(p => p.taxnumber)
                .Include(p => p.website);

            var au = await collection.Find<StoreInformation>(filter).Project<StoreInformation>(field).ToListAsync();
            return au;
        }

        public async Task<(bool isSuccessful, string message)> Register(StoreInformation registob)
        {
            var store = registob as StoreInformation;
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("StoreInformation");
            BsonDocument newProductDoc = new BsonDocument
            {
                {"Name",store.name},
                {"Address", store.address},
                {"PhoneNumber", store.phonenumber},
                {"Email", store.email},
                {"Facebook", store.facebook},
                {"Instagram", store.instagram},
                {"TaxNumber", store.taxnumber},
                {"Website", store.website},
            };
            try
            {
                await collection.InsertOneAsync(newProductDoc);
                Console.WriteLine("User Inserted into " + _session.CurrnetUser.companyInformation);
                return (true,newProductDoc["_id"].ToString());
            }
            catch(Exception e)
            {
                return(false,e.Message);
            }
        }

        public async Task<(bool isSuccessful, string message)> Update(FilterDefinition<StoreInformation> filter, UpdateDefinition<StoreInformation> updatedata)
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<StoreInformation>("StoreInformation");
            try
            {
                object o = await collection.UpdateOneAsync(filter, updatedata);
                string s = o.ToString();
                return (true,s);
            }
            catch(Exception e)
            {
                return (false,e.Message);
            }
        }
    }
}

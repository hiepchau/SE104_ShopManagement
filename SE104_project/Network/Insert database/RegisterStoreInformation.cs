using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Network.Insert_database
{
    public class RegisterStoreInformation
    {
        private StoreInformation store;
        private MongoClient mongoClient;
        private AppSession session;
        public RegisterStoreInformation(StoreInformation store, MongoClient client, AppSession ses)
        {
            this.store = store;
            this.mongoClient = client;
            this.session = ses;
        }
        public async Task<string> register()
        {
            var database = mongoClient.GetDatabase(session.CurrnetUser.companyInformation);
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
            await collection.InsertOneAsync(newProductDoc);
            Console.WriteLine("User Inserted into " + session.CurrnetUser.companyInformation);
            return newProductDoc["_id"].ToString();
        }
    }
}

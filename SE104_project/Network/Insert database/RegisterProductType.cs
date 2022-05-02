using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;

namespace SE104_OnlineShopManagement.Network.Insert_database
{
    class RegisterProductType
    {
        private ProductTypeInfomation newShip;
        private MongoClient mongoClient;
        private AppSession session;

        public RegisterProductType(ProductTypeInfomation info, MongoClient client, AppSession session)
        {
            this.newShip = info;
            this.mongoClient = client;
            this.session = session;
        }

        public async Task<String> register()
        {
            var database = mongoClient.GetDatabase(session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("ProductTypeInfomation");
            var projectioncheck = Builders<BsonDocument>.Projection.Include("_id");
            var filtercheck = Builders<BsonDocument>.Filter.Eq("_id", newShip.ID);
            var lscheck = await collection.Find(filtercheck).Project(projectioncheck).ToListAsync();
            BsonDocument newProductDoc = new BsonDocument
            {
                {"ProductTypeName", newShip.name },
                {"Note", newShip.note },
                {"isActivated", newShip.isActivated },
            };
            await collection.InsertOneAsync(newProductDoc);
            Console.WriteLine("ProductType Inserted into " + session.CurrnetUser.companyInformation);
            return newProductDoc["_id"].ToString();
        }
    }
}

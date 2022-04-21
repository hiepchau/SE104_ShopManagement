using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Network.Insert_database
{
    public class RegisterProducer
    {
        private ProducerInformation producer;
        private MongoClient mongoClient;
        private AppSession session;
        public RegisterProducer(ProducerInformation newcus, MongoClient client, AppSession ses)
        {
            this.producer = newcus;
            this.mongoClient = client;
            this.session = ses;
        }
        public async Task<string> register()
        {
            var database = mongoClient.GetDatabase(session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("ProducerInfromation");
            var projectioncheck = Builders<BsonDocument>.Projection.Include("_id");
            var filtercheck = Builders<BsonDocument>.Filter.Eq("_id", producer.ID);
            var lscheck = await collection.Find(filtercheck).Project(projectioncheck).ToListAsync();
            if (lscheck.Count > 0)
            {
                Console.WriteLine("Insert error");
                return null;
            }
            BsonDocument newProductDoc = new BsonDocument{
                {"_id", producer.ID},
                {"Name", producer.Name},
                {"Email", producer.Email },
                {"Phone", producer.PhoneNumber },
            };
            await collection.InsertOneAsync(newProductDoc);
            Console.WriteLine("Object Inserted into " + session.CurrnetUser.companyInformation);
            return newProductDoc["_id"].ToString();
        }
    }
}

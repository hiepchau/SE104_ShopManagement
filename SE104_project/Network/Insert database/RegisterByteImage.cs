using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Network.Insert_database
{
    public class RegisterByteImage
    {
        private ByteImage newimg;
        private MongoClient mongoClient;
        private AppSession session;

        public RegisterByteImage(ByteImage img, MongoClient client, AppSession sess)
        {
            this.newimg = img;
            this.mongoClient = client;
            this.session = sess;
        }

        public async Task register()
        {
            var database = mongoClient.GetDatabase(session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("Images");
            BsonDocument newDoc = new BsonDocument()
            {
                {"obID",newimg.obID},
                {"data", newimg.data},
            };
            await collection.InsertOneAsync(newDoc);
            
        }
    }
}

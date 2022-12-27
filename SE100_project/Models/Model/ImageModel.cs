using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Models.Model
{
    public class ImageModel : BaseModel, IModel<ByteImage>
    {
        public ImageModel(MongoClient client, AppSession session) : base(client, session)
        {
        }

        public async Task<List<ByteImage>> GetEntity(FilterDefinition<ByteImage> filter)
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<ByteImage>("Images");
            var field = Builders<ByteImage>.Projection
                .Include(p => p.obID)
                .Include(p => p.data);

            var ls = await collection.Find<ByteImage>(filter).Project<ByteImage>(field).ToListAsync();
            return ls;
        }

        public async Task<(bool isSuccessful, string message)> Register(ByteImage registob)
        {
            var newimg = registob as ByteImage;
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("Images");
            BsonDocument newDoc = new BsonDocument()
            {
                {"obID",newimg.obID},
                {"data", newimg.data},
            };
            try
            {
                await collection.InsertOneAsync(newDoc);
                return (true, "Added");

            }
            catch(Exception e)
            {
                return (false, e.Message);
            }
            
        }

        public async Task<(bool isSuccessful, string message)> Update(FilterDefinition<ByteImage> filter, UpdateDefinition<ByteImage> updatedata)
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<ByteImage>("Images");
            try
            {
                object o = await collection.UpdateOneAsync(filter, updatedata);
                string s = o.ToString();
                return (true,s);
            }
            catch(Exception e)
            {
                return (false, e.Message);
            }
            
        }
    }
}

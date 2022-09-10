using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Models.Model
{
    public class ProductTypeModel : BaseModel, IModel<ProductTypeInfomation>
    {
        public ProductTypeModel(MongoClient client, AppSession session) : base(client, session)
        {
        }

        public async Task<List<ProductTypeInfomation>> GetEntity(FilterDefinition<ProductTypeInfomation> filter)
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<ProductTypeInfomation>("ProductTypeInformation");
            var field = Builders<ProductTypeInfomation>.Projection
                .Include(x => x.ID)
                .Include(X => X.name)
                .Include(x => x.note)
                .Include(x => x.isActivated);

            return await collection.Find<ProductTypeInfomation>(filter).Project<ProductTypeInfomation>(field).ToListAsync();
        }

        public async Task<(bool isSuccessful, string message)> Register(ProductTypeInfomation registob)
        {
            var newShip = registob as ProductTypeInfomation;
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("ProductTypeInformation");
            var projectioncheck = Builders<BsonDocument>.Projection.Include("_id");
            var filtercheck = Builders<BsonDocument>.Filter.Eq("_id", newShip.ID);
            var lscheck = await collection.Find(filtercheck).Project(projectioncheck).ToListAsync();
            BsonDocument newProductDoc = new BsonDocument
            {
                {"ProductTypeName", newShip.name },
                {"Note", newShip.note },
                {"isActivated", newShip.isActivated },
            };
            try
            {
                await collection.InsertOneAsync(newProductDoc);
                Console.WriteLine("ProductType Inserted into " + _session.CurrnetUser.companyInformation);
                return (true,newProductDoc["_id"].ToString());
            }
            catch(Exception e)
            {
                return(false,e.Message);
            }
        }

        public async Task<(bool isSuccessful, string message)> Update(FilterDefinition<ProductTypeInfomation> filter, UpdateDefinition<ProductTypeInfomation> updatedata)
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<ProductTypeInfomation>("ProductTypeInformation");
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

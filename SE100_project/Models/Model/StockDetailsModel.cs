using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Models.Model
{
    public class StockDetailsModel : BaseModel, IModel<StockDetails>
    {
        public StockDetailsModel(MongoClient client, AppSession session) : base(client, session)
        {
        }

        public async Task<List<StockDetails>> GetEntity(FilterDefinition<StockDetails> filter)
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<StockDetails>("StockDetailInformation");
            var field = Builders<StockDetails>.Projection
                .Include(p => p.StockDetailID)
                .Include(p => p.amount)
                .Include(p => p.stockID)
                .Include(p => p.productID)
                .Include(p => p.sumPrice);

            var au = await collection.Find<StockDetails>(filter).Project<StockDetails>(field).ToListAsync();
            return au;
        }

        public async Task<(bool isSuccessful, string message)> Register(StockDetails registob)
        {
            var detail = registob as StockDetails;
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("StockDetailInformation");
            BsonDocument newProductDoc = new BsonDocument
            {
                {"ProductID",detail.productID},
                {"BillID", detail.stockID},
                {"Amount", detail.amount},
                {"SumPrice", detail.sumPrice},
            };
            try
            {
                await collection.InsertOneAsync(newProductDoc);
                Console.WriteLine("User Inserted into " + _session.CurrnetUser.companyInformation);
                return (true,newProductDoc["_id"].ToString());
            }
            catch(Exception e)
            {
                return (false, e.Message);
            }
        }

        public Task<(bool isSuccessful, string message)> Update(FilterDefinition<StockDetails> filter, UpdateDefinition<StockDetails> updatedata)
        {
            throw new NotImplementedException();
        }
    }
}

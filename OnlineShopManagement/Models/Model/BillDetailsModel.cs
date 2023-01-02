    using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Models.Model
{
    public class BillDetailsModel : BaseModel, IModel<BillDetails>
    {
        public BillDetailsModel(MongoClient client, AppSession session) : base(client, session)
        {
        }

        public async Task<List<BillDetails>> GetEntity(FilterDefinition<BillDetails> filter)
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BillDetails>("BillDetailsInformation");
            var field = Builders<BillDetails>.Projection
                .Include(p => p.BillDetailID)
                .Include(p => p.amount)
                .Include(p => p.billID)
                .Include(p => p.productID)
                .Include(p => p.sumPrice);

            var au = await collection.Find<BillDetails>(filter).Project<BillDetails>(field).ToListAsync();
            return au;
        }

        public async Task<(bool isSuccessful, string message)> Register(BillDetails registob)
        {
            var newBill = registob as BillDetails;
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("BillDetailsInformation");
            BsonDocument newProductDoc = new BsonDocument
            {
                {"ProductID",newBill.productID},
                {"BillID", newBill.billID},
                {"Amount", newBill.amount},
                {"SumPrice", newBill.sumPrice},
            };
            try
            {
                await collection.InsertOneAsync(newProductDoc);
                Console.WriteLine("User Inserted into " + _session.CurrnetUser.companyInformation);
                return (true,newProductDoc["_id"].ToString());
            }
            catch(Exception e)
            {
                return (false,e.Message);
            }
        }

        public Task<(bool isSuccessful, string message)> Update(FilterDefinition<BillDetails> filter, UpdateDefinition<BillDetails> updatedata)
        {
            throw new NotImplementedException();
        }
    }
}

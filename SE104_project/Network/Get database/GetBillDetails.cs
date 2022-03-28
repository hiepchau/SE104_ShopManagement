using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Network.Get_database
{
    public class GetBillDetails
    {
        private MongoClient _client;
        private AppSession _session;
        private FilterDefinition<BillDetails> _filter;
        public GetBillDetails(MongoClient client, AppSession session, FilterDefinition<BillDetails> filter)
        {
            _client = client;
            _session = session;
            _filter = filter;
        }
        public List<BillDetails> Get()
        {

            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BillDetails>("BillDetailsInformation");
            var field = Builders<BillDetails>.Projection
                .Include(p => p.BillDetailID)
                .Include(p => p.amount)
                .Include(p => p.billID)
                .Include(p => p.productID)
                .Include(p => p.sumPrice);

            var au = collection.Find<BillDetails>(_filter).Project<BillDetails>(field).ToList();
            return au;

        }
    }
}

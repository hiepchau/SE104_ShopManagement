using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Network.Get_database
{
    public class GetBills
    {
        private MongoClient _client;
        private AppSession _session;
        private FilterDefinition<BillInformation> _filter;
        public GetBills(MongoClient client, AppSession session, FilterDefinition<BillInformation> filter)
        {
            _client = client;
            _session = session;
            _filter = filter;
        }
        public async Task<List<BillInformation>> Get()
        {

            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BillInformation>("BillsInformation");
            var field = Builders<BillInformation>.Projection
                .Include(p => p.ID)
                .Include(p => p.User)
                .Include(p => p.saleDay)
                .Include(p => p.customer)
                .Include(p => p.total);

            var au = await collection.Find<BillInformation>(_filter).Project<BillInformation>(field).ToListAsync();
            return au;

        }
    }
}

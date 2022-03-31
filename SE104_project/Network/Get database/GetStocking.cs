using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Network.Get_database
{
    public class GetStocking
    {
        private MongoClient _client;
        private AppSession _session;
        private FilterDefinition<StockInformation> _filter;
        public GetStocking(MongoClient client, AppSession session, FilterDefinition<StockInformation> filter)
        {
            _client = client;
            _session = session;
            _filter = filter;
        }
        public List<StockInformation> Get()
        {

            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<StockInformation>("StockingInformation");
            var field = Builders<StockInformation>.Projection
                .Include(p => p.ID)
                .Include(p => p.StockDay)
                .Include(p => p.User)
                .Include(p => p.customer)
                .Include(p => p.total);

            var au = collection.Find<StockInformation>(_filter).Project<StockInformation>(field).ToList();
            return au;

        }
    }
}

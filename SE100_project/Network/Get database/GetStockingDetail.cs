using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Network.Get_database
{
    public class GetStockingDetail
    {
        private MongoClient _client;
        private AppSession _session;
        private FilterDefinition<StockDetails> _filter;
        public GetStockingDetail(MongoClient client, AppSession session, FilterDefinition<StockDetails> filter)
        {
            _client = client;
            _session = session;
            _filter = filter;
        }
        public async Task<List<StockDetails>> Get()
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<StockDetails>("StockDetailInformation");
            var field = Builders<StockDetails>.Projection
                .Include(p => p.StockDetailID)
                .Include(p => p.amount)
                .Include(p => p.stockID)
                .Include(p => p.productID)
                .Include(p => p.sumPrice);

            var au = await collection.Find<StockDetails>(_filter).Project<StockDetails>(field).ToListAsync();
            return au;

        }
    }
}

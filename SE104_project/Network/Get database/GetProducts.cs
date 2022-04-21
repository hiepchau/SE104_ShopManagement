using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;

namespace SE104_OnlineShopManagement.Network.Get_database
{
    public class GetProducts
    {
        private MongoClient _client;
        private AppSession _session;
        private FilterDefinition<ProductsInformation> _filter;
        public GetProducts(MongoClient client, AppSession session, FilterDefinition<ProductsInformation> filter )
        {
            _client = client;
            _session = session;
            _filter = filter;
        }
        public async Task<List<ProductsInformation>> Get()
        {
            
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<ProductsInformation>("ProductsInformation");
            var field = Builders<ProductsInformation>.Projection
                .Include(p => p.ID)
                .Include(p => p.StockCost)
                .Include(p => p.quantity)
                .Include(p => p.price)
                .Include(p => p.ProducerInformation)
                .Include(p => p.Category)
                .Include(p=>p.name)
                .Include(p=>p.Unit);

            var au = await collection.Find<ProductsInformation>(_filter).Project<ProductsInformation>(field).ToListAsync();
            return au;

        }
    }
}

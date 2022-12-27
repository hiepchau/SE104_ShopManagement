using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Network.Get_database
{
    public class GetStoreInformation
    {
        private MongoClient _client;
        private AppSession _session;
        private FilterDefinition<StoreInformation> _filter;
        public GetStoreInformation(MongoClient client, AppSession session, FilterDefinition<StoreInformation> filter)
        {
            _client = client;
            _session = session;
            _filter = filter;
        }
        public async Task<List<StoreInformation>> Get()
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<StoreInformation>("StoreInformation");
            var field = Builders<StoreInformation>.Projection
                .Include(p => p.name)
                .Include(p => p.phonenumber)
                .Include(p => p.address)
                .Include(p => p.email)
                .Include(p => p.facebook)
                .Include(p => p.instagram)
                .Include(p => p.taxnumber)
                .Include(p => p.website);

            var au = await collection.Find<StoreInformation>(_filter).Project<StoreInformation>(field).ToListAsync();
            return au;

        }
    }
}

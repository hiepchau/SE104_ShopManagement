using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Network.Get_database
{
    public class GetProducer
    {
        private MongoClient _client;
        private AppSession _session;
        private FilterDefinition<ProducerInformation> _filter;
        public GetProducer(MongoClient client, AppSession session, FilterDefinition<ProducerInformation> filter)
        {
            _client = client;
            _session = session;
            _filter = filter;
        }
        public async Task<List<ProducerInformation>> Get()
        {

            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<ProducerInformation>("ProducerInfromation");
            var field = Builders<ProducerInformation>.Projection
                .Include(p => p.ID)
                .Include(p => p.Name)
                .Include(p => p.Email)
                .Include(p => p.PhoneNumber);

            var au = await collection.Find<ProducerInformation>(_filter).Project<ProducerInformation>(field).ToListAsync();
            return au;

        }
    }
}

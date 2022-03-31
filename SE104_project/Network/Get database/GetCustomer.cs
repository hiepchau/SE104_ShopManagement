using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Network.Get_database
{
    public class GetCustomer
    {
        private MongoClient _client;
        private AppSession _session;
        private FilterDefinition<CustomerInformation> _filter;
        public GetCustomer(MongoClient client, AppSession session, FilterDefinition<CustomerInformation> filter)
        {
            _client = client;
            _session = session;
            _filter = filter;
        }
        public List<CustomerInformation> Get()
        {

            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<CustomerInformation>("CustomerInformation");
            var field = Builders<CustomerInformation>.Projection
                .Include(p => p.ID)
                .Include(p => p.Name)
                .Include(p => p.Email)
                .Include(p => p.PhoneNumber)
                .Include(p=>p.CustomerLevel);

            var au = collection.Find<CustomerInformation>(_filter).Project<CustomerInformation>(field).ToList();
            return au;

        }
    }
}

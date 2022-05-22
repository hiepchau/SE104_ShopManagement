using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Network.Get_database
{
    class GetMembership
    {
        private MongoClient _client;
        private AppSession _session;
        private FilterDefinition<MembershipInformation> _filter;

        public GetMembership(MongoClient cl, AppSession ses, FilterDefinition<MembershipInformation> fil)
        {
            _client = cl;
            _session = ses;
            _filter = fil;
        }

        public async Task<List<MembershipInformation>> Get()
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<MembershipInformation>("MembershipInformation");
            var field = Builders<MembershipInformation>.Projection
                .Include(x => x.ID)
                .Include(X => X.name)
                .Include(x => x.priority)
                .Include(x=>x.condition)
                .Include(x => x.isActivated);

            return await collection.Find<MembershipInformation>(_filter).Project<MembershipInformation>(field).ToListAsync();
        }
    }
}

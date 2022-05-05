using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Network.Update_database
{
    public class UpdateUserInformation
    {
        private MongoClient _client;
        private AppSession _session;
        private FilterDefinition<UserInfomation> _filter;
        private UpdateDefinition<UserInfomation> _update;
        public UpdateUserInformation(MongoClient cl, AppSession ses, FilterDefinition<UserInfomation> filter, UpdateDefinition<UserInfomation> update)
        {
            _client = cl;
            _session = ses;
            _filter = filter;
            _update = update;
        }
        public async Task<string> update()
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<UserInfomation>("UserInformation");
            object o = await collection.UpdateOneAsync(_filter, _update);
            string s = o.ToString();
            return s;
        }
    }
}

using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Services
{
    public abstract class IDGenerator
    {
        protected AppSession _session;
        protected MongoClient _client;
        public IDGenerator(AppSession session, MongoClient client)
        {
            this._session = session;
            this._client = client;
        }

        public abstract Task<string> Generate();
    }
}

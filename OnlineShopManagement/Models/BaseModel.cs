using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models
{
    public abstract class BaseModel
    {
        protected MongoClient _client;
        protected AppSession _session;
        public BaseModel(MongoClient client, AppSession session)
        {
            _client = client;
            _session = session;
        }
    }
}

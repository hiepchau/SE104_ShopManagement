﻿using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Services
{
    public class AutoStockingIDGenerator : IDGenerator
    {
        public AutoStockingIDGenerator(AppSession session, MongoClient client) : base(session, client)
        {
        }

        public override async Task<string> Generate()
        {
            string s = "";
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("SavedID");
            var projection = Builders<BsonDocument>.Projection.Include("ID");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", "SavedStock");
            var ls = await collection.Find(filter).Project(projection).ToListAsync();
            if (ls.Count < 1)
            {
                BsonDocument newdoc = new BsonDocument
                {
                    {"_id", "SavedStock" },
                    {"ID", "ST0" }
                };
                await collection.InsertOneAsync(newdoc);
                s = "ST0";
            }
            else if (ls.Count >= 1)
            {
                s = ls.First()["ID"].AsString;
                string tmp = s.Remove(0, 2);
                int index;
                if (int.TryParse(tmp, out index))
                {
                    tmp = "ST" + (index + 1).ToString();
                    s = tmp;
                    var collectioncheck = database.GetCollection<BsonDocument>("StockingInformation");
                    var projectioncheck = Builders<BsonDocument>.Projection.Include("DisplayID");
                    var filtercheck = Builders<BsonDocument>.Filter.Eq("DisplayID", tmp);
                    var lscheck = await collectioncheck.Find(filtercheck).Project(projectioncheck).ToListAsync();
                    if (lscheck.Count > 0)
                    {
                        tmp = "ST" + (index + 1).ToString();
                        BsonDocument newdoc = new BsonDocument
                        {
                            {"_id", "SavedStock" },
                            {"ID", tmp }
                        };
                        await collection.ReplaceOneAsync(filter, newdoc);
                        return Guid.NewGuid().ToString();
                    }
                    else
                    {
                        BsonDocument newdoc = new BsonDocument
                {
                    {"_id", "SavedStock" },
                    {"ID", tmp }
                };
                        await collection.ReplaceOneAsync(filter, newdoc);
                    }
                }
                else
                {
                    return Guid.NewGuid().ToString();
                }
            }
            return s;
        }
    }
}

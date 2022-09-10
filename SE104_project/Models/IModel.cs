using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Models
{
    public interface IModel<T> where T : EntityBase
    {
        Task<List<T>> GetEntity(FilterDefinition<T> filter);
        Task<(bool isSuccessful,string message)> Register(T registob);
        Task<(bool isSuccessful,string message)> Update(FilterDefinition<T> filter);
    }
}

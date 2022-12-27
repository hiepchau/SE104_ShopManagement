using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models
{
    public class ModelFactory : IModelFactory
    {
        public TModel CreateModel<TModel>()
        {
            throw new NotImplementedException();
        }
    }
}

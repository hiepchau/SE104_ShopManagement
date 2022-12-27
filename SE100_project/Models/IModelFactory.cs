using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models
{
    public delegate TModel ModelCreator<TModel>();
    public interface IModelFactory
    {
        TModel CreateModel<TModel>();
    }
}

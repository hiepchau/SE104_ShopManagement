using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Services.Common
{
    public interface IViewState
    {
        event Action StateChanged;
    }
}

using SE104_OnlineShopManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Services.Common
{
    public interface IViewState
    {
        ViewModelBase CurrentMainViewModel { get; set; }
        event Action StateChanged;
    }
}

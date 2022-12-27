using SE104_OnlineShopManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Services.Common
{
	public class ViewStateNavigator<VM> : INavigator where VM : ViewModelBase
	{

		private readonly IViewState _viewstate;
		private readonly ViewModelCreator<VM> _createViewModel;

		public ViewStateNavigator(IViewState viewstate, ViewModelCreator<VM> createViewModel)
		{
			_viewstate = viewstate;
			_createViewModel = createViewModel;
		}

		public void Navigate()
		{
			_viewstate.CurrentMainViewModel = _createViewModel();
		}

	}
}

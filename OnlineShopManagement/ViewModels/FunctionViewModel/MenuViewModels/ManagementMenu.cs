using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.MenuViewModels
{
    public class ManagementMenu : MenuViewModel
    {
        public int _selectedItem = -1;
        public int selectedItem { get => _selectedItem; set
            {
                _selectedItem = value;
            } }
        public bool isOverallAllowed { get; set; }
        public bool isProductsAllowed { get; set; }
        public bool isProducTypeAllowed { get;set; }
        public ManagementMenu(ManagingFunctionsViewModel viewmodel, AppSession session, MongoConnect connect) : base(viewmodel, session, connect)
        {
            ChangeViewModelCommand = new RelayCommand<Object>(null, change);
            isOverallAllowed = Utils.RoleSeperator.managerRole(session);
            isProductsAllowed = Utils.RoleSeperator.managerRole(session);
            isProducTypeAllowed = Utils.RoleSeperator.managerRole(session);
        }

        public override void change(object o)
        {
            var v = o as ListBoxItem;
            if (v != null && v.Name == "Overall")
            {
                Console.WriteLine(v.Name);
                _viewModel.Currentdisplaying = new OverviewFunction(_session, _mongoConnect);
                _viewModel.CurrentDisplayPropertyChanged();

            }
            if (v != null && v.Name == "Orders")
            {
                Console.WriteLine(v.Name);
                _viewModel.Currentdisplaying = new OrdersFunction(_session, _mongoConnect);
                _viewModel.CurrentDisplayPropertyChanged();
            }
            if (v != null && v.Name == "Products")
            {
                Console.WriteLine(v.Name);
                _viewModel.Currentdisplaying = new ProductsFunction(_session, _mongoConnect, _viewModel, this);
                _viewModel.CurrentDisplayPropertyChanged();
            }
            if (v != null && v.Name == "ProductsType")
            {
                Console.WriteLine(v.Name);
                _viewModel.Currentdisplaying = new ProductsTypeFunction(_session, _mongoConnect);
                _viewModel.CurrentDisplayPropertyChanged();
            }
            if (v != null && v.Name == "Stock")
            {
                Console.WriteLine(v.Name);
                _viewModel.Currentdisplaying = new ImportProductsFunction(_session, _mongoConnect);
                _viewModel.CurrentDisplayPropertyChanged();
            }
            if (v != null && v.Name == "Storage")
            {
                Console.WriteLine(v.Name);
                _viewModel.Currentdisplaying = new WareHouseFunction(_session, _mongoConnect, _viewModel, this);
                _viewModel.CurrentDisplayPropertyChanged();
            }
        }
        public void changeSelectedItem(int i)
        {
            selectedItem = i;
            OnPropertyChanged(nameof(selectedItem));
        }
    }
}

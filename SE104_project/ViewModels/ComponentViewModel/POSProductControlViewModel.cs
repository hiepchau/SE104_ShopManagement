using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Selling_functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{

    public class POSProductControlViewModel:ViewModelBase
    {
        public ProductsInformation product { get; set; }
        public string name { get; set; }
        public long price { get; set; }
        public int quantity { get; set; }
        private IUpdateSelectedList _parent;

        public ICommand UpdateBoughtCommand { get; set; }
        public POSProductControlViewModel(ProductsInformation product, IUpdateSelectedList parent)
        {
            this.product = product;
            this._parent = parent;
            name = product.name;
            price = product.price;
            quantity = product.quantity;
            UpdateBoughtCommand = new RelayCommand<Object>(null, UpdateBought);
        }

        private void UpdateBought(Object o)
        {
            _parent.UpdateSelectedList(product);
        }

    }
}

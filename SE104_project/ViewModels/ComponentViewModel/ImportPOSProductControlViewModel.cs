using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Selling_functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    public class ImportPOSProductControlViewModel
    {
        public ProductsInformation product { get; set; }
        public string ID { get; set; }
        public string name { get; set; }
        public long price { get; set; }
        public ICommand DeleteProductsCommand { get; set; }
        private IUpdateSelectedList _parent;
        public ImportPOSProductControlViewModel(ProductsInformation product, IUpdateSelectedList parent)
        {
            this.ID = product.ID;
            this.product = product;
            this._parent = parent;
            name = product.name;
            price = product.price;
            DeleteProductsCommand = new RelayCommand<object>(null, deleteproduct);
        }

        private void deleteproduct(object o)
        {
            _parent.UpdateBoughtList(product);
        }
    }
}

using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    class ProductsControlViewModel : ViewModelBase
    {
        #region Properties
        public ProductsInformation product { get; set; }
        public string ID { get; private set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public long price { get; set; }
        public long StockCost { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
        private IUpdateProductList _parent;
        #endregion

        #region ICommand
        public ICommand EditProductsCommand { get; set; }
        public ICommand DeleteProductsCommand { get; set; }
        #endregion

        public ProductsControlViewModel(ProductsInformation products, IUpdateProductList parent)
        {
            this.product = products;
            ID = product.ID;
            name = product.name;
            quantity = product.quantity;
            price = product.price;
            StockCost = product.StockCost;
            Category = product.Category;
            Unit = product.Unit;
            _parent = parent;
            DeleteProductsCommand = new RelayCommand<Object>(null, DeleteProduct);
        }

        #region Function
        public void DeleteProduct(Object o)
        {
            _parent.UpdateProductList(product);
        }
        #endregion
    }
}

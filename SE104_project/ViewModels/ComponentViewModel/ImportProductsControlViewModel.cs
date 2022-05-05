using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    class ImportProductsControlViewModel : ViewModelBase
    {
        #region Properties
        public ProductsInformation product { get; set; }
        public ControlNumericSnipper ImportQuantityNumeric { get; set; }
        public string ID { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public long StockCost { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }

        #endregion
        #region ICommand
        #endregion

        public ImportProductsControlViewModel(ProductsInformation product)
        {
            this.product = product;
            ImportQuantityNumeric = new ControlNumericSnipper(999);
            ID = product.ID;
            name = product.name;
            quantity = product.quantity;
            StockCost = product.StockCost;
            Category = product.Category;
            Unit = product.Unit;
        }

        #region Function
        #endregion
    }
}

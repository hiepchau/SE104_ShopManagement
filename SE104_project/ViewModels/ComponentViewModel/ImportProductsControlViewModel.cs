﻿using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Selling_functions;
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
        public long sum { get; set; }
        private IUpdateSelectedList _parent;
        #endregion
        #region ICommand
        #endregion

        public ImportProductsControlViewModel(ProductsInformation product, IUpdateSelectedList parent)
        {
            this.product = product;
            ImportQuantityNumeric = new ControlNumericSnipper(9999);
            ID = product.ID;
            name = product.name;
            quantity = product.quantity;
            StockCost = product.StockCost;
            Category = product.Category;
            Unit = product.Unit;
            _parent = parent;
            sum = 0;
        }


        #region Function
        public void onAmountChanged()
        {
            sum = ImportQuantityNumeric.GetDetailNum() * StockCost;
            OnPropertyChanged(nameof(sum));
        }
        #endregion
    }
}
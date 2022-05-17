using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    public class WareHouseControlViewModel : ViewModelBase
    {
        #region Properties
        public ProductsInformation products { get; set; }
        public string ID { get; set; }
        public string displayID { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public long price { get; set; }
        public long StockCost { get; set; }
        #endregion

        #region ICommand
        public ICommand ViewProductCommand { get; set; }
        #endregion

        public WareHouseControlViewModel(ProductsInformation pro)
        {
            this.products = pro;
            ID = pro.ID;
            name = pro.name;
            quantity = pro.quantity;
            price = pro.price;
            StockCost = pro.StockCost;
            displayID = pro.displayID;
        }

        #region Function
        #endregion
    }
}

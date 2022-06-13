using Microsoft.Win32;
using SE104_OnlineShopManagement.Commands;
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
        public string price { get; set; }
        public string StockCost { get; set; }
        public string InWareHouseStockValue { get; set; }
        public string InWareHouseSellValue { get; set; }
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
            price = SeparateThousands(pro.price.ToString());
            StockCost = SeparateThousands(pro.StockCost.ToString());
            displayID = pro.displayID;
            long WarehouseCost = pro.StockCost * pro.quantity;
            long WarehousePrice = pro.price * pro.quantity;
            InWareHouseStockValue = SeparateThousands(WarehouseCost.ToString());
            InWareHouseSellValue = SeparateThousands(WarehousePrice.ToString());
        }

        #region Function
        public string SeparateThousands(String text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                ulong valueBefore = ulong.Parse(text, System.Globalization.NumberStyles.AllowThousands);
                string res = String.Format(culture, "{0:N0}", valueBefore);
                return res;
            }
            return "";
        }


        #endregion
    }
}

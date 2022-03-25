using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class ProductsInformation
    {
        public string name { get; set; }    
        public int quantity { get; set; }   
        public long price { get; set; }
        public long StockCost { get; set; } 
        public string Category { get; set; }    
        public ProducerInformation ProducerInformation { get; set; }
        public ProductsInformation(string Name, int quan, long price, long cost, string category, ProducerInformation pcinfo)
        {
            this.name = Name;
            this.quantity = quan;
            this.price = price;
            this.StockCost = cost;
            this.Category = category;
            this.ProducerInformation = pcinfo;
        }
    }
}

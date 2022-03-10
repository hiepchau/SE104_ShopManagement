using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.Entity
{
    public class ProductInfo
    {
        public Guid Id { get; private set; }   
        public string Name { get; private set; }
        public string NhaSX { get; private set; }
        public string Category { get; private set; }
        public int Amount { get; private set; }  
        public long Price { get; private set; }

    }
}

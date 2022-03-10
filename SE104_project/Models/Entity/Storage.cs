using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.Entity
{
    public class Storage
    {
        public Guid ProductId { get;private set; }
        public int Amount { get;private set;}
        public string Name { get;private set; }
        public int VonTonKho { get;private set; } 
        public int GiaTriTon { get;private set; }
    }
}

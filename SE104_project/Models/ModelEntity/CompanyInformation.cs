using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class CompanyInformation
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public CompanyInformation(string id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
    }
}

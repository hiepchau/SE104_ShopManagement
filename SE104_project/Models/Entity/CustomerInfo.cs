using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.Entity
{
    public class CustomerInfo
    {
        public Guid Id { get; private set; }
        public string Name { get;private set; }    
        public string Email { get;private set; }   
        public DateTime Birthday { get;private set; }
        public string PhoneNumber { get;private set; }

        public CustomerInfo(Guid id, string name, string email, DateTime bday, string phone)
        {
            this.Id = id;
            this.Name = name;
            this.Email = email;
            this.Birthday = bday;
            this.PhoneNumber = phone;
        }
    }
}

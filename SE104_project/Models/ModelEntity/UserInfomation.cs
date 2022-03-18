using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public enum Role
    {
        Owner=0,
        Manager=1,
        Employee=2,
    }
    public class UserInfomation
    {
        public string ID { get; set; }  
        public string Name { get; set; }
        public string Email { get; set; }   
        public string PhoneNumber { get; set; } 
        public CompanyInformation companyInformation { get; set; }
        public Role role { get; set; }  
        public UserInfomation(string id, string name, string email, string phonenumer, CompanyInformation compa, Role role)
        {
            this.ID = id;
            this.Name = name;   
            this.Email = email; 
            this.PhoneNumber = phonenumer;  
            this.companyInformation = compa;
            this.role = role;
        }
    }
}

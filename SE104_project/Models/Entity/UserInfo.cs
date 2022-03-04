using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.Entity
{
    public enum UserRole
    {
        Owner = 0,
        Manager = 1,
        staff = 2
    }
    public class UserInfo
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }
        public UserRole Role { get; private set; }
        public string Address { get; private set; }
        public string Email { get;private set; }
        public string PhoneNumber { get; private set; }
        public UserInfo(Guid Id, string Name, int Age, UserRole role, string Address, string Email, string PhoneNumber)
        {
            this.Id = Id;
            this.Name = Name;
            this.Age = Age;
            this.Role = role;
            this.Address = Address;
            this.Email = Email;
            this.PhoneNumber = PhoneNumber;
        }
    }
}

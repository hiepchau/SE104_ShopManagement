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
        public string Id { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }
        public UserRole Role { get; private set; }
        public string Address { get; private set; }
    }
}

using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    class EmployeeControlViewModel : ViewModelBase
    {
        #region Properties
        public UserInfomation user { get; set; }
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Role role { get; set; }
        public Gender gender { get; set; }
        public long salary { get; set; }
        public bool isActivated { get; set; }
        public string displayID { get; set; }
        public DateTime birthday { get; set; }
        private IUpdateEmployeeList _parent;
        #endregion

        #region ICommand
        public ICommand EditEmployeeCommand { get; set; }
        public ICommand DeleteEmployeeCommand { get; set; }
        #endregion
        public EmployeeControlViewModel(UserInfomation user, IUpdateEmployeeList parent)
        {
            this.user = user;
            ID = user.ID;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            role = user.role;
            gender= user.gender;
            salary = user.salary;
            isActivated = user.isActivated;
            birthday = user.birthDay;
            displayID = user.displayID;
            _parent = parent;
            EditEmployeeCommand = new RelayCommand<Object>(null, editEmployee);
            DeleteEmployeeCommand = new RelayCommand<Object>(null, deleteEmployee);
        }

        #region Function
        public void deleteEmployee(Object o)
        {
            _parent.UpdateEmployeeList(user);
        }
        public void editEmployee(Object o)
        {
            _parent.EditEmployee(user);
        }
        #endregion
    }
}

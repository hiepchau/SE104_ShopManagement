using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    class MembershipControlViewModel : ViewModelBase
    {
        #region Properties
        public MembershipInformation membership;
        public string ID { get; set; }
        public string NumberOrder { get; set; }
        public string name { get; set; }
        public int prio { get; set; }
        public long condition { get; set; }
        public bool isActivated { get; set; }
        private IUpdateMembershipList _parent;

        #endregion
        #region ICommand
        public ICommand EditMembershipCommand { get; set; }
        public ICommand DeleteMembershipCommand { get; set; }
        #endregion
        

        public MembershipControlViewModel(MembershipInformation membership, IUpdateMembershipList parent, string no)
        {
            this.membership = membership;
            ID = membership.ID;
            NumberOrder = no;
            name = membership.name;
            prio = membership.priority;
            condition = membership.condition;
            isActivated = membership.isActivated;
            _parent = parent;
            EditMembershipCommand = new RelayCommand<Object>(null, editMembership);
            DeleteMembershipCommand = new RelayCommand<Object>(null, deleteMembership);
        }

        #region Function
        public void deleteMembership(Object o)
        {
            _parent.UpdateMembershipList(membership);
        }
        public void editMembership(Object o)
        {
            _parent.EditMembership(membership);
        }
        #endregion
    }
}

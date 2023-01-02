using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    public class ProductsTypeControlViewModel : ViewModelBase
    {
        #region Properties
        public ProductTypeInfomation type { get; set; }
        public string ID { get; set; }
        public string NumberOrder { get; set; }
        public string name { get; set; }
        public string note { get; set; }
        public bool isActivated { get; set; }
        private IUpdateProductTypeList _parent;
        #endregion

        #region ICommand
        public ICommand EditProductTypeCommand { get; set; }
        public ICommand DeleteProductTypeCommand { get; set; }
        #endregion

        public ProductsTypeControlViewModel(ProductTypeInfomation type, IUpdateProductTypeList parent, string no)
        {
            this.NumberOrder = no;
            this.type = type;
            ID = type.ID;
            name = type.name;
            note = type.note;
            
            isActivated = type.isActivated;
            _parent = parent;
            EditProductTypeCommand = new RelayCommand<Object>(null, EditType);
            DeleteProductTypeCommand = new RelayCommand<Object>(null, DeleteType);
        }

        #region Function
        public void DeleteType(Object o)
        {
            _parent.UpdateProductTypeList(type);
        }
        public void EditType(Object o)
        {
            _parent.EditProductType(type);
        }
        #endregion

    }
}

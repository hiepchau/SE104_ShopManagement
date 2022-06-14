using MaterialDesignThemes.Wpf;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Components;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Insert_database;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.MenuViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using SE104_OnlineShopManagement.Services;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using SE104_OnlineShopManagement.Models;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using System.Threading.Tasks;
using SE104_OnlineShopManagement.Network.Update_database;
using System.Linq;
using MongoDB.Bson;
using System.Windows;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    public interface IUpdateProductList
    {
        void UpdateProductList(ProductsInformation pro);
        void EditProduct(ProductsInformation pro);
    }
    class ProductsFunction : BaseFunction, IUpdateProductList
    {
        #region Properties
        public string productName { get; set; }
        public string productUnit { get; set; }
        public long productCost { get; set; }
        public long productPrice { get; set; }
        public int IsSelectedIndex { get; set; }
        public int IsSelectedProducerIndex { get; set; }
        public string searchString { get; set; }
        public BitmapImage productImage { get; set; }
        public ProductTypeInfomation SelectedProductsType { get; set; }
        public ProducerInformation SelectedProducer { get; set; }
        public ProductsControlViewModel SelectedProduct { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        private ManagingFunctionsViewModel managingFunction;
        private ManagementMenu ManagementMenu;
        public ObservableCollection<ProductsControlViewModel> listActiveItemsProduct { get; set; }
        public ObservableCollection<ProductsControlViewModel> listAllProduct { get; set; }
        public ObservableCollection<ProductTypeInfomation> ItemSourceProductsType { get; set; }
        public ObservableCollection<ProducerInformation> ItemSourceProducer { get; set; }

        #endregion

        #region ICommand
        //Product
        public ICommand OpenAddProductControlCommand { get; set; }
        public ICommand OpenImportProductsCommand { get; set; }
        public ICommand OpenProductsTypeCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ExportExcelCommand { get; set; }

        //AddProduct
        public ICommand SaveCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand TextChangedCommand { get; set; }
        public ICommand ReloadCommand { get; set; }

        #endregion
        public ProductsFunction(AppSession session, MongoConnect connect, ManagingFunctionsViewModel managingFunctionsViewModel, ManagementMenu managementMenu) : base(session, connect)
        {
            this._connection = connect;
            this._session = session;
            managingFunction = managingFunctionsViewModel;
            ManagementMenu = managementMenu;
            IsSelectedIndex = -1;
            IsSelectedProducerIndex = -1;

            ItemSourceProductsType = new ObservableCollection<ProductTypeInfomation>();
            listActiveItemsProduct = new ObservableCollection<ProductsControlViewModel>();
            ItemSourceProducer = new ObservableCollection<ProducerInformation>();
            listAllProduct = new ObservableCollection<ProductsControlViewModel>();
            //Get Data

            //
            TextChangedCommand = new RelayCommand<Object>(null, TextChangedHandle);
            OpenAddProductControlCommand = new RelayCommand<Object>(null, OpenAddProductControl);
            OpenProductsTypeCommand = new RelayCommand<Object>(null, OpenProductsType);
            OpenImportProductsCommand = new RelayCommand<Object>(null, OpenImportProducts);
            SelectImageCommand = new RelayCommand<Object>(null, SaveImage);
            SearchCommand = new RelayCommand<Object>(null, search);
            ReloadCommand = new RelayCommand<object>(null, Reload);
            ExportExcelCommand = new RelayCommand<Object>(null, ExportExcel);
            SelectedProductsType = null;
            SelectedProducer = null;
        }

        #region Function
        private async void Reload(object o = null)
        {
            await GetData();
            GetAllData();
            GetProductTypeData();
            GetProducerData();
        }
        //AddProduct
        public void OpenAddProductControl(Object o = null)
        {
            AddProductControl addProductControl = new AddProductControl();
            addProductControl.DataContext = this;
            DialogHost.Show(addProductControl, delegate (object sender, DialogClosingEventArgs args)
            {
                SetNull();
            });
            SaveCommand = new RelayCommand<Object>(CheckValidSave, SaveProduct);
            ExitCommand = new RelayCommand<Object>(null, exit =>
            {
                SetNull();
                DialogHost.CloseDialogCommand.Execute(null, null);
            });
        }
        //
        //Export Excel
        public void ExportExcel(Object o = null)
        {
            string filePath = "";
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel |*.xlsx"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
            }
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage p = new ExcelPackage())
                {
                    // Named
                    p.Workbook.Properties.Title = string.Format("Danh sách tồn kho");
                    p.Workbook.Worksheets.Add("sheet");

                    ExcelWorksheet ws = p.Workbook.Worksheets[0];
                    ws.Name = "DSTK";
                    ws.Cells.Style.Font.Size = 11;
                    ws.Cells.Style.Font.Name = "Calibri";
                    ws.Cells.Style.WrapText = true;
                    ws.Column(1).Width = 10;
                    ws.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 30;
                    ws.Column(4).Width = 20;
                    ws.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Column(5).Width = 20;
                    ws.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Column(6).Width = 20;
                    ws.Column(6).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Column(7).Width = 20;
                    ws.Column(7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Column(8).Width = 20;
                    ws.Column(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    // Create Column Header
                    string[] arrColumnHeader = { "STT", "Mã sản phẩm", "Tên sản phẩm", "Loại sản phẩm", "Đơn vị", "Tồn kho", "Giá nhập", "Giá bán"};

                    var countColHeader = arrColumnHeader.Count();


                    ws.Row(1).Height = 15;
                    ws.Cells[1, 1].Value = string.Format("Danh sách tồn kho");
                    ws.Cells[1, 1, 1, countColHeader].Merge = true;

                    ws.Cells[1, 1, 1, countColHeader].Style.Font.Bold = true;
                    ws.Cells[1, 1, 1, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int colIndex = 1;
                    int rowIndex = 2;

                    foreach (var item in arrColumnHeader)
                    {
                        ws.Row(rowIndex).Height = 15;
                        var cell = ws.Cells[rowIndex, colIndex];

                        var fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(255, 39, 119, 94);
                        cell.Style.Font.Bold = true;

                        var border = cell.Style.Border;
                        border.Bottom.Style =
                            border.Top.Style =
                            border.Left.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;

                        cell.Value = item;
                        colIndex++;
                    }

                    // Data
                    int i = 1;
                    foreach (ProductsControlViewModel control in listActiveItemsProduct)
                    {
                        //WareHouseControl control = new WareHouseControl();
                        ws.Row(rowIndex).Height = 15;
                        colIndex = 1;
                        rowIndex++;
                        string address = "A" + rowIndex + ":H" + rowIndex;
                        ws.Cells[address].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        if (rowIndex % 2 != 0)
                        {
                            ws.Cells[address].Style.Fill.BackgroundColor.SetColor(255, 255, 255, 255);
                        }
                        else
                        {
                            ws.Cells[address].Style.Fill.BackgroundColor.SetColor(255, 138, 220, 195);
                        }

                        ws.Cells[rowIndex, colIndex++].Value = i;
                        //ID
                        ws.Cells[rowIndex, colIndex++].Value = control.displayID;
                        //Name
                        ws.Cells[rowIndex, colIndex++].Value = control.name;
                        //txbProductsType
                        ws.Cells[rowIndex, colIndex++].Value = control.Category;
                        //txbUnit
                        ws.Cells[rowIndex, colIndex++].Value = control.Unit;
                        //txbQuantity
                        ws.Cells[rowIndex, colIndex++].Value = control.quantity;
                        //txbImportPrice
                        ws.Cells[rowIndex, colIndex++].Value = control.StockCost;
                        //txbSalesPrice
                        ws.Cells[rowIndex, colIndex++].Value = control.price;

                        i++;

                    }
                    //Save
                    Byte[] bin = p.GetAsByteArray();
                    File.WriteAllBytes(filePath, bin);
                }
                CustomMessageBox.Show("Xuất danh sách thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch
            {
            }

        }

        //
        public void OpenProductsType(Object o = null)
        {
            managingFunction.Currentdisplaying = new ProductsTypeFunction(Session, Connect);
            ManagementMenu.changeSelectedItem(3);
            managingFunction.CurrentDisplayPropertyChanged();
        }
        public void OpenImportProducts(Object o = null)
        {
            managingFunction.Currentdisplaying = new ImportProductsFunction(Session, Connect);
            ManagementMenu.changeSelectedItem(4);
            managingFunction.CurrentDisplayPropertyChanged();
        }
        public void TextChangedHandle(Object o = null)
        {
            (SaveCommand as RelayCommand<Object>).OnCanExecuteChanged();
        }
        public bool CheckValidSave(Object o = null)
        {

            if (String.IsNullOrEmpty(productName) ||
                String.IsNullOrEmpty(productUnit)
                || IsSelectedIndex == -1 || IsSelectedProducerIndex == -1
                || String.IsNullOrEmpty(productCost.ToString()) || productCost <= 0 
                || String.IsNullOrEmpty(productPrice.ToString()) || productPrice <= 0
                || productImage == null 
                || productCost > productPrice)
            {
                return false;
            }
            return true;
        }
        public async void SaveProduct(Object o = null)
        {
            if (SelectedProduct!=null)
            {
                var filter = Builders<ProductsInformation>.Filter.Eq("ID", SelectedProduct.ID);
                var update = Builders<ProductsInformation>.Update
                    .Set("ProductName", productName)
                    .Set("ProductPrice", productPrice)
                    .Set("ProductStockCost", productCost)
                    .Set("ProductCategory", SelectedProductsType.ID)
                    .Set("ProductProvider", SelectedProducer.ID)
                    .Set("Unit", productUnit);
                UpdateProductsInformation updater = new UpdateProductsInformation(_connection.client, _session, filter, update);
                var s = await updater.update();
                //Update Image
                ByteImage bimg = new ByteImage(SelectedProduct.ID, productImage);
                var filterImage = Builders<ByteImage>.Filter.Eq("ID", SelectedProduct.ID);
                var updateImage = Builders<ByteImage>.Update.Set("data", bimg);
                UpdateImage updaterImage = new UpdateImage(_connection.client,_session, filterImage, updateImage);
                var p = updaterImage.update();
                listActiveItemsProduct.Clear();
                _ = GetData();
                OnPropertyChanged(nameof(listActiveItemsProduct));
            }
            else
            {
                int flag = CheckExist();
                switch (flag)
                {
                    case 0:
                        CustomMessageBox.Show("Sản phẩm đã tồn tại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    case 1:
                        SetActive(SelectedProduct);
                        break;
                    case 2:
                        ProductsInformation info = new ProductsInformation("", productName, 0, productPrice, productCost, SelectedProductsType.ID,
                    SelectedProducer.ID, productUnit, true, await new AutoProductsIDGenerator(_session, _connection.client).Generate());
                        RegisterProducts regist = new RegisterProducts(info, _connection.client, _session);
                        Task<string> task = regist.register();
                        string id = await task;

                        Task.WaitAll(task);
                        //Register Image
                        ByteImage bimg = new ByteImage(id, productImage);
                        RegisterByteImage registImage = new RegisterByteImage(bimg, _connection.client, _session);
                        await registImage.register();
                        info.ID = id;
                        listActiveItemsProduct.Add(new ProductsControlViewModel(info, this));
                        listAllProduct.Add(new ProductsControlViewModel(info, this));
                        OnPropertyChanged(nameof(listActiveItemsProduct));
                        Console.WriteLine(id);
                        SelectedProductsType = null;
                        SelectedProducer = null;
                        break;

                }
            }
            DialogHost.CloseDialogCommand.Execute(null, null);

            CustomMessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            //Set Null
            SetNull();
        }
        public void SaveImage(Object o = null)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "image jpeg(*.jpg)|*.jpg|image png(*.png)|*.png";
            ofd.DefaultExt = ".jpeg";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                BitmapImage tmp = new BitmapImage(new Uri(ofd.FileName));
                productImage = tmp;
                OnPropertyChanged(nameof(productImage));
                (SaveCommand as RelayCommand<object>).OnCanExecuteChanged();
            }
        }
        public void UpdateProductList(ProductsInformation pro)
        {
            var result = CustomMessageBox.Show("Bạn có chắc chắn muốn xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                int i = 0;
                if (listActiveItemsProduct.Count > 0)
                {
                    foreach (ProductsControlViewModel ls in listActiveItemsProduct)
                    {
                        if (ls.product.ID.Equals(pro.ID))
                        {
                            SetUnactive(ls);
                            listAllProduct.Clear();
                            GetAllData();
                            break;
                        }
                        i++;
                    }
                    listActiveItemsProduct.RemoveAt(i);
                    OnPropertyChanged(nameof(listActiveItemsProduct));
                }
                else
                {
                    return;
                }
            }
            else
            {
                CustomMessageBox.Show("Xóa không thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        public void EditProduct(ProductsInformation pro)
        {
            if (listActiveItemsProduct.Count > 0)
            {
                foreach (ProductsControlViewModel ls in listActiveItemsProduct)
                {
                    if (ls.product.Equals(pro))
                    {
                        SelectedProduct = ls;
                        break;
                    }
                }
            }
            else
            {
                return;
            }
            int ProductTypeIndex = 0, ProducerIndex = 0;
            foreach (ProductTypeInfomation ls in ItemSourceProductsType)
            {
                if (ls.ID == SelectedProduct.Category)
                {
                    SelectedProductsType = ls;
                    IsSelectedIndex = ProductTypeIndex;
                    break;
                }
                ProductTypeIndex++;
            }
            foreach (ProducerInformation ls in ItemSourceProducer)
            {
                if (ls.ID == SelectedProduct.Producer)
                {
                    SelectedProducer = ls;
                    IsSelectedProducerIndex = ProducerIndex;    
                    break;
                }
                ProducerIndex++;
            }
            productName = SelectedProduct.name;
            productCost = ConvertToNumber(SelectedProduct.StockCost);
            productPrice = ConvertToNumber(SelectedProduct.price);
            productUnit = SelectedProduct.Unit;
            GetImage(SelectedProduct);
            OnPropertyChanged(nameof(productName));
            OnPropertyChanged(nameof(productCost));
            OnPropertyChanged(nameof(productPrice));
            OnPropertyChanged(nameof(productUnit));
            OnPropertyChanged(nameof(SelectedProducer));
            OnPropertyChanged(nameof(SelectedProductsType));
            OpenAddProductControl();
        }
        public async void SetActive(ProductsControlViewModel productinfo)
        {
            var filter = Builders<ProductsInformation>.Filter.Eq("ID", productinfo.ID);
            var update = Builders<ProductsInformation>.Update.Set("isActivated", true);
            UpdateProductsInformation updater = new UpdateProductsInformation(_connection.client, _session, filter, update);
            var s = await updater.update();
            listActiveItemsProduct.Add(SelectedProduct);
            OnPropertyChanged(nameof(listActiveItemsProduct));
            Console.WriteLine(s);
            SelectedProduct = null;
        }
        public async void SetUnactive(ProductsControlViewModel productinfo)
        {
            if (productinfo.isActivated == true)
            {
                var filter = Builders<ProductsInformation>.Filter.Eq("ID", productinfo.ID);
                var update = Builders<ProductsInformation>.Update.Set("isActivated", false);
                UpdateProductsInformation updater = new UpdateProductsInformation(_connection.client, _session, filter, update);
                var s = await updater.update();
                Console.WriteLine(s);
                SelectedProduct = null;
            }
            else Console.WriteLine("Cant execute");
        }
        public int CheckExist()
        {
            foreach (ProductsControlViewModel ls in listActiveItemsProduct)
            {
                if (productName == ls.name)
                {
                    return 0;
                }
            }

            foreach (ProductsControlViewModel ls1 in listAllProduct)
            {
                if (productName == ls1.name)
                {
                    SelectedProduct = ls1;
                    //Set Active
                    return 1;
                }
            }
            return 2;
        }
        public void SetNull()
        {
            SelectedProduct = null;
            SelectedProducer = null;
            SelectedProductsType = null;
            productName = "";
            productPrice = 0;
            productUnit = "";
            productCost = 0;
            productImage = null;
            OnPropertyChanged(nameof(productName));
            OnPropertyChanged(nameof(productPrice));
            OnPropertyChanged(nameof(productImage));
            OnPropertyChanged(nameof(productCost));
            OnPropertyChanged(nameof(productUnit));
            OnPropertyChanged(nameof(SelectedProductsType));
            OnPropertyChanged(nameof(SelectedProducer));
        }
        private async void search(object o)
        {
            searchString = (o.ToString());
            if (string.IsNullOrEmpty(searchString))
            {
                listActiveItemsProduct.Clear();
                await GetData();
            }
            else
            {
                await getsearchdata();
            }
        }
        public void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        public void NumberValidationTextBox(object sender, System.Windows.Input.KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }
        public long ConvertToNumber(string str)
        {
            string[] s = str.Split(',');
            string tmp = "";
            foreach (string a in s)
            {
                tmp += a;
            }

            return long.Parse(tmp);
        }


        #endregion

        #region DB
        public async void GetImage(ProductsControlViewModel pro)
        {
            GetByteImage getter = new GetByteImage(_connection.client, _session, Builders<ByteImage>.Filter.Eq(p => p.obID, pro.ID));
            var ls = await getter.Get();
            if (ls.Count > 0)
            {
                productImage = ls.FirstOrDefault().convertByteToImage();
                OnPropertyChanged(nameof(productImage));
            }
        }
        private async Task GetData()
        {
            var filter = Builders<ProductsInformation>.Filter.Eq("isActivated", true);
            GetProducts getter = new GetProducts(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (ProductsInformation pro in ls)
            {
                var lscheck = await CheckInactiveCategory.listInactiveCategory(_connection.client, _session, pro);
                if (lscheck.Count == 0)
                {
                    listActiveItemsProduct.Add(new ProductsControlViewModel(pro, this));
                }
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(listActiveItemsProduct));
        }
        public async void GetAllData()
        {
            var filter = Builders<ProductsInformation>.Filter.Empty;
            GetProducts getter = new GetProducts(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (ProductsInformation pro in ls)
            {
                listAllProduct.Add(new ProductsControlViewModel(pro, this));
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(listAllProduct));
        }
        public async void GetProductTypeData()
        {
            var filter = Builders<ProductTypeInfomation>.Filter.Empty;
            GetProductType getter = new GetProductType(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (ProductTypeInfomation pro in ls)
            {
                ItemSourceProductsType.Add(pro);
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(ItemSourceProductsType));
        }
        public async void GetProducerData()
        {
            var filter = Builders<ProducerInformation>.Filter.Empty;
            GetProducer getter = new GetProducer(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (ProducerInformation producer in ls)
            {
                ItemSourceProducer.Add(producer);
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(ItemSourceProducer));
        }
        private async Task getsearchdata()
        {
            listActiveItemsProduct.Clear();
            OnPropertyChanged(nameof(listActiveItemsProduct));
            FilterDefinition<ProductsInformation> filter = Builders<ProductsInformation>.Filter.Eq(x => x.name, searchString);
            var tmp = new GetProducts(_connection.client, _session, filter);
            var ls = await tmp.Get();
            foreach (ProductsInformation pr in ls)
            {
                listActiveItemsProduct.Add(new ProductsControlViewModel(pr, this));
            }
            OnPropertyChanged(nameof(listActiveItemsProduct));
        }
        #endregion
    }
}

using MongoDB.Driver;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Components.Controls;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.MenuViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Linq;
using Microsoft.Win32;
using SE104_OnlineShopManagement.Services;
using SE104_OnlineShopManagement.Views.Pages.ManagementComponents;
using System.IO;
using System.Windows;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    public class WareHouseFunction : BaseFunction
    {
        #region Properties
        private MongoConnect _connection;
        private AppSession _session;
        private ManagingFunctionsViewModel managingFunction;
        private ManagementMenu ManagementMenu;
        public bool isLoaded { get; set; }
        public string searchString { get; set; }
        public ObservableCollection<WareHouseControlViewModel> listItemWareHouse { get; set; }
        #endregion

        #region Icommand
        public ICommand OpenImportProductsCommand { get; set; }
        public ICommand PreviousWareHousePageCommand { get; set; }
        public ICommand NextWareHousePageCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ReloadCommand { get; set; }
        public ICommand ExportExcelCommand { get; set; }

        #endregion

        public WareHouseFunction(AppSession session, MongoConnect connect, ManagingFunctionsViewModel managingFunctionsViewModel, ManagementMenu managementMenu) : base(session, connect)
        {
            this._session = session;
            this._connection = connect;
            listItemWareHouse = new ObservableCollection<WareHouseControlViewModel>();

            managingFunction = managingFunctionsViewModel;
            ManagementMenu = managementMenu;
            isLoaded = true;
            OpenImportProductsCommand = new RelayCommand<Object>(null, OpenImportProducts);
            SearchCommand = new RelayCommand<Object>(null, search);
            ReloadCommand = new RelayCommand<object>(null, Reload);
            ExportExcelCommand = new RelayCommand<Object>(null, ExportExcel);

        }

        #region Function
        public void OpenImportProducts(Object o = null)
        {
            managingFunction.Currentdisplaying = new ImportProductsFunction(Session, Connect);
            ManagementMenu.changeSelectedItem(4);
            managingFunction.CurrentDisplayPropertyChanged();
        }
        private async void search(object o)
        {
            searchString = (o.ToString());
            if (string.IsNullOrEmpty(searchString))
            {
                listItemWareHouse.Clear();
                await GetData();
            }
            else
            {
                await getsearchdata();
            }
        }
        public void ExportExcel(Object o = null)
        {
            string filePath = "";
            SaveFileDialog saveFileDialog = new SaveFileDialog
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
                    string[] arrColumnHeader = { "STT","Mã sản phẩm", "Tên sản phẩm", "Số lượng", "Vốn tồn kho", "Mua vào", "Bán ra", "Giá trị tồn"};

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
                    foreach(WareHouseControlViewModel control in listItemWareHouse)
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
                        //Quan
                        ws.Cells[rowIndex, colIndex++].Value = control.quantity;
                        //InWareHouseStockValue
                        ws.Cells[rowIndex, colIndex++].Value = control.InWareHouseStockValue;
                        //txtBuyProduct
                        ws.Cells[rowIndex, colIndex++].Value = control.StockCost;
                        //txtSellProduct
                        ws.Cells[rowIndex, colIndex++].Value = control.price;
                        //InWareHouseSellValue
                        ws.Cells[rowIndex, colIndex++].Value = control.InWareHouseSellValue;

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

        private async void Reload(object o = null)
        {
            await GetData();
        }
        #endregion

        #region DB
        private async Task GetData()
        {
            listItemWareHouse.Clear();
            var filter = Builders<ProductsInformation>.Filter.Eq("isActivated", true);
            GetProducts getter = new GetProducts(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (ProductsInformation pro in ls)
            {
                listItemWareHouse.Add(new WareHouseControlViewModel(pro));
            }
            Console.Write("Executed");
            isLoaded = false;
            OnPropertyChanged(nameof(isLoaded));
            OnPropertyChanged(nameof(listItemWareHouse));
        }
        private async Task getsearchdata()
        {
            listItemWareHouse.Clear();
            OnPropertyChanged(nameof(listItemWareHouse));
            FilterDefinition<ProductsInformation> filter = Builders<ProductsInformation>.Filter.Eq(x => x.name, searchString);
            var tmp = new GetProducts(_connection.client, _session, filter);
            var ls = await tmp.Get();
            foreach (ProductsInformation pr in ls)
            {
                listItemWareHouse.Add(new WareHouseControlViewModel(pr));
            }
            OnPropertyChanged(nameof(listItemWareHouse));
        }
        #endregion
    }
}

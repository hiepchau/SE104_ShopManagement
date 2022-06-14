using MaterialDesignThemes.Wpf;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Components;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Insert_database;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.MenuViewModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using SE104_OnlineShopManagement.Services;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using SE104_OnlineShopManagement.Network.Update_database;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;


namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    public interface IUpdateCustomerList
    {
        void EditCustomer(CustomerInformation cus);
    }
    class CustomerFunction : BaseFunction, IUpdateCustomerList
    {
        #region Properties
        public string customerName { get; set; }
        public string customerPhone { get; set; }
        public string customerCMND { get; set; }
        public string customerAddress { get; set; }
        public string searchString { get; set; }
        public int customerCount { get; set; }
        public string totalRevenue { get; set; }
        public MembershipInformation SelectedMembership { get; set; }

        private MongoConnect _connection;
        private AppSession _session;
        public int selectedItem { get; set; }
        public CustomerControlViewModel selectedCus { get; set; }
        public ObservableCollection<CustomerControlViewModel> listAllCustomer { get; set; }
        public ObservableCollection<MembershipInformation> ItemSourceMembership { get; set; }
        public ObservableCollection<CustomerControlViewModel> backupMemberlist { get; set; }
        public int selectedSort { get; set; }

        private ManagingFunctionsViewModel managingFunction;
        private CustomerSelectMenu customerSelectMenu;
        #endregion

        #region ICommand
        //Customer
        public ICommand LoadCustomerCommand { get; set; }
        public ICommand ExportExcelCommand { get; set; }
        public ICommand GoToNextPageCommandCus { get; set; }
        public ICommand GoToPreviousPageCommandCus { get; set; }
        public ICommand FindCustomerCommand { get; set; }
        public ICommand OpenAddCustomerControlCommand { get; set; }
        public ICommand SortCustomerCommand { get; set; }
        public ICommand CountCustomerCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand TextChangedCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand RankSelectCommand { get; set; }
        //Membership
        public ICommand OpenMemberShipControlCommand { get; set; }


        //AddCustomer
        public ICommand SaveCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public ICommand ReloadCommand { get; set; }

        public ICommand SortOptionChangedCommand { get; set; }
        #endregion

        public CustomerFunction(AppSession session, MongoConnect connect, ManagingFunctionsViewModel managingFunctionsViewModel, CustomerSelectMenu _customerSelectMenu) : base(session, connect)
        {
            this._connection = connect;
            this._session=session;
            managingFunction = managingFunctionsViewModel;
            customerSelectMenu = _customerSelectMenu;
            searchString = "";
            listAllCustomer = new ObservableCollection<CustomerControlViewModel>();
            backupMemberlist = new ObservableCollection<CustomerControlViewModel>();
            ItemSourceMembership = new ObservableCollection<MembershipInformation>();
            ReloadCommand = new RelayCommand<object>(null, reload);
            TextChangedCommand = new RelayCommand<Object>(null, TextChangedHandle);
            ExportExcelCommand = new RelayCommand<Object>(null, ExportExcel);

            OpenAddCustomerControlCommand = new RelayCommand<Object>(null, OpenAddCustomerControl);
            OpenMemberShipControlCommand = new RelayCommand<Object>(null, OpenMemberShipControl);
            SearchCommand = new RelayCommand<Object>(null, search);
            RankSelectCommand = new RelayCommand<object>(null, rankchanged);
            SortOptionChangedCommand = new RelayCommand<object>(null, sortchanged);
            selectedSort = -1;
            OnPropertyChanged(nameof(selectedSort));
            
        }

        #region Function
        private void rankchanged(object o)
        {
            backupMemberlist.Clear();
            if(SelectedMembership != null) {
                foreach (CustomerControlViewModel cs in listAllCustomer) {
                    if (cs.customer.CustomerLevel.Equals(SelectedMembership.ID))
                        backupMemberlist.Add(cs);
                }
                    
            
                if (selectedSort == 1) {
                    List<CustomerControlViewModel> lstmp = new List<CustomerControlViewModel>();
                    foreach(CustomerControlViewModel cs in backupMemberlist)
                    {
                        lstmp.Add(cs);
                    }
                    lstmp.Sort((x,y)=>x.Sum.CompareTo(y.Sum));
                    backupMemberlist.Clear();
                    foreach(CustomerControlViewModel item in lstmp)
                    {
                        backupMemberlist.Add(item);
                    }
                }

                if (selectedSort == 0)
                {
                    List<CustomerControlViewModel> lstmp = new List<CustomerControlViewModel>();
                    foreach (CustomerControlViewModel cs in backupMemberlist)
                    {
                        lstmp.Add(cs);
                    }
                    lstmp.Sort((x, y) => y.Sum.CompareTo(x.Sum));
                    backupMemberlist.Clear();
                    foreach (CustomerControlViewModel item in lstmp)
                    {
                        backupMemberlist.Add(item);
                    }
                }
                OnPropertyChanged(nameof(backupMemberlist));
            }
        }

        public void sortchanged(object o) {
            if (selectedSort == 1)
            {
                List<CustomerControlViewModel> lstmp = new List<CustomerControlViewModel>();
                foreach (CustomerControlViewModel cs in backupMemberlist)
                {
                    lstmp.Add(cs);
                }
                lstmp.Sort((x, y) => x.Sum.CompareTo(y.Sum));
                backupMemberlist.Clear();
                foreach (CustomerControlViewModel item in lstmp)
                {
                    backupMemberlist.Add(item);
                }
            }

            if (selectedSort == 0)
            {
                List<CustomerControlViewModel> lstmp = new List<CustomerControlViewModel>();
                foreach (CustomerControlViewModel cs in backupMemberlist)
                {
                    lstmp.Add(cs);
                }
                lstmp.Sort((x, y) => y.Sum.CompareTo(x.Sum));
                backupMemberlist.Clear();
                foreach (CustomerControlViewModel item in lstmp)
                {
                    backupMemberlist.Add(item);
                }
            }
            OnPropertyChanged(nameof(backupMemberlist));
        }

        public void OpenAddCustomerControl(Object o = null)
        {
            AddCustomerControl addCustomerControl = new AddCustomerControl();
            addCustomerControl.DataContext = this;
            DialogHost.Show(addCustomerControl, delegate (object sender, DialogClosingEventArgs args)
            {
                SetNull();
            });
            SaveCommand = new RelayCommand<Object>(CheckValidSave, SaveCustomer);

            ExitCommand = new RelayCommand<Object>(null, exit =>
            {
                SetNull();
                DialogHost.CloseDialogCommand.Execute(null, null);
            });

        }
        public void OpenMemberShipControl(Object o = null)
        {
            managingFunction.Currentdisplaying = new MembershipFunction(Session, Connect);
            customerSelectMenu.changeSelectedItem(1);
            managingFunction.CurrentDisplayPropertyChanged();
        }
        private async void reload(Object o = null)
        {
            await GetData();
            await GetMembershipData();
            Console.WriteLine("Executed membership for reload " + ItemSourceMembership.Count.ToString());
            Console.WriteLine("Executed customer for reload " + listAllCustomer.Count.ToString());
            //Display membership

            //Display element
            customerCount = (listAllCustomer.Count > 0) ? listAllCustomer.Count : 0;
            OnPropertyChanged(nameof(customerCount));

            //totalRevenue
            long displayTotalRevenue = 0;
            foreach (var member in listAllCustomer)
            {
                displayTotalRevenue += member.Sum;
            }
            
            totalRevenue = SeparateThousands(displayTotalRevenue.ToString());
            OnPropertyChanged(nameof(totalRevenue));

        }

        public string SeparateThousands(String text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                ulong valueBefore = ulong.Parse(text, System.Globalization.NumberStyles.AllowThousands);
                string res = String.Format(culture, "{0:N0}", valueBefore);
                return res;
            }
            return "";
        }
        public void TextChangedHandle(Object o = null)
        {
            (SaveCommand as RelayCommand<Object>).OnCanExecuteChanged();
        }
        public bool CheckValidSave(Object o = null)
        {
            if ((String.IsNullOrEmpty(customerName)
                || String.IsNullOrEmpty(customerPhone) || customerPhone.Length != 10
                || String.IsNullOrEmpty(customerAddress)
                || String.IsNullOrEmpty(customerCMND) || customerCMND.Length != 12))
            {
                return false;
            }
            return true;
        }
        public void SetNull(Object o = null)
        {
            customerAddress = "";
            customerCMND = "";
            customerName = "";
            customerPhone = "";
            selectedCus = null;
            OnPropertyChanged(nameof(customerCMND));
            OnPropertyChanged(nameof(customerPhone));
            OnPropertyChanged(nameof(customerName));
            OnPropertyChanged(nameof(customerAddress));
        }
        public async void SaveCustomer(Object o = null)
        {
            if (selectedCus != null)
            {
                var filter = Builders<CustomerInformation>.Filter.Eq("ID", selectedCus.ID);
                var update = Builders<CustomerInformation>.Update.Set("Name", customerName).Set("Phone", customerPhone).Set("CMND",customerCMND).Set("Address",customerAddress);
                UpdateCustomerInformation updater = new UpdateCustomerInformation(_connection.client, _session, filter, update);
                var s = await updater.update();
                listAllCustomer.Clear();
                _ = GetData();
                OnPropertyChanged(nameof(listAllCustomer));
            }
            else if (CheckExist() == false)
            {
                CustomerInformation info = new CustomerInformation("", customerName, customerPhone, "62a87836fc4e8cc93aa37d7d", customerCMND, customerAddress, true, await new AutoCustomerIDGenerator(_session, _connection.client).Generate());
                RegisterCustomer regist = new RegisterCustomer(info, _connection.client, _session);
                string s = await regist.register();
                listAllCustomer.Clear();
                listAllCustomer.Clear();
                _ = GetData();

                OnPropertyChanged(nameof(listAllCustomer));
                Console.WriteLine(s);
            }
            else
            {
                CustomMessageBox.Show("Thuộc tính đã tồn tại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CustomMessageBox.Show("Thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);

            DialogHost.CloseDialogCommand.Execute(null, null);

            //Set Null
            SetNull();
        }
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
                    p.Workbook.Properties.Title = string.Format("DANH SÁCH NHÀ CUNG CẤP");
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
                    // Create Column Header
                    string[] arrColumnHeader = { "STT", "Mã NCC", "Nhà cung cấp", "Địa chỉ", "Email", "SĐT", "Tổng tiền" };

                    var countColHeader = arrColumnHeader.Count();

                    //Merge column
                    ws.Row(1).Height = 15;
                    ws.Cells[1, 1].Value = string.Format("DANH SÁCH KHÁCH HÀNG");
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
                    foreach (CustomerControlViewModel control in listAllCustomer)
                    {
                        //WareHouseControl control = new WareHouseControl();
                        ws.Row(rowIndex).Height = 15;
                        colIndex = 1;
                        rowIndex++;
                        string address = "A" + rowIndex + ":G" + rowIndex;
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
                        ws.Cells[rowIndex, colIndex++].Value = control.displayId;
                        //Name
                        ws.Cells[rowIndex, colIndex++].Value = control.Name;
                        //Address
                        ws.Cells[rowIndex, colIndex++].Value = control.address;
                        //Phone
                        ws.Cells[rowIndex, colIndex++].Value = control.PhoneNumber;
                        //Membership
                        ws.Cells[rowIndex, colIndex++].Value = control.membership;
                        //Total
                        ws.Cells[rowIndex, colIndex++].Value = control.Sum;
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

        public void EditCustomer(CustomerInformation cus)
        {
            if (listAllCustomer.Count > 0)
            {
                foreach (CustomerControlViewModel ls in listAllCustomer)
                {
                    if (ls.customer.Equals(cus))
                    {
                        selectedCus = ls;
                        break;
                    }
                }
            }
            else
            {
                return;
            }
            customerName = selectedCus.Name;
            customerAddress = selectedCus.address;
            customerCMND = selectedCus.cmnd;
            customerPhone = selectedCus.PhoneNumber;
            OnPropertyChanged(nameof(customerCMND));
            OnPropertyChanged(nameof(customerPhone));
            OnPropertyChanged(nameof(customerName));
            OnPropertyChanged(nameof(customerAddress));
            OpenAddCustomerControl();
        }
        public async void SetActive(CustomerControlViewModel cusinfo)
        {
            var filter = Builders<CustomerInformation>.Filter.Eq("ID", cusinfo.ID);
            var update = Builders<CustomerInformation>.Update.Set("isActivated", true);
            UpdateCustomerInformation updater = new UpdateCustomerInformation(_connection.client, _session, filter, update);
            var s = await updater.update();
            listAllCustomer.Add(selectedCus);
            OnPropertyChanged(nameof(listAllCustomer));
            selectedCus=null;
            Console.WriteLine(s);
        }
        public async void SetUnactive(CustomerControlViewModel cusinfo)
        {
            if (cusinfo.isActivated == true)
            {
                var filter = Builders<CustomerInformation>.Filter.Eq("ID", cusinfo.ID);
                var update = Builders<CustomerInformation>.Update.Set("isActivated", false);
                UpdateCustomerInformation updater = new UpdateCustomerInformation(_connection.client, _session, filter, update);
                var s = await updater.update();
                Console.WriteLine(s);
                selectedCus = null;
            }
            else Console.WriteLine("Cant execute");
        }
        public bool CheckExist()
        {
            foreach (CustomerControlViewModel ls in listAllCustomer)
            {
                if (customerCMND == ls.cmnd || customerPhone == ls.PhoneNumber)
                {
                    selectedCus = ls;
                    return true;
                }
            }
            return false;
        }
        public void SetNull()
        {
            customerName = "";
            customerPhone = "";
            customerCMND = "";
            customerAddress = "";
            OnPropertyChanged(nameof(customerName));
            OnPropertyChanged(nameof(customerPhone));
            OnPropertyChanged(nameof(customerCMND));
            OnPropertyChanged(nameof(customerAddress));
        }
        public void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        public void NumberValidationTextBox(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }
        private async void search(object o)
        {
            searchString = (o.ToString());
            if (string.IsNullOrEmpty(searchString))
            {
                listAllCustomer.Clear();
                await GetData();
            }
            else
            {
                await getsearchdata();
            }
        }
        #endregion

        #region DB

        public async Task GetData()
        {
            listAllCustomer.Clear();
            backupMemberlist.Clear();
            var filter = Builders<CustomerInformation>.Filter.Empty;
            GetCustomer getter = new GetCustomer(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (CustomerInformation cus in ls)
            {
                long sum = await GetSumCustomer(cus);
                listAllCustomer.Add(new CustomerControlViewModel(cus, sum, this)) ;
                backupMemberlist.Add(new CustomerControlViewModel(cus, sum, this));
            }
            Console.Write("Executed");
        }
        public async Task GetMembershipData()
        {
            ItemSourceMembership.Clear();
            var filter = Builders<MembershipInformation>.Filter.Empty;
            GetMembership getter = new GetMembership(_connection.client, _session, filter);
            Task<List<MembershipInformation>> task = getter.Get();
            var ls = await task;
            Task.WaitAll(task);
            foreach (MembershipInformation mem in ls)
            {
                ItemSourceMembership.Add(mem);
            }
            Console.WriteLine("Executed member ship "+ ItemSourceMembership.Count.ToString());
            OnPropertyChanged(nameof(ItemSourceMembership));
        }
        private async Task getsearchdata()
        {
            backupMemberlist.Clear();
            OnPropertyChanged(nameof(backupMemberlist));
            FilterDefinition<CustomerInformation> filter = Builders<CustomerInformation>.Filter.Eq(x => x.Name, searchString);
            var tmp = new GetCustomer(_connection.client, _session, filter);
            var ls = await tmp.Get();
            foreach (CustomerInformation pr in ls)
            {
                long sum = await GetSumCustomer(pr);
                backupMemberlist.Add(new CustomerControlViewModel(pr,sum, this));
            }
            OnPropertyChanged(nameof(backupMemberlist));
        }

        private async Task<long> GetSumCustomer(CustomerInformation cus)
        {
            long sum = 0;
            FilterDefinition<BillInformation> filter = Builders<BillInformation>.Filter.Eq(x => x.customer, cus.PhoneNumber);
            GetBills getter = new GetBills(_connection.client, _session, filter);
            var list = await getter.Get();
            if (list.Count > 0)
            {
                foreach(var item in list)
                {
                    sum += item.total;
                }
            }
            return sum;
        }



        #endregion
    }
}

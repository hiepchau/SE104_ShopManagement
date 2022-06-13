using MaterialDesignThemes.Wpf;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    public class StockTemplateViewmodel:ViewModelBase
    {
        #region Properties
        public string saleDay { get; set; }
        public string User { get; set; }
        public string customer { get; set; }
        public long total { get; set; }
        public string billID { get; set; }
        public string displayID { get; set; }
        public string state { get; set; }
        public string sateID { get; set; }
        public ObservableCollection<StockTemplateControlViewModel> listDetail { get; set; }
        private AppSession _session;
        private MongoConnect _connection;
        #endregion

        #region ICommand
        public ICommand ExitCommand { get; set; }
        #endregion

        public StockTemplateViewmodel(StockInformation stock, MongoConnect connect, AppSession session)
        {
            _connection = connect;
            _session = session;
            listDetail = new ObservableCollection<StockTemplateControlViewModel>();
            saleDay = stock.StockDay.ToString("dd/MM/yyyy HH:mm:ss");
            billID = stock.ID;
            displayID = stock.displayID;
            User = _session.CurrnetUser.LastName;
            customer = "";
            total = stock.total;
            state = "";
            sateID = "Mã phiếu nhập";

            ExitCommand = new RelayCommand<Object>(null, exit =>
            {

                DialogHost.CloseDialogCommand.Execute(null, null);
            });
            getdata();
        }

        #region Function
        #endregion

        #region DB
        private async void getdata()
        {
            FilterDefinition<StockDetails> filter = Builders<StockDetails>.Filter.Eq(x => x.stockID, billID);
            var tmp = new GetStockingDetail(_connection.client, _session, filter);
            var ls = await tmp.Get();

            int i = 1;
            foreach (StockDetails bill in ls)
            {

                listDetail.Add(new StockTemplateControlViewModel(bill, _connection, _session, i.ToString()));
                i++;
            }
            OnPropertyChanged(nameof(listDetail));
        }
    }
    #endregion
}

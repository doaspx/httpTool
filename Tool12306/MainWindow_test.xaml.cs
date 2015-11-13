using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tool12306.Windows;
using WPF_hl_17xy_cn;
using Tool12306.Models;
using System.IO;

namespace Tool12306
{
    /// <summary>
    /// Interaction logic for MainWindow_test.xaml
    /// </summary>
    public partial class MainWindow_test : Window
    {
        private HttpClient _client;

        private string _randCode;

        public MainWindow_test()
        {
            InitializeComponent();

            _client = HttpClient.BeginSession("12306");
            //if (_client.Restore())
            //{
            //    new _12306Proxy(_client).T();
            //}

            _ctlListForm.ItemsSource = Cities.Data;
            _ctlListTo.ItemsSource = Cities.Data;


            _ctlOrderTrain.ItemsSource = Trains.Data;


            _ctlListPassenger.ItemsSource = Passengers.Data;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow window = new LoginWindow(_client);
            if (window.ShowDialog() == true)
            {
                _randCode = window.LoginModel.RandCode;
                _client.Store();

            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            List<TicketModel> tickets = new List<TicketModel>();

            QueryModel query = new QueryModel();
            query.orderRequest_from_station_telecode = (_ctlListForm.SelectedItem as City).Code;
            query.orderRequest_to_station_telecode = (_ctlListTo.SelectedItem as City).Code;
            //query.orderRequest_train_date = _ctlDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            _12306Proxy proxy = new _12306Proxy(_client);

            for (int day = 0; day < 10; day++)
            {
                query.orderRequest_train_date = DateTime.Now.AddDays(day).ToString("yyyy-MM-dd");
                tickets.AddRange(proxy.Query(query));
            }
            ListCollectionView GroupedCustomers = new ListCollectionView(tickets); GroupedCustomers.GroupDescriptions.Add(new PropertyGroupDescription("StartDate"));
            _ctlDataGrid.ItemsSource = GroupedCustomers;
            
        }

        private void _ctlBtnOrder_Click(object sender, RoutedEventArgs e)
        {
            //_12306Proxy proxy = new _12306Proxy(_client);
            //ConfirmModel confirmModel = proxy.Submit(_ctlOrderTrain.SelectedItem as TrainModel,
            //    _ctlOrderDate.SelectedDate.Value,
            //    _ctlOrderRandCode.Text);

            //InputRandCodeWindow window = new InputRandCodeWindow(_client);
            //if (window.ShowDialog() == true)
            //{
            //    confirmModel.randCode = window.RandCode;
            //    proxy.Confirm(confirmModel, new List<PassengerModel>(Passengers.Data));
            //}
        }

        private void _btnOrderYW_Click(object sender, RoutedEventArgs e)
        {
            //SeatModel seat = new SeatModel();
            //seat.seat = "3";
            //seat.seat_detail = "0";
            //seat.seat_detail_select = "0";

            //_12306Proxy proxy = new _12306Proxy(_client);
            //InputRandCodeWindow window = new InputRandCodeWindow(_client);
            //if (window.ShowDialog() == true)
            //{
            //    TicketModel ticket = (sender as Button).DataContext as TicketModel;
            //    ConfirmModel confirmModel = proxy.Submit(ticket, _ctlOrderRandCode.Text);

            //    confirmModel.randCode = window.RandCode;
            //    if (proxy.Confirm(confirmModel, new List<PassengerModel>(Passengers.Data), seat))
            //    {
            //        MessageBox.Show("订票成功!");
            //    }
            //    else
            //    {
            //        MessageBox.Show("订票失败!");
            //    }
            //}
        }

        private void _btnOrderYZ_Click(object sender, RoutedEventArgs e)
        {
            //SeatModel seat = new SeatModel();
            //seat.seat = "1";
            //seat.seat_detail = "0";
            //seat.seat_detail_select = "0";

            //_12306Proxy proxy = new _12306Proxy(_client);
            //InputRandCodeWindow window = new InputRandCodeWindow(_client);
            //if (window.ShowDialog() == true)
            //{
            //    TicketModel ticket = (sender as Button).DataContext as TicketModel;
            //    ConfirmModel confirmModel = proxy.Submit(ticket, _ctlOrderRandCode.Text);

            //    confirmModel.randCode = window.RandCode;
            //    if (proxy.Confirm(confirmModel, new List<PassengerModel>(Passengers.Data), seat))
            //    {
            //        MessageBox.Show("订票成功!");
            //    }
            //    else
            //    {
            //        MessageBox.Show("订票失败!");
            //    }
            //}
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            object o = _ctlListPassenger.SelectedItems;
        }
    }
}

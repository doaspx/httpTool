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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpClient _client;

        private string _randCode;

        private QueryModel _query = new QueryModel();

        public MainWindow()
        {
            InitializeComponent();

            _client = HttpClient.BeginSession("12306");
            //if (_client.Restore())
            //{
            //    new _12306Proxy(_client).T();
            //}

            _ctlListForm.ItemsSource = Cities.Data;
            _ctlListTo.ItemsSource = Cities.Data;

            _ctlListPassenger.ItemsSource = Passengers.Data;
            _ctlListTrainType.ItemsSource = Trains.Types;

            LoginWindow window = new LoginWindow(_client);
            if (window.ShowDialog() == true)
            {
                _randCode = window.LoginModel.RandCode;
                _client.Store();

                button1.IsEnabled = false;
            }
            else
            {
                button1.IsEnabled = true;
            }
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
            int day = 1;
            List<TicketModel> tickets = new List<TicketModel>();

            _query.trainClass = string.Empty;
            foreach (object item in _ctlListTrainType.SelectedItems)
            {
                TrainType tt = (TrainType)item;
                _query.trainClass += tt.Code + "%23";
            }

            _query.from_station_telecode_name = (_ctlListForm.SelectedItem as City).Name;
            _query.to_station_telecode_name = (_ctlListTo.SelectedItem as City).Name;
            _query.orderRequest_from_station_telecode = (_ctlListForm.SelectedItem as City).Code;
            _query.orderRequest_to_station_telecode = (_ctlListTo.SelectedItem as City).Code;
            //query.orderRequest_train_date = _ctlDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            _12306Proxy proxy = new _12306Proxy(_client);

            _query.orderRequest_train_date = DateTime.Now.AddDays(day).ToString("yyyy-MM-dd");
            tickets.AddRange(proxy.Query(_query));

            ListCollectionView GroupedCustomers = new ListCollectionView(tickets); GroupedCustomers.GroupDescriptions.Add(new PropertyGroupDescription("StartDate"));
            _ctlDataGrid.ItemsSource = GroupedCustomers;
            
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            int day = 10;
            List<TicketModel> tickets = new List<TicketModel>();

            _query.trainClass = string.Empty;
            foreach (object item in _ctlListTrainType.SelectedItems)
            {
                TrainType tt = (TrainType)item;
                _query.trainClass += tt.Code + "%23";
            }

            _query.from_station_telecode_name = (_ctlListForm.SelectedItem as City).Name;
            _query.to_station_telecode_name = (_ctlListTo.SelectedItem as City).Name;
            _query.orderRequest_from_station_telecode = (_ctlListForm.SelectedItem as City).Code;
            _query.orderRequest_to_station_telecode = (_ctlListTo.SelectedItem as City).Code;
            //query.orderRequest_train_date = _ctlDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            _12306Proxy proxy = new _12306Proxy(_client);

            _query.orderRequest_train_date = DateTime.Now.AddDays(day).ToString("yyyy-MM-dd");
            tickets.AddRange(proxy.Query(_query));

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
            SeatModel seat = new SeatModel();
            seat.seat = "3";
            seat.seat_detail = "0";
            seat.seat_detail_select = "0";

            _12306Proxy proxy = new _12306Proxy(_client);
            InputRandCodeWindow window = new InputRandCodeWindow(_client);
            if (window.ShowDialog() == true)
            {
                TicketModel ticket = (sender as Button).DataContext as TicketModel;
                ConfirmModel confirmModel = proxy.Submit(_query, ticket, window.RandCode);

                confirmModel.randCode = window.RandCode;
                if (proxy.Confirm(confirmModel, new List<PassengerModel>(Passengers.Data), seat))
                {
                    MessageBox.Show("订票成功!");
                }
                else
                {
                    MessageBox.Show("订票失败!");
                }
            }
        }

        private void _btnOrderYZ_Click(object sender, RoutedEventArgs e)
        {
            SeatModel seat = new SeatModel();
            seat.seat = "1";
            seat.seat_detail = "0";
            seat.seat_detail_select = "0";

            _12306Proxy proxy = new _12306Proxy(_client);
            InputRandCodeWindow window = new InputRandCodeWindow(_client);
            if (window.ShowDialog() == true)
            {
                TicketModel ticket = (sender as Button).DataContext as TicketModel;
                ConfirmModel confirmModel = proxy.Submit(_query, ticket, window.RandCode);

                confirmModel.randCode = window.RandCode;
                if (proxy.Confirm(confirmModel, new List<PassengerModel>(Passengers.Data), seat))
                {
                    MessageBox.Show("订票成功!");
                }
                else
                {
                    MessageBox.Show("订票失败!");
                }
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            object o = _ctlListPassenger.SelectedItems;
        }
    }

    public class NumColorConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool boolValue = (bool)value;
            if (boolValue)
            {
                return new SolidColorBrush(Colors.Green);
            }
            else
            {
                return new SolidColorBrush(Colors.Gray);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NumTextConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool boolValue = (bool)value;
            if (boolValue)
            {
                return "有";
            }
            else
            {
                return "无";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class NumEnableConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

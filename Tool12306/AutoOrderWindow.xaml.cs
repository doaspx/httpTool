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
using System.Threading;

namespace Tool12306
{
    /// <summary>
    /// Interaction logic for AutoOrderWindow.xaml
    /// </summary>
    public partial class AutoOrderWindow : Window
    {
        private HttpClient _client;

        private string _randCode;
        private int _day;
        private TrainInfo _selectedTrain;
        private List<PassengerModel> _selectedPassenger;
        private bool _isRunning = false;
        private SeatModel _selectedSeat;

        private Thread _thread;

        public AutoOrderWindow()
        {
            InitializeComponent();

            _client = HttpClient.BeginSession("12306");
            //if (_client.Restore())
            //{
            //    new _12306Proxy(_client).T();
            //}

            this.Loaded += new RoutedEventHandler(AutoOrderWindow_Loaded);
        }

        void AutoOrderWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoginWindow window = new LoginWindow(_client);
            if (window.ShowDialog() == true)
            {
                _client.Store();

                //button1.IsEnabled = false;
            }
            else
            {
                Close();
                return;
            }

            _12306Proxy proxy = new _12306Proxy(_client);


            //_ctlListPassenger.ItemsSource = proxy.GetOfflinePassengers();
            _ctlListPassenger.ItemsSource = proxy.GetPassengers();
            _ctlListTrain.ItemsSource = proxy.GetOfflineTrains();

            List<SeatModel> seats = new List<SeatModel>();
            #region 软卧
            SeatModel seatRWTop = new SeatModel();
            seatRWTop.caption = "软卧-上铺";
            seatRWTop.seat = "4";
            seatRWTop.seat_detail = "0";
            seatRWTop.seat_detail_select = "3";
            seats.Add(seatRWTop);
            SeatModel seatRWMiddle = new SeatModel();
            seatRWMiddle.caption = "软卧-中铺";
            seatRWMiddle.seat = "4";
            seatRWMiddle.seat_detail = "0";
            seatRWMiddle.seat_detail_select = "2";
            seats.Add(seatRWMiddle);
            SeatModel seatRWBottom = new SeatModel();
            seatRWBottom.caption = "软卧-下铺";
            seatRWBottom.seat = "4";
            seatRWBottom.seat_detail = "0";
            seatRWBottom.seat_detail_select = "1";
            seats.Add(seatRWBottom);
            #endregion

            #region 硬卧
            SeatModel seatYWTop = new SeatModel();
            seatYWTop.caption = "硬卧-上铺";
            seatYWTop.seat = "3";
            seatYWTop.seat_detail = "0";
            seatYWTop.seat_detail_select = "3";
            seats.Add(seatYWTop);
            SeatModel seatYWMiddle = new SeatModel();
            seatYWMiddle.caption = "硬卧-中铺";
            seatYWMiddle.seat = "3";
            seatYWMiddle.seat_detail = "0";
            seatYWMiddle.seat_detail_select = "2";
            seats.Add(seatYWMiddle);
            SeatModel seatYWBottom = new SeatModel();
            seatYWBottom.caption = "硬卧-下铺";
            seatYWBottom.seat = "3";
            seatYWBottom.seat_detail = "0";
            seatYWBottom.seat_detail_select = "1";
            seats.Add(seatYWBottom);
            #endregion

            SeatModel seatYZ = new SeatModel();
            seatYZ.caption = "硬座";
            seatYZ.seat = "1";
            seatYZ.seat_detail = "0";
            seatYZ.seat_detail_select = "0";
            seats.Add(seatYZ);
            _ctlListSeat.ItemsSource = seats;

            DateTime limitDate = new DateTime(2013,1,7);
            
            for (int index = 0; index < 20 && DateTime.Now.Date.AddDays(index) < limitDate; index++)
            {
                Button btn = new Button();
                btn.Width = 100;
                btn.Height = 14;
                btn.Margin = new Thickness(5,3,0,0);
                StackPanel sp = new StackPanel();
                btn.Content = sp;
                TextBlock t1 = new TextBlock();
                t1.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                t1.FontSize = 10;
                t1.Text = string.Format("{0}", DateTime.Now.AddDays(index).ToString("yyyy年MM月dd日"));
                TextBlock t2 = new TextBlock();
                t2.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                t2.Text = GetDayCountString(index);
                sp.Children.Add(t1);
                sp.Children.Add(t2);
                btn.Tag = index;
                btn.Click += new RoutedEventHandler(btn_Click);
                _ctlWrapPanel.Children.Add(btn);
            }
        }

        private string  GetDayCountString(int index)
        {
            switch (index)
            {
                case 0:
                    return "今天";
                case 1:
                    return "明天";
                case 2:
                    return "后天";
                default:
                    return string.Format("第{0}天",index + 1);
            }
        }

        void btn_Click(object sender, RoutedEventArgs e)
        {
            if (_isRunning)
            {
                return;
            }

            _selectedPassenger = new List<PassengerModel>();
            foreach (PassengerModel item in _ctlListPassenger.SelectedItems)
            {
                _selectedPassenger.Add(item);
            }
            _selectedTrain = _ctlListTrain.SelectedItem as TrainInfo;
            _selectedSeat = (_ctlListSeat.SelectedItem as SeatModel);
            if (_selectedPassenger.Count == 0 || _selectedSeat == null || _selectedTrain == null)
            {
                return;
            }

            InputRandCodeWindow window = new InputRandCodeWindow(_client);
            if (window.ShowDialog() == true)
            {
                _ctlBtnCancel.IsEnabled = true;
                _borderMask.Visibility = System.Windows.Visibility.Visible;

                _randCode = window.RandCode;
                _day = Convert.ToInt32((sender as Button).Tag);
                _isRunning = true;
                _thread = new Thread(Run);
                _thread.Start();
            }
        }

        private void Run()
        {
            int tryCount = -1;
            bool isFinished = false;
            string orderNo = null;
            string message = null;

            ConfirmModel confirmModel =null;
            int step = 1; // 1提交订单 2排队领号
            _12306Proxy proxy = new _12306Proxy(_client);
            QueryModel _query = new QueryModel();
            _query.trainClass = "QB#D#Z#T#K#QT#";// string.Empty;

            _query.from_station_telecode_name = _selectedTrain.Start;
            _query.to_station_telecode_name = _selectedTrain.Arrive;
            _query.orderRequest_from_station_telecode = _selectedTrain.StartCode;
            _query.orderRequest_to_station_telecode = _selectedTrain.ArriveCode;
            _query.orderRequest_train_no = _selectedTrain.No;
            //query.orderRequest_train_date = _ctlDate.SelectedDate.Value.ToString("yyyy-MM-dd");

            _query.orderRequest_train_date = DateTime.Now.AddDays(_day).ToString("yyyy-MM-dd");
            while (_isRunning)
            {
                if (step == 1)
                {
                    #region 查票 预定
                    Thread.Sleep(1000);
                    tryCount++;
                    List<TicketModel> tickets = proxy.Query2(_query, out message);

                    if (tickets == null || tickets.Count == 0)
                    {
                        _isRunning = false;
                        break;
                    }
                    else
                    {
                        foreach (TicketModel t in tickets)
                        {
                            if (_selectedSeat.seat == "4" && t.Num_RW)
                            {
                                // 软卧
                                confirmModel = proxy.Submit2(_query, t, _randCode, out message);

                                if (confirmModel == null)
                                {
                                    this.Dispatcher.Invoke(new Action<string, int>(OrderLog), message, tryCount);
                                    _isRunning = false;
                                    break;
                                }
                                else
                                {
                                    confirmModel.randCode = _randCode;
                                    if (proxy.Confirm2(confirmModel, _selectedPassenger, _selectedSeat, out message))
                                    {
                                        step = 2; // 前往排队领号
                                        this.Dispatcher.Invoke(new Action<string, int>(OrderLog), "提交成功,等待领号!", tryCount);
                                        break;
                                    }
                                    else
                                    {
                                        // 订购失败
                                        this.Dispatcher.Invoke(new Action<string, int>(OrderLog), message, tryCount);
                                    }
                                }
                            }
                            else if (_selectedSeat.seat == "3" && t.Num_YW)
                            {
                                // 硬卧
                                confirmModel = proxy.Submit2(_query, t, _randCode, out message);

                                if (confirmModel == null)
                                {
                                    this.Dispatcher.Invoke(new Action<string, int>(OrderLog), message, tryCount);
                                    _isRunning = false;
                                    break;
                                }
                                else
                                {
                                    confirmModel.randCode = _randCode;
                                    if (proxy.Confirm2(confirmModel, _selectedPassenger, _selectedSeat, out message))
                                    {
                                        step = 2; // 前往排队领号
                                        this.Dispatcher.Invoke(new Action<string, int>(OrderLog), "提交成功,等待领号!", tryCount);
                                        break;
                                    }
                                    else
                                    {
                                        // 订购失败
                                        this.Dispatcher.Invoke(new Action<string, int>(OrderLog), message, tryCount);
                                    }
                                }
                            }
                            else if (_selectedSeat.seat == "1" && t.Num_YZ)
                            {
                                // 硬座
                                confirmModel = proxy.Submit2(_query, t, _randCode, out message);
                                if (confirmModel == null)
                                {
                                    this.Dispatcher.Invoke(new Action<string, int>(OrderLog), message, tryCount);
                                    _isRunning = false;
                                    break;
                                }
                                else
                                {
                                    confirmModel.randCode = _randCode;
                                    if (proxy.Confirm2(confirmModel, _selectedPassenger, _selectedSeat, out message))
                                    {
                                        step = 2; // 前往排队领号
                                        this.Dispatcher.Invoke(new Action<string, int>(OrderLog), "提交成功,等待领号!", tryCount);
                                        break;
                                    }
                                    else
                                    {
                                        // 订购失败
                                        this.Dispatcher.Invoke(new Action<string, int>(OrderLog), message, tryCount);
                                    }
                                }
                            }
                            else
                            {
                                // 订购失败
                                this.Dispatcher.Invoke(new Action<string, int>(OrderLog), "暂无车票!", tryCount);
                            }
                        }
                    }
                    #endregion
                }
                else if (step == 2)
                {
                    int waitCount = 0;
                    Thread.Sleep(500);
                    orderNo = proxy.GetOrderNo(out waitCount, out message);
                    if (!string.IsNullOrEmpty(orderNo))
                    {
                        step = 3;
                    }
                    else
                    {
                        if (waitCount == 0)
                        {
                            _isRunning = false;
                            break;
                        }
                    }
                }
                else
                {
                    proxy.GoPay(orderNo, confirmModel, _selectedPassenger, _selectedSeat);

                    isFinished = true;
                    _isRunning = false;
                }
            }
            // 订购完毕
            if (isFinished)
            {
                this.Dispatcher.Invoke(new Action<string>(OrderSuccess), "订购成功!");
            }
            else
            {
                // 订购失败
                this.Dispatcher.Invoke(new Action<string>(OrderFailure), message);
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

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            object o = _ctlListPassenger.SelectedItems;
        }

        private void _ctlBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            _isRunning = false;
            _ctlBtnCancel.IsEnabled = false;
        }

        private void OrderSuccess(string message)
        {
            _borderMask.Visibility = System.Windows.Visibility.Collapsed;
            MessageBox.Show(message);
        }

        private void OrderFailure(string message)
        {
            _borderMask.Visibility = System.Windows.Visibility.Collapsed;
            MessageBox.Show(message);
        }

        private void OrderLog(string message, int tryCount)
        {
            _ctlTextLog.Text = message;
            _ctlTextTry.Text = tryCount.ToString();
        }
    }
}

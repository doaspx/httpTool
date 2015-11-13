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
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.IO;
using WPF_hl_17xy_cn;
using Tool12306.Models;

namespace Tool12306.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : System.Windows.Window
    {
        private HttpClient _client;
        public LoginModel LoginModel { get; private set; }

        public LoginWindow(HttpClient client)
        {
            InitializeComponent();

            _client = client;


            OnGet();
        }

        private void OnGet()
        {
            _12306Proxy proxy = new _12306Proxy(_client);

            LoadVerifyImage();

            LoginModel = proxy.GetLoginModel();
            this.DataContext = LoginModel;
        }

        private void LoadVerifyImage()
        {
            _12306Proxy proxy = new _12306Proxy(_client);

            byte[] data = proxy.GetVerifyImage();
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = new MemoryStream(data);
            bi.EndInit();
            image1.Source = bi;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            LoginModel.Password = textBox2.Password;
            if (new _12306Proxy(_client).Login(LoginModel))
            {
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("登录失败!", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                LoadVerifyImage();
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

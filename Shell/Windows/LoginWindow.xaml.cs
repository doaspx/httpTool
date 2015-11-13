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
using WPF_hl_17xy_cn.hl_17xy_cn;
using System.Text.RegularExpressions;
using System.IO;
using hl_17xy_cn;

namespace WPF_hl_17xy_cn.Windows
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
            HLProxy proxy = new HLProxy(_client);
            LoginModel = proxy.GetLoginModel();

            byte[] data = proxy.GetVerifyImage(LoginModel.VerifyNumber);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = new MemoryStream(data);
            bi.EndInit();
            image1.Source = bi;

            this.DataContext = LoginModel;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string userId = new HLProxy(_client).Login(LoginModel);
            if(!string.IsNullOrEmpty(userId))
            {
                DialogResult = true;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

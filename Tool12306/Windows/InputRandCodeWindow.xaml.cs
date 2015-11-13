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
using WPF_hl_17xy_cn;
using System.IO;

namespace Tool12306.Windows
{
    /// <summary>
    /// Interaction logic for InputRandCodeWindow.xaml
    /// </summary>
    public partial class InputRandCodeWindow : Window
    {
        public string RandCode { get; set; }

        public InputRandCodeWindow(HttpClient client)
        {
            InitializeComponent();



            _12306Proxy proxy = new _12306Proxy(client);
            byte[] data = proxy.GetVerifyImage2();
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = new MemoryStream(data);
            bi.EndInit();
            this.image1.Source = bi;

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            RandCode = textBox1.Text;
            DialogResult = true;
        }
    }
}

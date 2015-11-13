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
using WPF_hl_17xy_cn;
using System.Windows.Controls.Primitives;

namespace BaiduImage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpClient _client;
        public MainWindow()
        {
            InitializeComponent();

            _client = HttpClient.BeginSession("baiduimage");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_ctlWord.Text))
            {
                return;
            }
            BaiduImageProxy proxy = new BaiduImageProxy(_client);
            List<Pic> pics = proxy.Query(_ctlWord.Text);
            _ctlContainer.ItemsSource = pics;
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            PreviewView view = new PreviewView();
            view.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            view.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            view.PreviewMouseRightButtonUp += new MouseButtonEventHandler(view_PreviewMouseRightButtonUp);
            view.DataContext = img.Source;
            _layout.Children.Add(view);
        }

        void view_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            PreviewView view = sender as PreviewView;
            _layout.Children.Remove(view);
        }
    }
}

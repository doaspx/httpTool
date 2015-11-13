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
using WPF_hl_17xy_cn.hl_17xy_cn;
using WPF_hl_17xy_cn.Windows;
using System.Text.RegularExpressions;
using System.Data;
using System.Windows.Threading;
using System.Threading;
using hl_17xy_cn;

namespace WPF_hl_17xy_cn
{
    /// <summary>
    /// Interaction logic for MainWindow2.xaml
    /// </summary>
    public partial class MainWindow2 : System.Windows.Window
    {
        private HttpClient _client;

        DispatcherTimer _timer;
        private int _getCount;
        private Thread _thread;
        private bool _isRunning = false;

        LoginModel _loginModel;

        public MainWindow2()
        {
            InitializeComponent();

            _client = HttpClient.BeginSession("17xy");
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _isRunning = false;
            Thread.Sleep(5000);

            base.OnClosing(e);
        }

        private void OnBtnLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow window = new LoginWindow(_client);
            if (window.ShowDialog() == true)
            {
                _loginModel = window.LoginModel;
            }
        }

        private void Run()
        {
            while (_isRunning)
            {
                btnGet_Click(null, null);
                Thread.Sleep(2000);
            }
        }

        private void btnGet_Click(object sender, RoutedEventArgs e)
        {

            string content = _client.Get("http://s"+_loginModel.SelectedServer.Index+".hl.17xy.cn/hl/i.php?a=5006", null);

            // 获取总页数
            // <span class='code'>第1/5页</span>
            int totalPage = 0;
            Regex totalPageReg = new Regex("<span class='code'>第1/(?<TotalPage>[^<>]*)页</span>");
            MatchCollection totalPageMatchs = totalPageReg.Matches(content);
            if (totalPageMatchs.Count > 0)
            {
                totalPage = Convert.ToInt32(totalPageMatchs[0].Groups["TotalPage"].Value);
            }

            List<Player> players = new List<Player>();
            for (int i = 1; i <= totalPage; i++)
            {
                if (i > 1)
                {
                    content = _client.Get("http://s" + _loginModel.SelectedServer.Index + ".hl.17xy.cn/hl/i.php?a=5006&p=" + i.ToString(), null);
                }

                Regex trReg = new Regex("<tr>(?<a>(.|\n)*?)</tr>");
                MatchCollection trMatchs = trReg.Matches(content);
                for (int index = 1; index < trMatchs.Count; index++)
                {
                    string subContent = trMatchs[index].Groups["a"].Value;
                    //Regex nameReg = new Regex("<a[^<>]*aurl=\"i.php?a=2040&id_qiu_yuan=(?<Id>[\\d]*)&type=zhuan_hui\">(?<Name>.*)(\\s)*</a>");
                    Regex nameReg = new Regex("<a[^<>]*aurl=[^<>]*id_qiu_yuan=(?<Id>[^<>]*)&type[^<>]*>(?<Name>[^<>]*)</a>");

                    MatchCollection nameMatchs = nameReg.Matches(subContent);
                    if (nameMatchs.Count > 0)
                    {
                        Player player = new Player();
                        player.Id = nameMatchs[0].Groups["Id"].Value;
                        player.Name = nameMatchs[0].Groups["Name"].Value;

                        
                        // 星级
                        //<span class="icons stars super3" tmes="明星球员">星级</span>
                        Regex starReg = new Regex("<span class=\"icons stars super(?<Star>[^<>]*)\" tmes=\"[^<>]*\">星级</span>");
                        MatchCollection starMatchs = starReg.Matches(subContent);
                        if (starMatchs.Count > 0)
                        {
                            player.Star = starMatchs[0].Groups["Star"].Value;
                        }

                        /*<td>PF/C</td>
                          <td>34</td>
                          <td>3717</td>
                          <td>4赛季</td>*/
                        Regex otherReg = new Regex("<td>(?<Pos>[^<>]*)</td>\\s*<td>(?<Age>[^<>]*)</td>\\s*<td>(?<Point>[^<>]*)</td>\\s*<td>(?<Season>[^<>]*)赛季</td>");
                        MatchCollection otherMatchs = otherReg.Matches(subContent);
                        if (otherMatchs.Count > 0)
                        {
                            player.Pos = otherMatchs[0].Groups["Pos"].Value;
                            player.Age = otherMatchs[0].Groups["Age"].Value;
                            player.Point = otherMatchs[0].Groups["Point"].Value;
                            player.Season = otherMatchs[0].Groups["Season"].Value;
                        }

                        /*
                         <td><span tmes="10,080,000">1008万</span></td>
                         <td><span tmes="出价人：麓之风<br>当前价：10,080,000"  class="color-blue">1008万</span></td>
                         */
                        Regex priceReg = new Regex("<td><span tmes=[^<>]*>(?<Price>[^<>]*)万</span></td>\\s*<td><span tmes");
                        MatchCollection priceMatchs = priceReg.Matches(subContent);
                        if (priceMatchs.Count > 0)
                        {
                            player.AuctionPrice = priceMatchs[0].Groups["Price"].Value;
                        }

                        // aurl="i.php?a=5426&id=388243" ><span>出价</span></a>
                        Regex AuctionIdReg = new Regex("aurl=\"i.php[^<>]*a=5426[^<>]*id=(?<AuctionId>\\d*)\"\\s*><span>出价</span></a>");
                        MatchCollection AuctionIdMatchs = AuctionIdReg.Matches(subContent);
                        if (AuctionIdMatchs.Count > 0)
                        {
                            player.AuctionId = AuctionIdMatchs[0].Groups["AuctionId"].Value;
                        }

                        players.Add(player);

                        if (!string.IsNullOrEmpty(player.Star)
                            && player.Star != "3" && !string.IsNullOrEmpty(player.AuctionId))
                        {
                            AutoAuction(player);
                        }
                    }
                }
            }

            //Regex nameReg = new Regex("<input type='hidden' name='a'[^<>\"]*value='(?<a>[^<>\"]*)'>");
            //MatchCollection nameMatchs = nameReg.Matches(content);
            //if (nameMatchs.Count > 0)
            //{
            //    p2.Add("a", nameMatchs[0].Groups["a"].Value);
            //}
            _getCount++;
            this.Dispatcher.Invoke(new UID2(Upui), null, true, string.Empty);
            this.Dispatcher.Invoke(new UID3(OnUID3), players);
        }

        private void OnUID3(List<Player> players)
        {
            this._ctlGridPlayers.ItemsSource = players;
        }

        private void Upui(Player player, bool isSuccess, string message)
        {
            if (player != null)
            {
                if (isSuccess)
                {
                    _ctlEditLog.Text += string.Format("购买球员:{0}成功 - 花费:{1}万!{2}", player.Name, player.AuctionPrice, Environment.NewLine);
                }
                else
                {
                    _ctlEditLog.Text += string.Format("购买球员:{0}失败!{1}", player.Name, Environment.NewLine);
                    _ctlEditLog.Text += string.Format("error:{0}{1}", message, Environment.NewLine);
                }
            }
            _ctlTextCount.Text = string.Format("已经查看了{0}次", _getCount);
        }

        private void Upui2(Player player, bool isSuccess, string message)
        {
            _ctlEditLog.Text += string.Format("开始购买球员:{0} - 编号:{1} - 星级:{2}.{3}", player.Name, player.AuctionId, player.Star, Environment.NewLine);
        }

        private void AutoAuction(Player player)
        {
            this.Dispatcher.Invoke(new UID2(Upui2), player, true, string.Empty);
            
            Dictionary<string, string> p = new Dictionary<string, string>();
            p.Add("a", "5401");
            p.Add("id", player.AuctionId);
            p.Add("jin_bi_shu", player.AuctionPrice);
            string content = _client.Get("http://s" + _loginModel.SelectedServer.Index + ".hl.17xy.cn/hl/i.php", p, "http://s" + _loginModel.SelectedServer.Index + ".hl.17xy.cn/hl/i.php?a=2000");
            if (content.Contains("出价成功"))
            {
                this.Dispatcher.Invoke(new UID2(Upui), player, true, content);
                //this.Activate();
            }
            else
            {
                this.Dispatcher.Invoke(new UID2(Upui), player, false, content);
                //this.Activate();
            }
        }

        private void btnAutoAuction_Click(object sender, RoutedEventArgs e)
        {
            if (_thread == null)
            {
                _thread = new Thread(Run);
                _isRunning = true;
                _thread.Start();
            }
            else
            {
                _isRunning = false;
                Thread.Sleep(2000);
                _thread = null;
            }
        }
    }
    public delegate void UID2(Player player, bool isSuccess, string message);
    public delegate void UID3(List<Player> players);
}

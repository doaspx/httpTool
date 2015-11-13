using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WPF_hl_17xy_cn;
using System.Text.RegularExpressions;
using System.Threading;

namespace WDM_Chou
{
    public partial class MainForm : Form
    {
        private HttpClient client;
        private string crumb;

        int number = 0;

        Thread _thread;

        bool _isRuning = true;
        bool _isWorking = false;
        int _count = 0;

        public MainForm()
        {
            InitializeComponent();
            
            client = HttpClient.BeginSession("wdm");


            _thread = new Thread(Run);
            _thread.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _isRuning = false;
            Thread.Sleep(2000);

            base.OnClosing(e);
        }

        private void _btnChou_Click(object sender, EventArgs e)
        {
            if (_isWorking)
            {
                _ctlEditPhoneNumber.ReadOnly = false;
                _btnChou.Text = "已停止";
                _isWorking = false;
                return;
            }

            if (string.IsNullOrEmpty(_ctlEditPhoneNumber.Text.Trim()))
            {
                return;
            }
            foreach (Control ctl in panel1.Controls)
            {
                if (ctl is RadioButton)
                {
                    if ((ctl as RadioButton).Checked)
                    {
                        number = Convert.ToInt32((ctl as RadioButton).Tag);
                        break;
                    }
                }
            }

            if (number == 0)
            {
                return;
            }

            _count = 0;
            _ctlEditPhoneNumber.ReadOnly = true;
            _btnChou.Text = "进行中";
            _isWorking = true;
        }

        private void Run()
        {
            while (_isRuning)
            {
                if (_isWorking)
                {
                    _count++;
                    string getContent = client.Get("http://www.wdmcake.cn/mini/chou.html");
                    if (!string.IsNullOrEmpty(getContent))
                    {
                        //<input type="hidden" name="crumb" value="e9b0b670d0">
                        string stsContent = client.Post("http://www.wdmcake.cn/mini/sts.php", "http://www.wdmcake.cn/mini/chou.html");
                        Regex crumbReg = new Regex("<input type=\"hidden\" name=\"crumb\" value=\"(?<crumb>[^<>\"]*)");

                        MatchCollection crumbMatchs = crumbReg.Matches(stsContent);
                        if (crumbMatchs.Count > 0)
                        {
                            crumb = crumbMatchs[0].Groups["crumb"].Value;
                        }
                        else
                        {
                            crumb = string.Empty;
                            Regex msgReg = new Regex("<div class=\"tskw\">(?<msg>[^<>\"]*)");
                            string message = string.Empty;
                            MatchCollection msgMatchs = msgReg.Matches(stsContent);
                            if (msgMatchs.Count > 0)
                            {
                                message = msgMatchs[0].Groups["msg"].Value;
                            }
                            this.Invoke(new Action<string>(U2), message);
                        }
                        this.Invoke(new Action<string, string>(U3), "crumb", crumb);

                        if (!string.IsNullOrEmpty(crumb))
                        {
                            Dictionary<string, string> parameters = new Dictionary<string, string>();
                            parameters.Add("mobile", _ctlEditPhoneNumber.Text.Trim());
                            parameters.Add("batch", number.ToString());
                            parameters.Add("crumb", crumb);
                            string content = client.Post("http://www.wdmcake.cn/mini/send.php", parameters, "http://www.wdmcake.cn/mini/chou.html");

                            //<div class="tskw">很遗憾，此奖项今天的名额已满
                            Regex msgReg = new Regex("<div class=\"tskw\">(?<msg>[^<>\"]*)");
                            string message = string.Empty;
                            MatchCollection msgMatchs = msgReg.Matches(content);
                            if (msgMatchs.Count > 0)
                            {
                                message = msgMatchs[0].Groups["msg"].Value;
                            }
                            //MessageBox.Show(message);
                            this.Invoke(new Action<string, string>(U), message, crumb);
                        }
                    }
                    else
                    {
                        this.Invoke(new Action<string>(U2), "服务器出错!");
                    }
                }
                else
                {
                    this.Invoke(new Action<string>(U2), "已停止!");
                }

                Thread.Sleep(1000);
            }
        }

        private void U3(string message, string crumb)
        {
            _ctlTextCrumb.Text = crumb;
            _ctlEditLog.AppendText(message + ","+crumb);
            _ctlEditLog.AppendText(Environment.NewLine);
        }

        private void U(string message, string crumb)
        {
            _ctlTextCrumb.Text = crumb;
            _ctlEditLog.AppendText(message);
            _ctlEditLog.AppendText(Environment.NewLine);
            _ctlStatusCount.Text = string.Format("已尝试{0}次", _count);

            _ctlStatusMobile.Text = _ctlEditPhoneNumber.Text.Trim();
            _ctlStatusBatch.Text = number.ToString();

            _ctlEditPhoneNumber.ReadOnly = false;
            _btnChou.Text = "已停止";
            _isWorking = false;
            MessageBox.Show(message);
        }

        private void U2(string message)
        {
            _ctlStatusCount.Text = string.Format("已尝试{0}次", _count);
            _ctlStatus.Text = message;

            _ctlStatusMobile.Text = _ctlEditPhoneNumber.Text.Trim();
            _ctlStatusBatch.Text = number.ToString();
        }
    }
}

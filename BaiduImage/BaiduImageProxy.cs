using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using WPF_hl_17xy_cn;
using System.Xml;
using System.Windows.Media.Imaging;

namespace BaiduImage
{
    public class BaiduImageProxy
    {
        private HttpClient _client;

        public BaiduImageProxy(HttpClient client)
        {
            _client = client;
        }

        public byte[] GetImage(string url)
        {
            return _client.GetBinary(url);
        }

        public List<Pic> Query(string work)
        {
            List<Pic> pics = new List<Pic>();

            string content = _client.Get(
                "http://image.baidu.com/i?tn=baiduimagejson&ct=201326592&cl=2&lm=-1&st=-1&fm=&fr=&sf=1&fmq=1351233874959_R&pv=&ic=0&nc=1&z=&se=1&showtab=0&fb=0&width=&height=&face=0&istype=2&word="
                + Uri.EscapeUriString(work)
                + "&pn=60&rn=360&805213543297.2085&268233121795.04297");

            content = content.Replace("&nbsp;", string.Empty);

            Regex reg = new Regex("\"objURL\":\"(?<objurl>[^\"]*)\"");
            MatchCollection matchs = reg.Matches(content);

            for (int i = 0; i < matchs.Count; i++)
            {
                Pic p = new Pic();
                p.Name = i.ToString();
                p.Uri = matchs[i].Groups[1].Value;

                p.Source = new BitmapImage(new Uri(p.Uri));

                pics.Add(p);
            }
            return pics;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using WPF_hl_17xy_cn;

namespace hl_17xy_cn
{
    public class HLProxy
    {
        private HttpClient _client;

        public HLProxy(HttpClient client)
        {
            _client = client;
        }

        public LoginModel GetLoginModel()
        {
            string content = _client.Get("http://hl.17xy.cn/");

            LoginModel model = new LoginModel();

            //<div class="codexy"><img src="image.php?content=59233" alt="" width="40" height="21" /></div>
            //<div class="codexy"><img src="image.php?content=72878" alt="" width="40" height="21" />
            Regex codeReg = new Regex("src=\"image.php.{1}content=(?<verify>\\d+)");
            MatchCollection codeMatchs = codeReg.Matches(content);
            if (codeMatchs.Count > 0)
            {
                model.VerifyNumber = codeMatchs[0].Groups["verify"].Value;
            }
            /*
               <input name="url" type="hidden" value="/index.php?" />
		       <input type="hidden" name="gid" value="14" />
		       <input type="hidden" name="act" value="signin" />
             */
            Regex urlReg = new Regex("<input[^<>]*name=\"url\"[^<>]*value=\"(?<url>[^<>\"]*)\"[^<>]*/>");
            MatchCollection urlMatchs = urlReg.Matches(content);
            if (urlMatchs.Count > 0)
            {
                model.Url = urlMatchs[0].Groups["url"].Value;
            }
            Regex gidReg = new Regex("<input[^<>]*name=\"gid\"[^<>]*value=\"(?<gid>[^<>\"]*)\"[^<>]*/>");
            MatchCollection gidMatchs = gidReg.Matches(content);
            if (gidMatchs.Count > 0)
            {
                model.Gid = gidMatchs[0].Groups["gid"].Value;
            }
            Regex actReg = new Regex("<input[^<>]*name=\"act\"[^<>]*value=\"(?<act>[^<>\"]*)\"[^<>]*/>");
            MatchCollection actMatchs = actReg.Matches(content);
            if (actMatchs.Count > 0)
            {
                model.Act = actMatchs[0].Groups["act"].Value;
            }


            Regex svrRangeReg = new Regex("<div class=\"boxWrap\">\\s*<div class=\"boxs\">\\s*<div class=\"server_box\">((.|\n)*?)</div>\\s*<div class=\"boxs hide\">");
            MatchCollection svrRangeMatchs = svrRangeReg.Matches(content);
            if(svrRangeMatchs.Count>0)
            {
                string subContent = svrRangeMatchs[0].Groups[0].Value;
                Regex svrReg = new Regex("<a href=\"javascript:check_server\\((?<Id>\\d*)\\);\"><span class=\"color04\">[^<>]*</span>\\s*(?<Name>[^<>]*?)<em>[^<>]*</em></a></div>");

                MatchCollection svrMatchs = svrReg.Matches(subContent);
                for (int index = 0; index < svrMatchs.Count; index++)
                {
                    ServerModel serverModel = new ServerModel();
                    serverModel.Id = svrMatchs[index].Groups["Id"].Value;
                    serverModel.Name = svrMatchs[index].Groups["Name"].Value;
                    serverModel.Index = index.ToString();
                    model.Servers.Add(serverModel);
                }
            }
            return model;
        }

        public byte[] GetVerifyImage(string verifyNumber)
        {
            return _client.GetBinary("http://hl.17xy.cn/image.php?content=" + verifyNumber);
        }
        
        public string Login(LoginModel model)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["username"] = model.UserName;
            parameters["password"] = model.Password;
            parameters["url"] = model.Url;
            parameters["gid"] = model.Gid;
            parameters["act"] = model.Act;
            parameters["verify"] = model.Verify;
            string content = _client.Post("http://hl.17xy.cn/login_check.php", parameters, "http://hl.17xy.cn");
            //content = _client.Post("http://hl.17xy.cn/svr.php", p, "http://hl.17xy.cn");
            content = _client.Get("http://hl.17xy.cn/go.php?sid=" + model.SelectedServer.Id, null, "http://hl.17xy.cn");


            Dictionary<string, string> p2 = new Dictionary<string, string>();
            string url = string.Empty;
            Regex urlReg = new Regex("<form name='entrance' action='(?<url>[^<>\"]*)' method='get'>");
            MatchCollection urlMatchs = urlReg.Matches(content);
            if (urlMatchs.Count > 0)
            {
                url = urlMatchs[0].Groups["url"].Value;
            }

            Regex aReg = new Regex("<input type='hidden' name='a'[^<>\"]*value='(?<a>[^<>\"]*)'>");
            MatchCollection aMatchs = aReg.Matches(content);
            if (aMatchs.Count > 0)
            {
                p2.Add("a", aMatchs[0].Groups["a"].Value);
            }
            Regex UserIDReg = new Regex("<input type='hidden' name='UserID'[^<>\"]*value='(?<UserID>[^<>\"]*)'>");
            MatchCollection UserIDMatchs = UserIDReg.Matches(content);
            if (UserIDMatchs.Count > 0)
            {
                p2.Add("UserID", UserIDMatchs[0].Groups["UserID"].Value);
            }
            Regex SubTimeReg = new Regex("<input type='hidden' name='SubTime'[^<>\"]*value='(?<SubTime>[^<>\"]*)'>");
            MatchCollection SubTimeMatchs = SubTimeReg.Matches(content);
            if (SubTimeMatchs.Count > 0)
            {
                p2.Add("SubTime", SubTimeMatchs[0].Groups["SubTime"].Value);
            }
            Regex ServerIDReg = new Regex("<input type='hidden' name='ServerID'[^<>\"]*value='(?<ServerID>[^<>\"]*)'>");
            MatchCollection ServerIDMatchs = ServerIDReg.Matches(content);
            if (ServerIDMatchs.Count > 0)
            {
                p2.Add("ServerID", ServerIDMatchs[0].Groups["ServerID"].Value);
            }
            Regex chkReg = new Regex("<input type='hidden' name='chk'[^<>\"]*value='(?<chk>[^<>\"]*)'>");
            MatchCollection chkMatchs = chkReg.Matches(content);
            if (chkMatchs.Count > 0)
            {
                p2.Add("chk", chkMatchs[0].Groups["chk"].Value);
            }
            content = _client.Get(url, p2, "http://hl.17xy.cn");


            //id_jiao_se :  39815
            Regex userIDReg = new Regex("id_jiao_se\\s*:\\s*(?<UserId>\\d*)");
            MatchCollection userIDMatchs = userIDReg.Matches(content);
            if (userIDMatchs.Count > 0)
            {
                return userIDMatchs[0].Groups["UserId"].Value;
            }
            return string.Empty;
        }
    }
}

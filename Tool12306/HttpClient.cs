/*
 * http client 类
 * 更新时间2012年10月23日
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace WPF_hl_17xy_cn
{
    public class HttpClient
    {
        private static Dictionary<string, HttpClient> s_sessions = new Dictionary<string,HttpClient>();

        public static HttpClient BeginSession(string sessionID)
        {
            if (s_sessions.ContainsKey(sessionID))
            {
                return s_sessions[sessionID];
            }

            HttpClient httpClient = new HttpClient();
            httpClient._sessionID = sessionID;
            s_sessions.Add(sessionID, httpClient);
            return httpClient;
        }

        private string _sessionID;
        private CookieContainer _cookieContainer = new CookieContainer();

        public string SessionID
        {
            get { return _sessionID; }
        }

        private HttpClient()
        {
        }

        public string Post(string url, Dictionary<string, string> postData)
        {
            StringBuilder requestUrl = new StringBuilder(url);

            StringBuilder postDataStringBuilder = new StringBuilder();
            foreach (string key in postData.Keys)
            {
                postDataStringBuilder.AppendFormat("{0}={1}&", key, postData[key]);
            }
            string postDataString = postDataStringBuilder.ToString();

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUrl.ToString());

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] b = encoding.GetBytes(postDataString);
            request.UserAgent = "Mozilla/4.0";
            request.Method = "POST";
            request.CookieContainer = _cookieContainer;
            request.ContentLength = b.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(b, 0, b.Length);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string content = string.Empty;
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                content = streamReader.ReadToEnd();
                _cookieContainer.Add(response.Cookies);
            }
            return content;
        }

        public string Post(string url, Dictionary<string, string> postData, string referer)
        {
            StringBuilder requestUrl = new StringBuilder(url);

            StringBuilder postDataStringBuilder = new StringBuilder();
            foreach (string key in postData.Keys)
            {
                string encodeKeyword = WebUtility.HtmlEncode(postData[key]);
                postDataStringBuilder.AppendFormat("{0}={1}&", key, postData[key]);
            }
            string postDataString = postDataStringBuilder.ToString();

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUrl.ToString());

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] b = encoding.GetBytes(postDataString);
            request.UserAgent = "Mozilla/4.0";
            request.Method = "POST";
            request.CookieContainer = _cookieContainer;
            request.ContentLength = b.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Referer = referer;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(b, 0, b.Length);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string content = string.Empty;
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                content = streamReader.ReadToEnd();
                _cookieContainer.Add(response.Cookies);
            }
            return content;
        }

        public string Post(string url, string referer)
        {
            StringBuilder requestUrl = new StringBuilder(url);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUrl.ToString());

            ASCIIEncoding encoding = new ASCIIEncoding();
            request.UserAgent = "Mozilla/4.0";
            request.Method = "POST";
            request.CookieContainer = _cookieContainer;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = 0;
            request.Referer = referer;
            string content = string.Empty;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    content = streamReader.ReadToEnd();
                    _cookieContainer.Add(response.Cookies);
                }
            }
            catch
            {
                
            }
            return content;
        }

        public string Post(string url, Dictionary<string, string> queryString, Dictionary<string,string> postData, string referer)
        {
            StringBuilder requestUrl = new StringBuilder(url);

            if (queryString != null)
            {
                requestUrl.Append("?");
                foreach (string key in queryString.Keys)
                {
                    requestUrl.AppendFormat("{0}={1}&", key, queryString[key]);
                }
            }

            StringBuilder postDataStringBuilder = new StringBuilder();
            foreach (string key in postData.Keys)
            {
                postDataStringBuilder.AppendFormat("{0}={1}&", key,  postData[key]);
            }
            string postDataString = postDataStringBuilder.ToString();

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUrl.ToString());

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] b = encoding.GetBytes(postDataString);
            request.UserAgent = "Mozilla/4.0";
            request.Method = "POST";
            request.CookieContainer = _cookieContainer;
            request.ContentLength = b.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Referer = referer;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(b, 0, b.Length);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string content = string.Empty;
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                content = streamReader.ReadToEnd();
                _cookieContainer.Add(response.Cookies);
            }
            return content;
        }

        public string Get(string url)
        {
            return Get(url, null, string.Empty);
        }

        public string Get(string url, string referer)
        {
            return Get(url, null, referer);
        }

        public string Get(string url, Dictionary<string, string> queryString)
        {
            StringBuilder requestUrl = new StringBuilder(url);
            if (queryString != null)
            {
                requestUrl.Append("?");
                foreach (string key in queryString.Keys)
                {
                    requestUrl.AppendFormat("{0}={1}&", key, queryString[key]);
                }
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUrl.ToString());
            request.UserAgent = "Mozilla/4.0";
            request.Method = "GET";
            request.CookieContainer = _cookieContainer;
            request.ContentType = "application/x-www-form-urlencoded";

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string content = string.Empty;
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    content = streamReader.ReadToEnd();
                    _cookieContainer.Add(response.Cookies);
                }
                return content;
            }
            catch
            {
                
                return string.Empty;
            }
        }

        public string Get2(string url, Dictionary<string, string> queryString, string referer)
        {
            StringBuilder requestUrl = new StringBuilder(url);
            if (queryString != null)
            {
                requestUrl.Append("?");
                bool isFirst = true;
                foreach (string key in queryString.Keys)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                        requestUrl.AppendFormat("{0}={1}", key, queryString[key]);
                    }
                    else
                    {
                        requestUrl.AppendFormat("&{0}={1}", key, queryString[key]);
                    }
                }
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUrl.ToString());

            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.3; .NET4.0C; .NET4.0E)";
            request.Method = "GET";
            request.Headers["x-requested-with"] = "XMLHttpRequest";
            request.Headers[HttpRequestHeader.AcceptLanguage] = "zh-cn";
            request.Accept = "text/plain, */*";
            request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.Connection = "Keep-Alive";
            
            request.CookieContainer = _cookieContainer;
            request.Referer = referer;

            //ASCIIEncoding encoding = new ASCIIEncoding();
            //byte[] b = encoding.GetBytes(requestUrl.ToString());
            //using (Stream stream = request.GetRequestStream())
            //{
            //    stream.Write(b, 0, b.Length);
            //}

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string content = string.Empty;
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                content = streamReader.ReadToEnd();
                _cookieContainer.Add(response.Cookies);
            }
            return content;
        }

        public string Get(string url, Dictionary<string, string> queryString, string referer)
        {
            StringBuilder requestUrl = new StringBuilder(url);
            if (queryString != null)
            {
                requestUrl.Append("?");
                foreach (string key in queryString.Keys)
                {
                    requestUrl.AppendFormat("{0}={1}&", key, queryString[key]);
                }
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUrl.ToString());
            request.UserAgent = "Mozilla/4.0";
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";

            request.CookieContainer = _cookieContainer;
            request.Referer = referer;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string content = string.Empty;
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                content = streamReader.ReadToEnd();
                _cookieContainer.Add(response.Cookies);
            }
            return content;
        }

        public byte[] GetBinary(string url)
        {
            return GetBinary(url, null);
        }

        public byte[] GetBinary(string url, Dictionary<string, string> queryString)
        {
            StringBuilder requestUrl = new StringBuilder(url);
            if (queryString != null)
            {
                requestUrl.Append("?");
                foreach (string key in queryString.Keys)
                {
                    requestUrl.AppendFormat("{0}={1}&", key, queryString[key]);
                }
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUrl.ToString());
            request.UserAgent = "Mozilla/4.0";
            request.Method = "GET";
            request.CookieContainer = _cookieContainer;
            request.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            byte[] data = new byte[1024 * 1024 * 4];
            byte[] returnData = null;
            using (BinaryReader streamReader = new BinaryReader(response.GetResponseStream()))
            {
                int count = streamReader.Read(data, 0, data.Length);
                returnData = new byte[count];
                Array.Copy(data, returnData, count);
                _cookieContainer.Add(response.Cookies);
            }
            return returnData;
        }

        public byte[] GetSslBinary(string url)
        {
            return GetSslBinary(url, null);
        }

        public byte[] GetSslBinary(string url, Dictionary<string, string> queryString)
        {
            StringBuilder requestUrl = new StringBuilder(url);
            if (queryString != null)
            {
                requestUrl.Append("?");
                foreach (string key in queryString.Keys)
                {
                    requestUrl.AppendFormat("{0}={1}&", key, queryString[key]);
                }
            }
            //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
            // 这一句一定要写在创建连接的前面。使用回调的方法进行证书验证。
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUrl.ToString());
            using (Stream stream = this.GetType().Assembly.GetManifestResourceStream("Tool12306.srca.cer"))
            {
                byte[] cerdata = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(cerdata, 0, cerdata.Length);
                //创建证书文件
                //X509Certificate objx509 = X509Certificate.CreateFromCertFile(AppDomain.CurrentDomain.BaseDirectory + "./12306.cer");
                X509Certificate objx509 = new X509Certificate(cerdata);
                //添加到请求里
                request.ClientCertificates.Add(objx509);
            }


            request.ProtocolVersion = HttpVersion.Version10;
            request.UserAgent = "Mozilla/4.0";
            request.Method = "GET";
            request.CookieContainer = _cookieContainer;
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = 0;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            byte[] data = new byte[1024 * 1024 * 4];
            byte[] returnData = null;
            using (BinaryReader streamReader = new BinaryReader(response.GetResponseStream()))
            {
                int count = streamReader.Read(data, 0, data.Length);
                returnData = new byte[count];
                Array.Copy(data, returnData, count);
                _cookieContainer.Add(response.Cookies);
            }
            return returnData;
        }

        public void Store()
        {
            StringBuilder sbc = new StringBuilder();
            List<Cookie> cooklist = GetAllCookies(this._cookieContainer);
            foreach (Cookie cookie in cooklist)
            {
                sbc.AppendFormat(
                    "{0};{1};{2};{3};{4};{5};{6};{7}\r\n",
                    cookie.Domain,
                    cookie.Name,
                    cookie.Path,
                    cookie.Port,
                    cookie.Secure.ToString(),
                    cookie.Value,
                    cookie.HttpOnly,
                    cookie.Expired);
            }

            FileStream fs = File.Create(".\\" + this._sessionID + ".txt");
            fs.Close();
            File.WriteAllText(".\\" + this._sessionID + ".txt", sbc.ToString(), System.Text.Encoding.Default);

        }

        public bool Restore()
        {
            if (!File.Exists(".\\" + this._sessionID + ".txt"))
            {
                return false;
            }

            string[] cookies = File.ReadAllText(".\\" + this._sessionID + ".txt", System.Text.Encoding.Default).Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string c in cookies)
            {
                string[] cc = c.Split(";".ToCharArray());
                Cookie ck = new Cookie(); ;
                ck.Discard = false;
                ck.Domain = cc[0];
                ck.Name = cc[1];
                ck.Path = cc[2];
                ck.Port = cc[3];
                ck.Secure = bool.Parse(cc[4]);
                ck.Value = cc[5];
                ck.HttpOnly = Convert.ToBoolean(cc[6]);
                ck.Expired = Convert.ToBoolean(cc[7]);

                _cookieContainer.Add(ck);
            }
            return true;
        }

        private List<Cookie> GetAllCookies(CookieContainer cc)
        {
            List<Cookie> lstCookies = new List<Cookie>();

            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance, null, cc, new object[] { });

            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies) lstCookies.Add(c);
            }

            return lstCookies;
        }

        //回调验证证书问题
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            // 总是接受    
            return true;
        }

    }

    //internal class AcceptAllCertificatePolicy : ICertificatePolicy
    //{
    //    public AcceptAllCertificatePolicy()
    //    {
    //    }

    //    public bool CheckValidationResult(ServicePoint sPoint, X509Certificate cert, WebRequest wRequest, int certProb)
    //    {
    //        return true;
    //    }
    //}
}

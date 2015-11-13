using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

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
            return Get(url, null);
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
            request.CookieContainer = _cookieContainer;
            request.ContentType = "application/x-www-form-urlencoded";
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
    }
}

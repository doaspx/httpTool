using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace WPF_hl_17xy_cn.hl_17xy_cn
{
    public delegate void ResponseCallback(byte[] data, Encoding encoding);

    public class HttpUtil
    {
        public ResponseCallback GetCallback;
        public ResponseCallback PostCallback;

        private CookieContainer _cookieContainer = new CookieContainer();

        public void Get(string url, ResponseCallback callback)
        {
            Get(url, string.Empty, callback);
        }

        public void Get(string url, Dictionary<string, object> queryParams, ResponseCallback callback)
        {
            string queryString = string.Empty;
            foreach (string key in queryParams.Keys)
            {
                queryString += string.Format("{0}={1}&", key, queryParams[key]);
            }
            Get(url, queryString, callback);
        }

        public void Get(string url, string queryString, ResponseCallback callback)
        {
            GetCallback = callback;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url + "?" + queryString);
            request.Method = "GET";
            request.CookieContainer = _cookieContainer;
            request.BeginGetResponse(OnGetCallback, request);
        }

        private void OnGetCallback(IAsyncResult ar)
        {
            HttpWebRequest request = ar.AsyncState as HttpWebRequest;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(ar);

            _cookieContainer.Add(response.Cookies);

            Stream stream = response.GetResponseStream();

            byte[] datas = new byte[4000000];
            byte[] buffer = new byte[2000];
            int receiveCount= 0;
            int count = stream.Read(buffer, 0, 2000);
            while (count > 0)
            {
                Buffer.BlockCopy(buffer, 0, datas, receiveCount, count);
                receiveCount += count;
                count = stream.Read(buffer, 0, 2000);
            }

            byte[] finalyDatas = new byte[receiveCount + count];
            Buffer.BlockCopy(datas, 0, finalyDatas, 0, finalyDatas.Length);


            if (GetCallback != null)
            {
                if (string.IsNullOrEmpty(response.CharacterSet))
                {
                    GetCallback(finalyDatas, Encoding.UTF8);
                }
                else
                {
                    GetCallback(finalyDatas, Encoding.GetEncoding(response.CharacterSet));
                }
            }
        }

        #region Post

        public void Post(string url, Dictionary<string, object> parameters, ResponseCallback callback)
        {
            PostCallback = callback;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded"; 
            request.CookieContainer = _cookieContainer;

            string paramString = string.Empty;
            foreach (string key in parameters.Keys)
            {
                paramString += string.Format("{0}={1}&", key, parameters[key]);
            }
            Stream stream = request.GetRequestStream();
            byte[] datas = Encoding.UTF8.GetBytes(paramString);
            stream.Write(datas,0, datas.Length);
            stream.Close();

            request.BeginGetResponse(OnPostCallback, request);
        }

        private void OnPostCallback(IAsyncResult ar)
        {
            HttpWebRequest request = ar.AsyncState as HttpWebRequest;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(ar);

            _cookieContainer.Add(response.Cookies);

            Stream stream = response.GetResponseStream();

            byte[] datas = new byte[4000000];
            byte[] buffer = new byte[2000];
            int receiveCount = 0;
            int count = stream.Read(buffer, 0, 2000);
            while (count > 0)
            {
                Buffer.BlockCopy(buffer, 0, datas, receiveCount, count);
                receiveCount += count;
                count = stream.Read(buffer, 0, 2000);
            }

            byte[] finalyDatas = new byte[receiveCount + count];
            Buffer.BlockCopy(datas, 0, finalyDatas, 0, finalyDatas.Length);


            if (PostCallback != null)
            {
                PostCallback(finalyDatas, Encoding.GetEncoding(response.CharacterSet));
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RESI.Sorter
{
 
    public class HttpHelper
    {
        public HttpHelper()
        {

        }

        public Task<string> PostAsync(string postUrl, string paramData, bool hasHeaders = false)
        {
            return Task.Factory.StartNew(() =>
            {

                return Post(postUrl, paramData, hasHeaders);
            });
        }


        public Task<string> GetAsync(string getUrl)
        {
            return Task.Factory.StartNew(() =>
            {

                return Get(getUrl);
            });
        }



        public string Post(string postUrl, string paramData, bool hasHeaders = false)
        {
            HttpWebRequest webReq = null;
            HttpWebResponse response = null;
            Stream newStream = null;
            Stream resStream = null;
            StreamReader sr = null;

            try
            {
                string ret = string.Empty;

                Encoding dataEncode = System.Text.Encoding.GetEncoding("utf-8");
                //将URL编码后的字符串转化为字节
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                ServicePointManager.DefaultConnectionLimit = 512;
                ServicePointManager.Expect100Continue = false;
                webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                //webReq.ServicePoint.Expect100Continue = false;
                //webReq.ServicePoint.ConnectionLimit = 512;
                webReq.Proxy = null;
                webReq.KeepAlive = true;
                //Post请求方式
                webReq.Method = "POST";
                if (hasHeaders)
                {
                    webReq.Headers.Add(HttpRequestHeader.Authorization, "2D12CFF31185C46693C3EA4F0A8647CF");
                    SetHeaderValue(webReq.Headers, "RegisterNo", "902");
                    SetHeaderValue(webReq.Headers, "Date", DateTime.Now.ToString("yyyy-MM-dd HH:m:ss"));
                }
                // 内容类型
                webReq.ContentType = "application/json";
                webReq.Timeout = 1500;
                //设置请求的 ContentLength 
                webReq.ContentLength = byteArray.Length;
                //获得请 求流
                newStream = webReq.GetRequestStream();

                //将请求参数写入流
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                response = (HttpWebResponse)webReq.GetResponse();
                //获得响应流
                resStream = response.GetResponseStream();
                sr = new StreamReader(resStream, System.Text.Encoding.GetEncoding("utf-8"));
                ret = sr.ReadToEnd();
                return ret;
            }
            catch (Exception ex)
            {

                Log.Error("Http Err", ex);
                return "";
            }
            finally
            {

                if (newStream != null)
                {
                    newStream.Close();
                    newStream = null;
                }
                if (resStream != null)
                {
                    resStream.Close();
                    resStream = null;
                }
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (webReq != null)
                {
                    webReq.Abort();
                    webReq = null;
                }
                if (sr != null)
                {
                    sr.Close();
                    sr = null;
                }
            }

        }


        public string Get(string getUrl)
        {
            HttpWebRequest webReq = null;
            HttpWebResponse response = null;
            Stream resStream = null;
            StreamReader sr = null;

            try
            {
                string ret = string.Empty;
                webReq = WebRequest.Create(getUrl) as HttpWebRequest;
                webReq.Method = "GET";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.UserAgent = null;
                webReq.Timeout = 1550;

                response = (HttpWebResponse)webReq.GetResponse();
                //获得响应流
                resStream = response.GetResponseStream();
                sr = new StreamReader(resStream, System.Text.Encoding.GetEncoding("utf-8"));
                ret = sr.ReadToEnd();
                return ret;
            }
            catch (Exception ex)
            {

                Log.Error("Http Err", ex);
                return "";
            }
            finally
            {


                if (resStream != null)
                {
                    resStream.Close();
                    resStream = null;
                }
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (webReq != null)
                {
                    webReq.Abort();
                    webReq = null;
                }
                if (sr != null)
                {
                    sr.Close();
                    sr = null;
                }
            }

        }




        /// <summary>
        /// 添加自定义Header
        /// </summary>
        /// <param name="header"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection", BindingFlags.Instance | BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }


    }
}

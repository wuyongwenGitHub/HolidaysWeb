using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace Holidays.Common
{
    /// <summary>
    /// HTTP帮助类
    /// add by fruitchan
    /// 2016-1-13 10:06:37
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// 请求URL获取数据
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="data">需要发送的数据(如果为GET方式请填写NULL)</param>
        /// <param name="method">请求方式</param>
        /// <returns>服务器返回的数据</returns>
        public static string RequestURL(string url, string data, string method)
        {
            string result = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.ContentType = "application/x-www-form-urlencoded";

            if (!string.IsNullOrEmpty(data) && "POST" == method)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                request.ContentLength = byteArray.Length;

                // 写数据                
                Stream stream = request.GetRequestStream();
                stream.Write(byteArray, 0, byteArray.Length);
                stream.Close();
            }

            // 读数据
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            result = sr.ReadToEnd();
            sr.Close();

            return result;
        }
    }
}

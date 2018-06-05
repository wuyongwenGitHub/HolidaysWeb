using Holidays.Common;
using Holidays.Model.FormatModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Holidays.Web.Code
{
    public class WXJsApiTicket
    {
        private static string strJsApiTicket;
        private static DateTime expires_time;

        // 获取公众号全局token
        public static string GetJsApiTicket()
        {
            if (String.IsNullOrEmpty(strJsApiTicket) || expires_time == null ||  expires_time <= DateTime.Now)
            {
                string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + WXAccessToken.GetAccessToken() + "&type=jsapi";
                string jsonTicket = HttpHelper.RequestURL(url, null, "GET");
                if (!String.IsNullOrEmpty(jsonTicket))
                {
                    JsApiTicket jsTicket = JsonConvert.DeserializeObject<JsApiTicket>(jsonTicket);

                    if (jsTicket != null && !String.IsNullOrEmpty(jsTicket.ticket))
                    {
                        strJsApiTicket = jsTicket.ticket;
                        expires_time = DateTime.Now.AddSeconds(jsTicket.expires_in - 60);
                    }
                }

            }

            return strJsApiTicket;
        }
    }
}
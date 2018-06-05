using Holidays.Common;
using Holidays.Model.FormatModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Holidays.Web.Code
{
    public class WXAccessToken
    {
        private static string access_token;
        private static DateTime expires_time;

        // 获取公众号全局token
        public static string GetAccessToken()
        {
            if (String.IsNullOrEmpty(access_token) || expires_time == null || expires_time <= DateTime.Now)
            {
                //string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + WxPayAPI.WxPayConfig.APPID + "&secret=" + WxPayAPI.WxPayConfig.APPSECRET;
                string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + ConfigurationHelper.AppSetting("WeixinAppID") + "&secret=" +ConfigurationHelper.AppSetting("WeixinAppSecret");
                string jsonToken = HttpHelper.RequestURL(url, null, "GET");

                if (!String.IsNullOrEmpty(jsonToken))
                {
                    AccessToken accessToken = JsonConvert.DeserializeObject<AccessToken>(jsonToken);

                    if (accessToken != null && !String.IsNullOrEmpty(accessToken.access_token))
                    {
                        access_token = accessToken.access_token;
                        expires_time = DateTime.Now.AddSeconds(accessToken.expires_in - 60);
                    }
                }
            }

            return access_token;
        }

    }
}
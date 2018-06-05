using Holidays.Common;
using Holidays.Model.Entites;
using Holidays.Model.FormatModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Areas.Weixin.Controllers
{
    public class AuthorizeController : Controller
    {
        //
        // GET: /Weixin/Authorize/
        private const  string appid = "wxe1d3978be8a1fc0c";
        private const string appSecret = "143ac7348ffd0010789b49353bfddd5b";

        /// <summary>
        /// 跳转微信授权网页
        /// add by fruitchan
        /// 2017-2-16 09:33:29
        /// </summary>
        /// <returns>Redirect</returns>
        public ActionResult Index()
        {
            string redirectUri = HttpUtility.UrlEncode("http://www.wyqdj.com/weixin/authorize/redirecturi");

            StringBuilder strUrl = new StringBuilder();
            strUrl.Append("https://open.weixin.qq.com/connect/oauth2/authorize?appid=");
            strUrl.Append(appid);
            strUrl.Append("&redirect_uri=");
            strUrl.Append(redirectUri);
            strUrl.Append("&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect");

            return Redirect(strUrl.ToString());
        }

        /// <summary>
        /// 微信授权回调
        /// add by fruitchan
        /// 2017-2-16 09:33:02
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Redirect</returns>
        public ActionResult RedirectUri(string code)
        {
            if (!String.IsNullOrEmpty(code))
            {
                //APPID&secret=SECRET&code=CODE&grant_type=authorization_code");

                StringBuilder strUrl = new StringBuilder();
                strUrl.Append("https://api.weixin.qq.com/sns/oauth2/access_token?appid=");
                strUrl.Append(appid);
                strUrl.Append("&secret=");
                strUrl.Append(appSecret);
                strUrl.Append("&code=");
                strUrl.Append(code);
                strUrl.Append("&grant_type=authorization_code");

                string jsonAccessToken = HttpHelper.RequestURL(strUrl.ToString(), null, "GET");

                if (!String.IsNullOrEmpty(jsonAccessToken))
                {
                    AccessToken token = JsonConvert.DeserializeObject<AccessToken>(jsonAccessToken);

                    if (token != null && !String.IsNullOrEmpty(token.access_token) && !String.IsNullOrEmpty(token.openid))
                    {
                        // 通过Token获取用户信息
                        string url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + token.access_token + "&openid=" + token.openid;

                        string jsonUserInfo = HttpHelper.RequestURL(url, null, "GET");

                        if (!String.IsNullOrEmpty(jsonUserInfo))
                        {
                            WeixinUserInfo weixinUserInfo = JsonConvert.DeserializeObject<WeixinUserInfo>(jsonUserInfo);

                            if (weixinUserInfo != null && !String.IsNullOrEmpty(weixinUserInfo.unionid))
                            {
                                Session["OpenID"] = weixinUserInfo.openid;
                                OperateContext.Current.UserLogin(new UserInfoView()
                                {
                                    Gender = weixinUserInfo.sex,
                                    Img = weixinUserInfo.headimgurl,
                                    Nikename = weixinUserInfo.nickname,
                                    Username = weixinUserInfo.nickname,
                                    WeixinUnionid = weixinUserInfo.unionid
                                }, 2, true);
                            }
                            else
                            {
                                return Content("登录失败：解析微信用户信息失败！" + jsonUserInfo);
                            }
                        }
                        else
                        {
                            return Content("登录失败：获取微信用户信息失败！");
                        }
                    }
                    else
                    {
                        return Content("登录失败：解析Token失败！" + jsonAccessToken);
                    }
                }
                else
                {
                    return Content("登录失败：获取微信Token失败！");
                }

                return Redirect("/weixin/wemain");
            }

            return Content("微信授权失败，请重新授权登录！");
        }
    }
}

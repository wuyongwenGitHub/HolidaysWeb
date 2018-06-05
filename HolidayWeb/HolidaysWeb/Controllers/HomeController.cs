using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Holidays.Model.Entites;
using Holidays.Web.Code;
using System.IO;
using Holidays.Common;
using System.Text.RegularExpressions;
using System.Text;
using Newtonsoft.Json;
using Holidays.Model.FormatModel;

namespace Holidays.Web.Controllers
{
    public class HomeController : BaseController
    {
        /// <summary>
        /// 微信登录
        /// add by fruitchan
        /// 2017-1-4 20:33:56
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="code">state</param>
        /// <returns>Redirect</returns>
        public ActionResult DoWeixinLogin(string code, string state)
        {
            string appId = ConfigurationHelper.AppSetting("WeixinAppID");

            if (!String.IsNullOrEmpty(code) && !String.IsNullOrEmpty(state))
            {
                // 校验state
                if (Session["State"] == null || Session["State"].ToString() != state)
                {
                    return Content("登录失败！");
                }

                Session["State"] = null;

                // 通过code获取access_token
                string appSecret = ConfigurationHelper.AppSetting("WeixinAppSecret");

                StringBuilder strUrl = new StringBuilder();
                strUrl.Append("https://api.weixin.qq.com/sns/oauth2/access_token?appid=");
                strUrl.Append(appId);
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

                return Redirect("/Home/Index");
            }
            else
            {
                // https://open.weixin.qq.com/connect/qrconnect?appid=APPID&redirect_uri=REDIRECT_URI&response_type=code&scope=SCOPE&state=STATE#wechat_redirect

                string redirectUri = HttpUtility.UrlEncode(ConfigurationHelper.AppSetting("RedirectUri"));
                string strState = Guid.NewGuid().ToString().Replace("-", "");
                Session["State"] = strState;

                StringBuilder sbUrl = new StringBuilder();
                sbUrl.Append("https://open.weixin.qq.com/connect/qrconnect?appid=");
                sbUrl.Append(appId);
                sbUrl.Append("&redirect_uri=");
                sbUrl.Append(redirectUri);
                sbUrl.Append("&response_type=code&scope=snsapi_login");
                sbUrl.Append("&state=");
                sbUrl.Append(strState);
                sbUrl.Append("#wechat_redirect");

                return Redirect(sbUrl.ToString());
            }
        }

        /// <summary>
        /// 获取省份列表
        /// add by fruitchan
        /// 2017-1-10 19:44:51
        /// </summary>
        /// <returns>省份列表数据</returns>
        public ActionResult GetProvinceList()
        {
            IList<Region> regionList = OperateContext.Current.BLLSession.IRegionBLL.GetListBy(m => m.ParentId == 100000).ToList();
            return OperateContext.Current.RedirectAjax("ok", null, regionList, null);
        }

        /// <summary>
        /// 根据父级编号获取城市列表
        /// add by fruitchan
        /// 2017-1-10 19:46:20
        /// </summary>
        /// <param name="parentId">父级编号</param>
        /// <returns>城市列表</returns>
        public ActionResult GetCityByParentId(int parentId)
        {
            IList<Region> regionList = OperateContext.Current.BLLSession.IRegionBLL.GetListBy(m => m.ParentId == parentId).ToList();
            return OperateContext.Current.RedirectAjax("ok", null, regionList, null);
        }

        /// <summary>
        /// 根据拼音查询城市列表
        /// add by fruitchan
        /// 2017-1-14 09:54:32
        /// </summary>
        /// <param name="pinyin">拼音</param>
        /// <returns>城市列表</returns>
        public ActionResult GetCityByPinYin(string pinyin)
        {
            IList<Region> regionList = OperateContext.Current.BLLSession.IRegionBLL.GetListBy(m => m.LevelType == 2 && m.Pinyin.Substring(0, 1) == pinyin).ToList();
            return OperateContext.Current.RedirectAjax("ok", null, regionList, null);
        }

        /// <summary>
        /// 根据名称查询城市列表
        /// add by fruitchan
        /// 2017-1-14 09:55:50
        /// </summary>
        /// <param name="name">城市名称</param>
        /// <returns>城市列表</returns>
        [ValidateInput(false)]
        public ActionResult GetCityByName(string name)
        {
            IList<Region> regionList = null;

            if (!String.IsNullOrWhiteSpace(name))
            {
                regionList = OperateContext.Current.BLLSession.IRegionBLL.GetListBy(m => m.LevelType == 2 && m.ShortName.Contains(name)).ToList();
            }

            return OperateContext.Current.RedirectAjax("ok", null, regionList, null);
        }

        //
        // GET: /Home/

        /// <summary>
        /// 我要去度假网首页
        /// add by fruitchan
        /// 2016-11-22 19:01:58
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            Model.Entites.Region currentCity = OperateContext.Current.CurrentCity;

            // Banner
            ViewBag.BannerList = OperateContext.Current.BLLSession.IHomeDataBLL.GetListBy(m => m.Type == 2).OrderByDescending(m => m.ID).ToList();

            // 广告
            ViewBag.AD = OperateContext.Current.BLLSession.IHomeDataBLL.GetListBy(m => m.Type == 1).FirstOrDefault();

            // 热门景点
            ViewBag.SpotInfoList = OperateContext.Current.BLLSession.ISpotInfoBLL.GetPagedList(1, 9, m => m.IsHomeShow == true && m.CityId == currentCity.Id, m => m.Sort, true);

            // 热门度假房
            // IList<HouseInfoView> houseList = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetPagedList(1, 4, m => m.State == 0 && m.CityId == currentCity.Id, m => m.BuyNum, false);
            // 房源图片
            //if (houseList != null && houseList.Count > 0)
            //{
            //    foreach (var house in houseList)
            //    {
            //        house.HouseImgList = OperateContext.Current.BLLSession.IHouseImgBLL.GetListBy(m => m.HouseInfoID == house.ID, m => m.CreateTime, true);
            //    }
            //}

            //ViewBag.HouseList = houseList;

            //热门店铺
            ViewBag.ShopList = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.IsCheck == 1 && h.State == 1, h => h.SortBy, true);

            // 地方特产
            ViewBag.ProductList = OperateContext.Current.BLLSession.IProductInfoBLL.GetPagedList(1, 4, m => m.IsHomeShow == true && m.CityId == currentCity.Id, m => m.Sort, true);

            // 租赁车辆
            ViewBag.CarList = OperateContext.Current.BLLSession.ICarInfoBLL.GetListBy(m => m.ID > 0 && m.CityId == currentCity.Id).ToList();

            return View();
        }

        /// <summary>
        /// 验证码图片
        /// add fruitchan
        /// 2016-12-19 22:46:43
        /// </summary>
        /// <returns>验证码图片</returns>
        public ActionResult AuthCode()
        {
            string code = CreateCheckCodeImage.GenerateCheckCode();
            Session["AuthCode"] = code;
            MemoryStream ms = CreateCheckCodeImage.Production(code);
            byte[] buffurPic = ms.ToArray();
            return File(buffurPic, "image/jpeg");
        }

        /// <summary>
        /// 校验验证码
        /// add by fruitchan
        /// 2016-12-19 23:08:40
        /// </summary>
        /// <param name="phoneNo">手机号</param>
        /// <param name="authCode">验证码</param>
        /// <returns>校验结果</returns>
        [HttpPost]
        public ActionResult CheckAuthCode(string phoneNo, string authCode)
        {
            string status = "fail";
            string msg = null;

            // 校验手机号
            if (String.IsNullOrWhiteSpace(phoneNo) || !Validate.ValidatePhone(phoneNo))
            {
                msg = "手机号输入错误，请重新输入！";
            }

            if (msg == null)
            {
                if (Session["AuthCode"] != null && Session["AuthCode"].ToString().ToLower() == authCode.ToLower())
                {
                    string smscode = "";
                    Random rand = new Random();
                    for (int i = 0; i < 4; i++)
                    {
                        smscode += rand.Next(0, 10);
                    }

                    Session["SmsCode"] = new Model.FormatModel.PhoneSmsCode() { PhoneNumber = phoneNo, SmsCode = smscode };


                    UserInfoView userInfo = OperateContext.Current.BLLSession.IUserInfoViewBLL.GetListBy(m => m.PhoneAccount == phoneNo).FirstOrDefault();

                    // 判断用户是否注册
                    if (userInfo != null)
                    {
                        // 判断用户状态
                        if (userInfo.State != 0)
                        {
                            msg = "此账号被禁止登录，请联系客服！";
                        }
                        else
                        {
                            msg = AliyunSMS.SendLoginAuthSMS(phoneNo, smscode);
                        }
                    }
                    else
                    {
                        msg = AliyunSMS.SendRegisterSMS(phoneNo, smscode);
                    }

                    if (msg == "ok")
                    {
                        status = "ok";
                        msg = "发送短信成功！";
                    }

#if DEBUG
                    msg = smscode;
#endif
                }
                else
                {
                    msg = "验证码错误！";
                }
            }

            Session["AuthCode"] = null;

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 手机号方式登录
        /// add by fruitchan
        /// 2016-12-19 23:34:39
        /// </summary>
        /// <param name="phoneNo">手机号</param>
        /// <param name="smscode">短信验证码</param>
        /// <param name="isAutoLogin">是否自动登录</param>
        /// <returns>登录结果</returns>
        [HttpPost]
        public ActionResult PhoneLogin(string phoneNo, string smscode, bool isAutoLogin)
        {
            string status = "fail";
            string msg = "短信验证码错误！";

            Model.FormatModel.PhoneSmsCode phoneSmsCode = Session["SmsCode"] as Model.FormatModel.PhoneSmsCode;

            // 校验手机号
            Regex rgx = new Regex("/^0?1[3|4|5|7|8][0-9]\\d{8}$/");
            if (String.IsNullOrWhiteSpace(phoneNo) || !rgx.IsMatch(phoneNo))
            {
                msg = "手机号输入错误，请重新输入！";
            }
            //调试
            //phoneSmsCode = new PhoneSmsCode { PhoneNumber = phoneNo,SmsCode = smscode };
            if (phoneSmsCode != null && phoneSmsCode.PhoneNumber == phoneNo && phoneSmsCode.SmsCode == smscode)
            {
                bool loginResult = OperateContext.Current.UserLogin(new UserInfoView() { PhoneAccount = phoneNo }, 1, isAutoLogin);

                if (loginResult)
                {
                    Session["SmsCode"] = null;
                    status = "ok";
                    msg = "登录成功！";
                }
                else
                {
                    msg = "登录失败！";
                }
            }
            else
            {
                msg = "短信验证码错误！";
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }


        /// <summary>
        ///账号登录
        /// </summary>
        /// <param name="input1"></param>
        /// <param name="input2"></param>
        /// <returns></returns>
        public ActionResult AccountLogin(string input1, string input2)
        {
            string status = "fail";
            string msg = "用户名或密码错误！";
            try
            {
                if (string.IsNullOrWhiteSpace(input1))
                {
                    msg = "登录账号不能为空！";
                }
                if (string.IsNullOrWhiteSpace(input2))
                {
                    msg = "登录密码不能为空！";
                }
                var encryptPassword = Common.Encrypt.MD5Encrypt32(input2);
                var isSuccess = OperateContext.Current.UserLogin(new UserInfoView { LoginAccount = input1, LoginPwd = encryptPassword }, 3, true);
                if (isSuccess)
                {
                    status = "ok";
                    msg = "登录成功！";
                }
                if (input1.Equals("18108302789"))
                {
                    SysLog log = new SysLog();
                    log.CreateOn = DateTime.Now;
                    log.ErrorMsg = "18108302789登录成功！";
                    OperateContext.Current.BLLSession.ISysLogBLL.Add(log);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                SysLog log = new SysLog();
                log.CreateOn = DateTime.Now;
                log.ErrorMsg ="登录发生错误。"+ ex.StackTrace;
                OperateContext.Current.BLLSession.ISysLogBLL.Add(log);
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input1"></param>
        /// <param name="input2"></param>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoginCenter(string input1, string input2, string verifyCode)
        {
            string status = "fail";
            string msg = null;
            if (string.IsNullOrWhiteSpace(input1))
            {
                msg = "登录账号不能为空！";
            }
            if (string.IsNullOrWhiteSpace(input2))
            {
                msg = "登录密码不能为空！";
            }
            if (string.IsNullOrWhiteSpace(verifyCode))
            {
                msg = "图形验证码不能为空！";
            }
            if (msg == null)
            {
                if (Session["AuthCode"] != null && Session["AuthCode"].ToString().ToLower() == verifyCode.ToLower())
                {
                    var encryptPassword = Common.Encrypt.MD5Encrypt32(input2);
                    var isSuccess = OperateContext.Current.UserLogin(new UserInfoView { LoginAccount = input1, LoginPwd = encryptPassword }, 3, true);
                    if (isSuccess)
                    {
                        status = "ok";
                        msg = "登录成功！";
                    }
                    else
                    {
                        msg = "账号或密码错误！";
                    }
                }
                else
                {
                    msg = "验证码错误！";
                }
            }


            Session["AuthCode"] = null;
            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }
        /// <summary>
        /// 切换城市
        /// add by fruitchan
        /// 2017-1-15 13:19:01
        /// </summary>
        /// <returns>切换城市结果</returns>
        [HttpPost]
        public ActionResult SwithCityById(int? id)
        {
            string status = "fail";
            string msg = "切换城市失败！";

            if (id.HasValue)
            {
                Region region = OperateContext.Current.BLLSession.IRegionBLL.GetListBy(m => m.Id == id.Value && m.LevelType == 2).FirstOrDefault();

                if (region != null)
                {
                    OperateContext.Current.CurrentCity = region;
                    status = "ok";
                    msg = "切换城市成功！";
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 退出登录
        /// add by fruitchan
        /// 2017-1-15 11:57:21
        /// </summary>
        /// <returns>Redirect</returns>
        public ActionResult Logout()
        {
            OperateContext.Current.CelarSession();
            return Redirect("/Home/Index");
        }

        /*update ^_^ liuyu 2017/05/03*/
        public ActionResult ReadRule(int rule)
        {
            ViewBag.Rule = rule;
            switch (rule)
            {
                case 1:
                    ViewBag.Nav = "我要去度假房东规则";
                    break;
                case 2:
                    ViewBag.Nav = "我要去度假房客规则";
                    break;
                case 3:
                    ViewBag.Nav = "我要去度假房间审核规范";
                    break;
                case 4:
                    ViewBag.Nav = "关于度假";
                    break;
            }
            return View();
        }
    }
}

using Holidays.BLL;
using Holidays.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web
{
    /// <summary>
    /// 操作上下文
    /// </summary>
    public class OperateContext
    {
        private const string ADMIN_SYSUSER = "SYSAdminLoginUser";
        private const string USER_KEY = "LoginUser";
        private const string ADMIN_LOGIC_SESSION_KEY = "BLLSession";
        private const string CURRENT_CITY = "CurrentCity";

        #region 0.1 Http上下文 及 相关属性
        /// <summary>
        /// Http上下文
        /// </summary>
        HttpContext ContextHttp
        {
            get
            {
                return HttpContext.Current;
            }
        }

        HttpResponse Response
        {
            get
            {
                return ContextHttp.Response;
            }
        }

        HttpRequest Request
        {
            get
            {
                return ContextHttp.Request;
            }
        }

        System.Web.SessionState.HttpSessionState Session
        {
            get
            {
                return ContextHttp.Session;
            }
        }
        #endregion

        #region 0.2实例构造函数 初始化 业务仓储
        public OperateContext()
        {
            BLLSession = DI.SpringHelper.GetObject<IBLL.IBLLSession>("BLLSession");
            //CustomBLL= DI.SpringHelper.GetObject<IBLL.ICustomBLL>("CustomBLL");
        }
        #endregion

        #region 0.3 业务仓储 +IBLL.IBLLSession BLLSession
        /// <summary>
        /// 业务仓储
        /// </summary>
        public IBLL.IBLLSession BLLSession;
        #endregion
        #region
        public IBLL.ICustomBLL CustomBLL;
        #endregion
        #region 0.4 获取当前 操作上下文 + OperateContext Current
        /// <summary>
        /// 获取当前 操作上下文 (为每个处理浏览器请求的服务器线程 单独创建 操作上下文)
        /// </summary>
        public static OperateContext Current
        {
            get
            {
                OperateContext oContext = CallContext.GetData(typeof(OperateContext).Name) as OperateContext;
                if (oContext == null)
                {
                    oContext = new OperateContext();
                    CallContext.SetData(typeof(OperateContext).Name, oContext);
                }
                return oContext;
            }
        }
        #endregion

        //---------------------------------------------2.0 登陆权限 等系统操作--------------------

        #region 2.1 当前管理用户对象 + Model.Entites.AdminUser AdminUser
        /// <summary>
        /// 系统用户信息
        /// </summary>
        public Model.Entites.AdminUser AdminUser
        {
            get { return Session[ADMIN_SYSUSER] as Model.Entites.AdminUser; }
          set { Session[ADMIN_SYSUSER] = value; }

        }
        /// <summary>
        /// 非admin的后台用户
        /// </summary>
        public Model.Entites.User User
        {
            get { return Session[ADMIN_SYSUSER] as Model.Entites.User; }
            set { Session[ADMIN_SYSUSER] = value; }
        }

        #endregion

        #region 2.2 管理员登录方法 + bool Login(string account, string password)
        /// <summary>
        /// 管理员登录方法
        /// </summary>
        /// <param name="account">登录帐号</param>
        /// <param name="password">登录密码</param>
        public bool Login(string account, string password)
        {
            //password = Common.Encrypt.MD5Encrypt32(password);
            //Model.Entites.AdminUser user = BLLSession.IAdminUserBLL.GetListBy(m => m.LoginAccount == account && m.LoginPassword == password).FirstOrDefault();

            //if (user != null)
            //{
            //    // 登录成功 将登录信息存入Session
            //    AdminUser = user;
            //    return true;
            //}

            //return false;

            password = Common.Encrypt.MD5Encrypt32(password);
            Model.Entites.AdminUser user = BLLSession.IAdminUserBLL.GetListBy(m => m.LoginAccount == account && m.LoginPassword == password).FirstOrDefault();
            //获取非admin的其他后台用户
            Model.Entites.User otherAdminUser = BLLSession.IUserBLL.GetListBy(m => m.LoginName == account && m.Password == password).FirstOrDefault();

            if (user != null||otherAdminUser!=null)
            {
                if (user!=null)
                {
                    // 登录成功 将登录信息存入Session
                    AdminUser = user;
                }
                else
                {
                    User = otherAdminUser;
                }
                return true;
            }

            return false;
        }
        #endregion

        #region 2.3 判断当前后台用户是否登陆 +bool AdminIsLogin()
        /// <summary>
        /// 判断当前后台用户是否登陆
        /// </summary>
        /// <returns></returns>
        public bool AdminIsLogin()
        {
            return Session[ADMIN_SYSUSER] != null;
        }
        #endregion

        #region 2.4 当前登录用户对象 + Model.Entites.AdminUser AdminUser
        /// <summary>
        /// 当前登录用户对象
        /// </summary>
        public Model.Entites.UserInfoView UserInfo
        {
            get
            {
                Model.Entites.UserInfoView userInfo = Session[USER_KEY] as Model.Entites.UserInfoView;

                if (userInfo == null)
                {
                    HttpCookie cookie = OperateContext.Current.Request.Cookies[USER_KEY];
                    if (cookie != null && cookie.Value != null && !String.IsNullOrEmpty(cookie.Value))
                    {
                        userInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Model.Entites.UserInfoView>(cookie.Value);

                        // Cookie登录的用户需要校验Token
                        if (userInfo != null)
                        {
                            if (!String.IsNullOrEmpty(userInfo.PhoneAccount))
                            {
                                userInfo = OperateContext.Current.BLLSession.IUserInfoViewBLL.GetListBy(m => m.PhoneAccount == userInfo.PhoneAccount && m.UserToken == userInfo.UserToken).FirstOrDefault();
                            }
                            else
                            {
                                userInfo = OperateContext.Current.BLLSession.IUserInfoViewBLL.GetListBy(m => m.WeixinUnionid == userInfo.WeixinUnionid && m.UserToken == userInfo.UserToken).FirstOrDefault();
                            }
                        }

                        if (userInfo != null)
                        {
                            return userInfo;
                        }
                    }
                }
                else
                {
                    // 为保证数据实时性，暂时从数据库读取用户数据
                    userInfo = OperateContext.Current.BLLSession.IUserInfoViewBLL.GetListBy(m => m.ID == userInfo.ID).FirstOrDefault();
                }

                return userInfo;
            }
            set { Session[USER_KEY] = value; }
        }
        #endregion

        #region 2.5 前台用户登录方法 + bool UserLogin(string account, string password)
        /// <summary>
        /// 前台用户登录方法
        /// </summary>
        /// <param name="userInfo">登录用户信息</param>
        /// <param name="type">1、手机号 2、微信</param>
        /// <returns>登录是否成功</returns>
        public bool UserLogin(Model.Entites.UserInfoView loginUser, int type, bool isAutoLogin)
        {
            try
            {
                Model.Entites.UserInfoView userInfo = null;

                if (type == 1)
                {
                    userInfo = OperateContext.Current.BLLSession.IUserInfoViewBLL.GetListBy(m => m.PhoneAccount == loginUser.PhoneAccount).FirstOrDefault();
                }
                else if (type == 2)
                {
                    userInfo = OperateContext.Current.BLLSession.IUserInfoViewBLL.GetListBy(m => m.WeixinUnionid == loginUser.WeixinUnionid).FirstOrDefault();
                }
                else if (type == 3)
                {
                    userInfo = OperateContext.Current.BLLSession.IUserInfoViewBLL.GetListBy(h => h.LoginAccount == loginUser.LoginAccount && h.LoginPwd == loginUser.LoginPwd).FirstOrDefault();
                }

                if (userInfo == null)
                {
                    string userToken = null;
                    string phoneAccount = null;
                    string weixinAccount = null;

                    if (type == 1)
                    {
                        phoneAccount = loginUser.PhoneAccount;
                    }
                    else if (type == 2)
                    {
                        weixinAccount = loginUser.WeixinUnionid;
                    }

                    if (isAutoLogin)
                    {
                        userToken = Guid.NewGuid().ToString();
                    }

                    // 用户账户信息
                    Model.Entites.UserAccount userAccount = new Model.Entites.UserAccount()
                    {
                        PhoneAccount = phoneAccount,
                        WeixinUnionid = weixinAccount,
                        State = 0,
                        CreateTime = DateTime.Now,
                        UserToken = userToken
                    };

                    OperateContext.Current.BLLSession.IUserAccountBLL.Add(userAccount);

                    // 用户信息
                    if (loginUser.Nikename != null && loginUser.Nikename.Length > 20)
                    {
                        loginUser.Nikename = loginUser.Nikename.Substring(0, 20);
                    }
                    if (loginUser.Username != null && loginUser.Username.Length > 20)
                    {
                        loginUser.Username = loginUser.Username.Substring(0, 20);
                    }

                    string nikename = String.IsNullOrEmpty(loginUser.Nikename) ? "我要去度假用户" + userAccount.ID : loginUser.Nikename;
                    string username = String.IsNullOrEmpty(loginUser.Username) ? "我要去度假用户" + userAccount.ID : loginUser.Username;

                    OperateContext.Current.BLLSession.IUserInfoBLL.Add(new Model.Entites.UserInfo()
                    {
                        AccountID = userAccount.ID,
                        PhoneNo = phoneAccount,
                        Gender = loginUser.Gender == 1 ? 0 : loginUser.Gender == 2 ? 1 : 2,
                        UserType = 1,
                        Img = loginUser.Img,
                        CreateTime = DateTime.Now,
                        IsRealName = 0,
                        Nikename = nikename,
                        Username = username
                    });
                }

                if (type == 1)
                {
                    userInfo = OperateContext.Current.BLLSession.IUserInfoViewBLL.GetListBy(m => m.PhoneAccount == loginUser.PhoneAccount).FirstOrDefault();
                }
                else if (type == 2)
                {
                    userInfo = OperateContext.Current.BLLSession.IUserInfoViewBLL.GetListBy(m => m.WeixinUnionid == loginUser.WeixinUnionid).FirstOrDefault();
                }
                else if (type == 3)
                {
                    userInfo = OperateContext.Current.BLLSession.IUserInfoViewBLL.GetListBy(h => h.LoginAccount == loginUser.LoginAccount && h.LoginPwd == loginUser.LoginPwd).FirstOrDefault();
                }
                if (userInfo != null)
                {
                    // 登录成功 将登录信息存入Session
                    UserInfo = userInfo;

                    if (isAutoLogin)
                    {
                        HttpCookie cookie = new HttpCookie(USER_KEY);
                        cookie.Expires = DateTime.Now.AddMonths(1);
                        cookie.Value = Newtonsoft.Json.JsonConvert.SerializeObject(userInfo);

                        OperateContext.Current.Response.Cookies.Add(cookie);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Model.Entites.SysLog log = new Model.Entites.SysLog();
                log.CreateOn = DateTime.Now;
                log.ErrorMsg = ex.Message;
                OperateContext.Current.BLLSession.ISysLogBLL.Add(log);
            }


            return false;
        }
        #endregion

        #region 2.6 判断当前前台用户是否登陆 +bool UserIsLogin()
        /// <summary>
        /// 判断当前前台用户是否登陆
        /// </summary>
        /// <returns></returns>
        public bool UserIsLogin()
        {
            return UserInfo != null;
        }
        #endregion

        #region 2.7 当前城市 + Model.Entites.Region CurrentCity
        public Model.Entites.Region CurrentCity
        {
            get
            {
                Model.Entites.Region city = Session[CURRENT_CITY] as Model.Entites.Region;

                if (city == null)
                {
                    HttpCookie cookie = OperateContext.Current.Request.Cookies[CURRENT_CITY];
                    if (cookie != null && cookie.Value != null && !String.IsNullOrEmpty(cookie.Value))
                    {
                        city = Newtonsoft.Json.JsonConvert.DeserializeObject<Model.Entites.Region>(cookie.Value);
                    }
                }

                if (city == null)
                {
                    // 默认城市
                    city = OperateContext.Current.BLLSession.IRegionBLL.GetListBy(m => m.Id == 500100).FirstOrDefault();

                    city.Name = HttpUtility.UrlEncode(city.Name);
                    city.ShortName = HttpUtility.UrlEncode(city.ShortName);
                    city.MergerName = HttpUtility.UrlEncode(city.Name);

                    Session[CURRENT_CITY] = Newtonsoft.Json.JsonConvert.SerializeObject(city);
                    HttpCookie cookie = new HttpCookie(CURRENT_CITY);
                    cookie.Expires = DateTime.Now.AddYears(1);
                    cookie.Value = Newtonsoft.Json.JsonConvert.SerializeObject(city);

                    OperateContext.Current.Response.Cookies.Add(cookie);

                    city.Name = HttpUtility.UrlDecode(city.Name);
                    city.ShortName = HttpUtility.UrlDecode(city.ShortName);
                    city.MergerName = HttpUtility.UrlDecode(city.Name);
                }
                else
                {
                    city.Name = HttpUtility.UrlDecode(city.Name);
                    city.ShortName = HttpUtility.UrlDecode(city.ShortName);
                    city.MergerName = HttpUtility.UrlDecode(city.Name);
                }

                return city;
            }
            set
            {
                Session[CURRENT_CITY] = value;

                value.Name = HttpUtility.UrlEncode(value.Name);
                value.ShortName = HttpUtility.UrlEncode(value.ShortName);
                value.MergerName = HttpUtility.UrlEncode(value.Name);

                HttpCookie cookie = new HttpCookie(CURRENT_CITY);
                cookie.Expires = DateTime.Now.AddYears(1);
                cookie.Value = Newtonsoft.Json.JsonConvert.SerializeObject(value);

                OperateContext.Current.Response.Cookies.Add(cookie);
            }
        }
        #endregion

        #region 2.7 清除缓存 + CelarSession()
        /// <summary>
        /// 清除缓存
        /// </summary>
        public void CelarSession()
        {
            Session[ADMIN_SYSUSER] = null;
            Session[USER_KEY] = null;
            Session.Clear();

            HttpCookie cookie = OperateContext.Current.Request.Cookies[USER_KEY];

            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                cookie.Value = "";
                OperateContext.Current.Response.Cookies.Add(cookie);
            }
        }
        #endregion

        //---------------------------------------------3.0 公用操作方法--------------------

        #region 3.1 生成 Json 格式的返回值 +ActionResult RedirectAjax(string statu, string msg, object data, string backurl)
        /// <summary>
        /// 生成 Json 格式的返回值
        /// </summary>
        /// <param name="status"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <param name="backurl"></param>
        /// <returns></returns>
        public ActionResult RedirectAjax(string status, string msg, object data, string backurl)
        {
            Model.FormatModel.AjaxMsgModel ajax = new Model.FormatModel.AjaxMsgModel()
            {
                Status = status,
                Msg = msg,
                Data = data,
                BackUrl = backurl
            };
            JsonResult res = new JsonResult();
            res.Data = ajax;

            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return res;
        }
        #endregion

        #region 3.2 重定向方法 根据Action方法特性  +ActionResult Redirect(string url, ActionDescriptor action)
        /// <summary>
        /// 重定向方法 有两种情况：如果是Ajax请求，则返回 Json字符串；如果是普通请求，则 返回重定向命令
        /// </summary>
        /// <returns></returns>
        public ActionResult Redirect(string url, ActionDescriptor action)
        {
            //如果Ajax请求没有权限，就返回 Json消息
            if (action.IsDefined(typeof(AjaxRequestAttribute), false)
            || action.ControllerDescriptor.IsDefined(typeof(AjaxRequestAttribute), false))
            {
                return RedirectAjax("nologin", "登录超时，请重新登录！", null, url);
            }
            else//如果 超链接或表单 没有权限访问，则返回 302重定向命令
            {
                return new RedirectResult(url);
            }
        }
        #endregion
        #region
        public List<T> CustomSql<T>(int ShopId, string date)
        {
            CustomBLL = new CustomBLL();
            return CustomBLL.GetTodayPrice<T>(ShopId, date); 
        }

        #endregion
    }
}
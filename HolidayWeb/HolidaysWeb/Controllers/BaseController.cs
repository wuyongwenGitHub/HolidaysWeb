using Holidays.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/

        public BaseController()
        {
            // 用户登录状态
            ViewBag.IsLogin = OperateContext.Current.UserIsLogin();
            ViewBag.UserInfo = OperateContext.Current.UserInfo;
            ViewBag.CurrentCity = OperateContext.Current.CurrentCity;
            ViewBag.UserType = OperateContext.Current.UserInfo != null ? OperateContext.Current.UserInfo.UserType : 2;
        }
    }
}

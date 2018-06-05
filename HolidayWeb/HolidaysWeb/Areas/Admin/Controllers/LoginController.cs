using Holidays.Common;
using Holidays.Model.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Admin/Login/

        /// <summary>
        /// 后台管理登录页面
        /// add by fruitchan
        /// 2016-11-22 19:02:31
        /// </summary>
        /// <returns>View</returns>
        [Common.Attributes.Skip]
        public ActionResult Index(string backUrl)
        {
            ViewBag.BackUrl = backUrl;
            return View();
        }

        /// <summary>
        /// 后台管理员登录
        /// add by fruitchan
        /// 2016-12-11 18:36:49
        /// </summary>
        /// <param name="loginAccount">登录账号</param>
        /// <param name="password">登录密码</param>
        /// <returns>登录是否成功</returns>
        [HttpPost]
        [Common.Attributes.Skip]
        public ActionResult AdminLogin(string loginAccount, string loginPassword)
        {
            string status = "-1";
            string msg = null;

            msg = Validate.ValidateString(new CustomValidate()
            {
                FieldName = "登录账号",
                FieldValue = loginAccount,
                IsRequired = true,
                MaxLength = 20
            },
            new CustomValidate()
            {
                FieldName = "登录密码",
                FieldValue = loginPassword,
                IsRequired = true,
                MinLength = 6
            });

            if (msg == null)
            {
                if (OperateContext.Current.Login(loginAccount, loginPassword))
                {
                    status = "ok";
                    msg = "/Admin/Main";
                }
                else
                {
                    msg = "登录账号或登录密码错误！";
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 退出系统业务处理，注销后重定向到后台登录页面
        /// add by fruitchan
        /// 2016-11-22 21:52:42
        /// </summary>
        /// <returns>View</returns>
        [Common.Attributes.Skip]
        public ActionResult Logout()
        {
            OperateContext.Current.CelarSession();
            return RedirectToAction("Index", "Login", new { area = "Admin" });
        }
        /// <summary>
        /// 分类获取菜单列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMenuCateList()
        {
            string status = HolidaysWebConst.HolidaysWebConst.SUCCESS;
            string msg = string.Empty;
            //获取所有菜单
            var allMenus = OperateContext.Current.BLLSession.IFunctionBLL.GetListBy(s => s.IsDeleted == false);
            List<Function> topFuncList = allMenus.Where(s => s.ParentId == 0).ToList();
            //用来装载分类的数据
            List<List<Function>> loadFuncList = new List<List<Function>>();
            //根据父级Id分类，目前只有两级
            foreach (var topFunc in topFuncList)
            {
                var secondFuncList = OperateContext.Current.BLLSession.IFunctionBLL.GetListBy(s => s.ParentId == topFunc.Id);
                if (secondFuncList.Count <= 0)
                {
                    List<Function> tmpList = new List<Function>();
                    tmpList.Add(topFunc);
                    loadFuncList.Add(tmpList);
                }
                else
                {
                    secondFuncList.Add(topFunc);
                    loadFuncList.Add(secondFuncList);
                }

            }
            ViewBag(loadFuncList);
            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }
    }
}

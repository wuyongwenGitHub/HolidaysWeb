using Holidays.Common.Attributes;
using Holidays.Model.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Areas.Admin.Controllers
{
    public class MainController : Controller
    {
        //
        // GET: /Admin/Main/

        /// <summary>
        /// 后台管理首页
        /// add by fruitchan
        /// 2016-11-22 19:03:06
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            DateTime dtToday = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

            // 房东审核待办数
            ViewBag.CertificateTotal = OperateContext.Current.BLLSession.IUserInfoCertificateBLL.CountRow(m => m.State == 0);

            // 普通会员
            ViewBag.NormalUserTotal = OperateContext.Current.BLLSession.IUserInfoBLL.CountRow(m => m.UserType == 1);
            ViewBag.NormalUserTodayAdd = OperateContext.Current.BLLSession.IUserInfoBLL.CountRow(m => m.UserType == 1 && m.CreateTime >= dtToday);
            ViewBag.NormalUserList = OperateContext.Current.BLLSession.IUserInfoBLL.GetPagedList(1, 3, m => m.UserType == 1, m => m.CreateTime, false).ToList();

            // 房东
            ViewBag.LandlordUserTotal = OperateContext.Current.BLLSession.IUserInfoBLL.CountRow(m => m.UserType == 2);
            ViewBag.LandlordUserTodayAdd = OperateContext.Current.BLLSession.IUserInfoBLL.CountRow(m => m.UserType == 2 && m.CreateTime >= dtToday);
            ViewBag.LandlordUserList = OperateContext.Current.BLLSession.IUserInfoBLL.GetPagedList(1, 3, m => m.UserType == 2, m => m.CreateTime, false).ToList();

            // 房源
            ViewBag.HouseTotal = OperateContext.Current.BLLSession.IHouseInfoViewBLL.CountRow(m => m.ID > 0);
            ViewBag.HouseTodayAdd = OperateContext.Current.BLLSession.IHouseInfoViewBLL.CountRow(m => m.CreateTime >= dtToday);
            ViewBag.HouseList = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetPagedList(1, 3, m => m.ID > 0, m => m.CreateTime, false).ToList();

            // 平台今日交易
            ViewBag.PriceTodayTotal = OperateContext.Current.BLLSession.IOrderInfoBLL.GetPriceTodayTotal();
            ViewBag.PlatformRoyaltyTodayTotal = OperateContext.Current.BLLSession.IOrderInfoBLL.GetPlatformRoyaltyTodayTotal();
            ViewBag.OrderList = OperateContext.Current.BLLSession.IOrderInfoBLL.GetPagedList(1, 3, m => m.State == 1 || m.State == 2, m => m.CreateTime, false).ToList();

            return View();
        }
        /// <summary>
        /// 分类获取菜单列表
        /// </summary>
        /// <returns></returns>
        public void GetMenuCateList()
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
            ViewBag.loadFuncList = loadFuncList;
        }
    }
}

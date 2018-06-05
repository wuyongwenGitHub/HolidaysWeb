using Holidays.Common;
using Holidays.Common.Attributes;
using Holidays.Model.Entites;
using Holidays.Model.FormatModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Areas.Admin.Controllers
{
    public class DataStatsController : Controller
    {
        //
        // GET: /Admin/DataStats/

        /// <summary>
        /// 数据统计页面
        /// add by fruitchan
        /// 2016-11-22 20:55:23
        /// </summary>
        /// <returns>View</returns>
        [ValidMenuPerm]
        public ActionResult Index()
        {
            // 房东审核待办数
            ViewBag.CertificateTotal = OperateContext.Current.BLLSession.IUserInfoCertificateBLL.CountRow(m => m.State == 0);

            DateTime dtToday = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

            // 普通会员
            ViewBag.NormalUserTotal = OperateContext.Current.BLLSession.IUserInfoBLL.CountRow(m => m.UserType == 1);
            ViewBag.NormalUserTodayAdd = OperateContext.Current.BLLSession.IUserInfoBLL.CountRow(m => m.UserType == 1 && m.CreateTime >= dtToday);

            // 房东
            ViewBag.LandlordUserTotal = OperateContext.Current.BLLSession.IUserInfoBLL.CountRow(m => m.UserType == 2);
            ViewBag.LandlordUserTodayAdd = OperateContext.Current.BLLSession.IUserInfoBLL.CountRow(m => m.UserType == 2 && m.CreateTime >= dtToday);

            // 房源
            ViewBag.HouseTotal = OperateContext.Current.BLLSession.IHouseInfoViewBLL.CountRow(m => m.ID > 0);
            ViewBag.HouseTodayAdd = OperateContext.Current.BLLSession.IHouseInfoViewBLL.CountRow(m => m.CreateTime >= dtToday);

            // 平台今日交易
            ViewBag.PriceTodayTotal = OperateContext.Current.BLLSession.IOrderInfoBLL.GetPriceTodayTotal();
            ViewBag.PlatformRoyaltyTodayTotal = OperateContext.Current.BLLSession.IOrderInfoBLL.GetPlatformRoyaltyTodayTotal();

            // 平台总交易
            ViewBag.PriceTotal = OperateContext.Current.BLLSession.IOrderInfoBLL.GetPriceTotal();
            ViewBag.PlatformRoyaltyTotal = OperateContext.Current.BLLSession.IOrderInfoBLL.GetPlatformRoyaltyTotal();

            return View();
        }

        /// <summary>
        /// 分页查询订单信息
        /// add by fruitchan
        /// 2016-12-12 21:41:19
        /// </summary>
        /// <param name="orderNo">订单编号</param>
        /// <param name="houseTitle">房源标题</param>
        /// <param name="username">会员姓名</param>
        /// <param name="startTime">交易时间（起始）</param>
        /// <param name="endTime">交易时间（结束）</param>
        /// <param name="page">当前页码</param>
        /// <param name="row">每页显示数据条数</param>
        /// <returns>订单列表数据</returns>
        [HttpPost]
        public ActionResult GetOrderList(string orderNo, string username, int? state, string landlordName, string landlordPhone, string houseTitle, DateTime? startTime, DateTime? endTime, int page = 1, int row = 10)
        {
            var whereLambda = PredicateBuilder.True<OrderInfo>();

            #region 查询条件

            if (!String.IsNullOrWhiteSpace(orderNo))
            {
                // 订单编号
                Expression<Func<OrderInfo, bool>> orderNoExpression = p => p.OrderNo.Contains(orderNo);
                whereLambda = whereLambda.And(orderNoExpression);
            }

            if (!String.IsNullOrWhiteSpace(username))
            {
                // 会员姓名
                Expression<Func<OrderInfo, bool>> usernameExpression = p => p.Username.Contains(username);
                whereLambda = whereLambda.And(usernameExpression);
            }

            if (!String.IsNullOrWhiteSpace(landlordName))
            {
                // 房东姓名
                Expression<Func<OrderInfo, bool>> landlordNameExpression = p => p.LandlordName == landlordName;
                whereLambda = whereLambda.And(landlordNameExpression);
            }

            if (!String.IsNullOrWhiteSpace(landlordPhone))
            {
                // 房东手机号
                Expression<Func<OrderInfo, bool>> landlordPhoneExpression = p => p.LandlordPhone == landlordPhone;
                whereLambda = whereLambda.And(landlordPhoneExpression);
            }

            if (!String.IsNullOrWhiteSpace(houseTitle))
            {
                // 商品名称（房源标题）
                Expression<Func<OrderInfo, bool>> houseTitleExpression = p => p.HouseTitle.Contains(houseTitle);
                whereLambda = whereLambda.And(houseTitleExpression);
            }

            if (startTime.HasValue)
            {
                // 交易时间（起始）
                Expression<Func<OrderInfo, bool>> startTimeExpression = p => p.CreateTime >= startTime.Value;
                whereLambda = whereLambda.And(startTimeExpression);
            }

            if (endTime.HasValue)
            {
                // 交易时间（结束）
                endTime = endTime.Value.AddDays(1);
                Expression<Func<OrderInfo, bool>> endTimeExpression = p => p.CreateTime <= endTime;
                whereLambda = whereLambda.And(endTimeExpression);
            }
            if (state.HasValue)
            {
                //订单状态
                Expression<Func<OrderInfo, bool>> stateExpression = h => h.State == state;
                whereLambda = whereLambda.And(stateExpression);
            }

            #endregion

            int rowCount = 0;
            IList<OrderInfo> orderList = OperateContext.Current.BLLSession.IOrderInfoBLL.GetPagedList(page, row, ref rowCount, whereLambda, m => m.ID, false);

            // 状态
          //  Expression<Func<OrderInfo, bool>> stateExpression = p => p.State == 1 || p.State == 2;
          //  whereLambda = whereLambda.And(stateExpression);
            decimal platformRoyaltyTotal = OperateContext.Current.BLLSession.IOrderInfoBLL.GetListBy(whereLambda).Sum(m => m.PlatformRoyalty);
            decimal priceTotal = OperateContext.Current.BLLSession.IOrderInfoBLL.GetListBy(whereLambda).Sum(m => m.Price);

            PageModel<OrderInfo> model = PageModel<OrderInfo>.GetPageModel(orderList, page, rowCount, row);
            StringBuilder sb = new StringBuilder();
            sb.Append("平台交易总额：￥");
            sb.Append(priceTotal);
            sb.Append("　　　　平台提成总额：￥");
            sb.Append(platformRoyaltyTotal);
            sb.Append("　　　　");

            if ((!String.IsNullOrWhiteSpace(landlordName) || !String.IsNullOrWhiteSpace(landlordPhone)) && orderList.Count > 0)
            {
                sb.Append(orderList[0].LandlordName);
            }
            else
            {
                sb.Append("房东");
            }

            sb.Append("收入总额：￥");
            sb.Append(priceTotal - platformRoyaltyTotal);

            return OperateContext.Current.RedirectAjax("ok", sb.ToString(), model, null);
        }

    }
}

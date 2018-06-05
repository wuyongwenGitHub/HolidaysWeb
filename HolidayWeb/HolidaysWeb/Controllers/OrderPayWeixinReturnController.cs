using Holidays.Model.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Controllers
{
    public class OrderPayWeixinReturnController : BaseController
    {
        //
        // GET: /OrderPayWeixinReturn/

        public ActionResult Index(string orderNo)
        {
            ViewBag.Status = "fail";
            ViewBag.Msg = "订单未支付成功！";

            if (!String.IsNullOrWhiteSpace(orderNo))
            {
                OrderInfo order = OperateContext.Current.BLLSession.IOrderInfoBLL.GetListBy(m => m.OrderNo == orderNo).FirstOrDefault();

                if (order != null && (order.State == 1 || order.State == 2))
                {
                    ViewBag.Status = "ok";
                    ViewBag.Msg = "支付成功！";
                }
            }

            return View();
        }

    }
}

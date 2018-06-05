using Com.Alipay;
using Holidays.Model.Entites;
using Holidays.Web.Code;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Controllers
{
    public class AlipayNotifyController : Controller
    {
        //
        // GET: /AlipayNotify/

        /// <summary>
        /// 支付宝通知地址
        /// add by fruitchan
        /// 2017-1-18 20:34:29
        /// </summary>
        /// <returns>Content</returns>
        public ActionResult Index()
        {
            SortedDictionary<string, string> sPara = GetRequestPost();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);

                if (verifyResult)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    //商户订单号

                    string out_trade_no = Request.Form["out_trade_no"];

                    //支付宝交易号

                    string trade_no = Request.Form["trade_no"];

                    //交易状态
                    string trade_status = Request.Form["trade_status"];


                    if (Request.Form["trade_status"] == "TRADE_FINISHED")
                    {
                        UpdateOrderState(out_trade_no, trade_no);
                    }
                    else if (Request.Form["trade_status"] == "TRADE_SUCCESS")
                    {
                        UpdateOrderState(out_trade_no, trade_no);
                    }
                    else
                    {
                        // ?
                    }

                    return Content("success");  //请不要修改或删除

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else//验证失败
                {
                    return Content("fail");
                }
            }
            else
            {
                return Content("无通知参数");
            }
        }

        /// <summary>
        /// 更新订单状态
        /// add by fruitchan
        /// 2017-1-18 20:04:20
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="otherNo">第三方订单号</param>
        /// <returns>更改订单是否成功</returns>
        private bool UpdateOrderState(string orderNo, string otherNo)
        {
            int result = 0;
            OrderItem orderItem = OperateContext.Current.BLLSession.IOrderItemBLL.GetListBy(m => m.OrderNo == orderNo).FirstOrDefault();
            OrderInfo order = OperateContext.Current.BLLSession.IOrderInfoBLL.GetListBy(m => m.ID == orderItem.OrderId).FirstOrDefault();
            var houseCode = Common.CouponHelper.Get8LCode();
            var queryObj = OperateContext.Current.BLLSession.IOrderInfoBLL.GetListBy(h => h.CouponCode.Equals(houseCode)).FirstOrDefault();
            while (queryObj != null)
            {
                houseCode = Common.CouponHelper.Get8LCode();
                queryObj = OperateContext.Current.BLLSession.IOrderInfoBLL.GetListBy(h => h.CouponCode.Equals(houseCode)).FirstOrDefault();
            }
            if (orderItem != null && orderItem.State == 0 && order != null && (order.State == 0 || order.State == 1))
            {
                // 子订单
                orderItem.State = 1;
                orderItem.OtherNo = otherNo;
                orderItem.PayTime = DateTime.Now;

                result = OperateContext.Current.BLLSession.IOrderItemBLL.Modify(orderItem, "State", "OtherNo", "PayTime");

                if (result == 1)
                {
                    // 主订单
                    order.State = orderItem.BalancePayment == 0 ? 2 : 1;
                    order.BalancePayment = orderItem.BalancePayment;
                    order.Price = order.TotalPrice - order.BalancePayment;
                    order.OtherNo = otherNo;
                    order.CouponCode = houseCode;
                    result = OperateContext.Current.BLLSession.IOrderInfoBLL.Modify(order, "State", "Price", "BalancePayment", "OtherNo", "CouponCode");
                }

                if (result == 1 && orderItem.BalancePayment == 0)
                {
                    // 更新房源购买数量   
                    HouseInfo houseInfo = OperateContext.Current.BLLSession.IHouseInfoBLL.GetListBy(m => m.ID == order.HouseInfoID).FirstOrDefault();

                    if (houseInfo != null)
                    {
                        houseInfo.BuyNum += 1;
                        OperateContext.Current.BLLSession.IHouseInfoBLL.Modify(houseInfo, "BuyNum");
                    }
                }
            }

            if (result == 1)
            {
                // 发送订单短信
                AliyunSMS.SendOrderSMS(order.LandlordPhone, order.HouseTitle, order.StartDate.ToString("yyyy-MM-dd"), order.EndDate.ToString("yyyy-MM-dd"), order.PersonNum.ToString(), order.Username, order.UserPhone);
                //发送顾客订单短信
                var houseInfo = OperateContext.Current.BLLSession.IHouseInfoBLL.GetListBy(h => h.ID == order.HouseInfoID).FirstOrDefault();
                AliyunSMS.SendOrderSMSByUser(order.UserPhone, order.Username, order.StartDate.ToString("yyyy-MM-dd"), order.EndDate.ToString("yyyy-MM-dd"), houseInfo.ShopName, order.PersonNum.ToString(), order.HouseTitle, houseCode, order.LandlordPhone);
            }

            return result == 1;
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }

    }
}

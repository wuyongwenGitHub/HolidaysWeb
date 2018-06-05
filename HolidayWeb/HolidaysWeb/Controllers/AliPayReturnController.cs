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
    public class AliPayReturnController : BaseController
    {
        //
        // GET: /AliPayReturn/

        /// <summary>
        /// 支付宝返回地址
        /// add by fruitchan
        /// 2017-1-18 20:34:52
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            string status = "fail";
            string strResult = "系统繁忙！";
            SortedDictionary<string, string> sPara = GetRequestGet();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.QueryString["notify_id"], Request.QueryString["sign"]);

                if (verifyResult)//验证成功
                {
                    //商户订单号

                    string out_trade_no = Request.QueryString["out_trade_no"];

                    //支付宝交易号

                    string trade_no = Request.QueryString["trade_no"];

                    //交易状态
                    string trade_status = Request.QueryString["trade_status"];


                    if (Request.QueryString["trade_status"] == "TRADE_FINISHED" || Request.QueryString["trade_status"] == "TRADE_SUCCESS")
                    {
                        UpdateOrderState(out_trade_no, trade_no);
                        status = "ok";
                        strResult = "处理订单成功！";
                    }
                    else
                    {
                        strResult = "trade_status=" + Request.QueryString["trade_status"];
                    }
                }
                else//验证失败
                {
                    strResult = "数据验证失败！";
                }
            }
            else
            {
                strResult = "无返回参数！";
            }

            return View(new Model.FormatModel.AjaxMsgModel()
            {
                Status = status,
                Msg = strResult
            });
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
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestGet()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }
    }
}

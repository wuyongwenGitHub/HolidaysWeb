using Holidays.Model.Entites;
using Holidays.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;

namespace Holidays.Web.Controllers
{
    public class WeixinPayBackUrlController : Controller
    {
        //
        // GET: /WeixinPayBackUrl/

        /// <summary>
        /// 微信支付回调地址
        /// add by fruitchan
        /// 2017-1-18 21:10:30
        /// </summary>
        /// <returns>Content</returns>
        public ActionResult Index()
        {
            WxPayData res = new WxPayData();
            res.SetValue("return_code", "FAIL");
            res.SetValue("return_msg", "业务异常");

            WxPayData notifyData = GetNotifyData();

            if (!notifyData.IsSet("return_code"))
            {
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "回调数据异常");
                Log.Info(this.GetType().ToString(), "The data WeChat post is error : " + res.ToXml());
                return Content(res.ToXml());
            }
            else
            {
                string return_code = notifyData.GetValue("return_code").ToString();
                string result_code = notifyData.GetValue("result_code").ToString();

                if (return_code == "SUCCESS" && result_code == "SUCCESS")
                {
                    string out_trade_no = notifyData.GetValue("out_trade_no").ToString();
                    string transaction_id = notifyData.GetValue("transaction_id").ToString();

                    // 处理订单
                    UpdateOrderState(out_trade_no, transaction_id);

                    res.SetValue("return_code", "SUCCESS");
                    res.SetValue("return_msg", "OK");

                }
            }

            return Content(res.ToXml());
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
        /// 接收从微信支付后台发送过来的数据并验证签名
        /// </summary>
        /// <returns>微信支付后台返回的数据</returns>
        private WxPayData GetNotifyData()
        {
            //接收从微信后台POST过来的数据
            System.IO.Stream s = Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

            Log.Info(this.GetType().ToString(), "Receive data from WeChat : " + builder.ToString());

            //转换数据格式并验证签名
            WxPayData data = new WxPayData();
            try
            {
                data.FromXml(builder.ToString());
            }
            catch (WxPayException ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
                Log.Error(this.GetType().ToString(), "Sign check error : " + res.ToXml());
                Response.Write(res.ToXml());
                Response.End();
            }

            Log.Info(this.GetType().ToString(), "Check sign success");
            return data;
        }

    }
}

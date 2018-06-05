using Holidays.Model.Entites;
using Holidays.Model.FormatModel;
using Holidays.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WxPayAPI;

namespace Holidays.Web.Areas.Weixin.Controllers
{
    public class WXPayController : Controller
    {
        //
        // GET: /Weixin/WXPay/

        // 微信支付授权目录 /weixin/wxpay

        public ActionResult OrderPayView(string orderNo, int? isFullPrice)
        {
            // 校验数据
            if (!String.IsNullOrEmpty(orderNo) && isFullPrice.HasValue)
            {
                if ((isFullPrice.Value != 0 && isFullPrice != 1))
                {
                    return Content("参数错误！");
                }

                bool bIsFullPrice = isFullPrice.Value == 1;

                OrderInfo order = OperateContext.Current.BLLSession.IOrderInfoBLL.GetListBy(m => m.OrderNo == orderNo).FirstOrDefault();

                if (order == null || (order.State != 0 && order.State != 1))
                {
                    return Content("未查询到订单信息或订单已完成！");
                }

                // 支付金额
                decimal price = bIsFullPrice ? order.BalancePayment : order.DownPayment;

                #region 子订单

                string strRand = "";
                Random rand = new Random();
                for (int i = 0; i < 6; i++)
                {
                    strRand += rand.Next(0, 10);
                }

                // 生成订单编号
                string newOrderNo = DateTime.Now.ToString("yyyyMMddHHmmss") + strRand;

                OrderItem orderItem = new OrderItem()
                {
                    OrderId = order.ID,
                    OrderNo = newOrderNo,
                    Price = price,
                    BalancePayment = order.BalancePayment - price,
                    State = 0,
                    CreateTime = DateTime.Now
                };

                OperateContext.Current.BLLSession.IOrderItemBLL.Add(orderItem);

                #endregion

                order.PayType = 2;
                order.IsFullPrice = bIsFullPrice;

                int result = OperateContext.Current.BLLSession.IOrderInfoBLL.Modify(order, "Price", "BalancePayment", "PayType", "IsFullPrice");

                if (result == 1)
                {
                    if (Session["OpenID"] == null)
                    {
                        return Content("获取用户信息失败，请从微信公众账号进入！");
                    }

                    try
                    {
                        // 微信支付
                        WxPayData data = new WxPayData();
                        data.SetValue("body", order.HouseTitle);//商品描述
                        data.SetValue("attach", "");//附加数据
                        data.SetValue("out_trade_no", orderItem.OrderNo);//订单号
                        data.SetValue("total_fee", Convert.ToInt32((price * 100)).ToString());//总金额
                        data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
                        data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
                        data.SetValue("goods_tag", "");//商品标记
                        data.SetValue("trade_type", "JSAPI");//交易类型
                        data.SetValue("product_id", orderNo);//商品ID
                        data.SetValue("openid", Session["OpenID"].ToString());

                        WxPayData orderResult = WxPayApi.UnifiedOrder(data);//调用统一下单接口

                        if (orderResult.GetValue("return_msg").ToString() == "OK" && orderResult.GetValue("result_code").ToString() == "SUCCESS" && orderResult.GetValue("return_code").ToString() == "SUCCESS")
                        {
                            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                            string appid = orderResult.GetValue("appid").ToString();
                            string timeStamp = Convert.ToInt64(ts.TotalSeconds).ToString();
                            string nonceStr = orderResult.GetValue("nonce_str").ToString();
                            string package = "prepay_id=" + orderResult.GetValue("prepay_id").ToString();

                            string jsApiTicket = WXJsApiTicket.GetJsApiTicket();
                            if (String.IsNullOrEmpty(jsApiTicket))
                            {
                                return Content("微信接口权限失效，请重新登录！");
                            }
                            string strTemp = "jsapi_ticket=" + jsApiTicket + "&noncestr=" + nonceStr + "&timestamp=" + timeStamp + "&url=" + Request.Url.ToString();
                            ViewBag.ConfigSingn = FormsAuthentication.HashPasswordForStoringInConfigFile(strTemp, "SHA1");

                            WxPayData newPayData = new WxPayData();
                            newPayData.SetValue("appId", appid);
                            newPayData.SetValue("timeStamp", timeStamp);
                            newPayData.SetValue("nonceStr", nonceStr);
                            newPayData.SetValue("package", package);
                            newPayData.SetValue("signType", "MD5");

                            string paySign = newPayData.MakeSign();

                            ViewBag.AppID = appid;
                            ViewBag.TimeStamp = timeStamp;
                            ViewBag.NonceStr = nonceStr;
                            ViewBag.Package = package;
                            ViewBag.PaySign = paySign;
                            return View();
                        }
                        else
                        {
                            ViewBag.IsFail = "1";
                            return View();
                        }

                    }
                    catch (Exception)
                    {
                        ViewBag.IsFail = "1";
                        return View();
                    }
                }
            }

            return Content("请使用正确方式提交订单！");
        }

    }
}

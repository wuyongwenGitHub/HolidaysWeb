using Com.Alipay;
using Holidays.Model.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;

namespace Holidays.Web.Controllers
{
    public class OrderController : BaseController
    {
        //
        // GET: /Order/

        /// <summary>
        /// 提交订单评价
        /// add by fruitchan
        /// 2017-1-22 17:23:08
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="cleanScore">整洁卫生分数</param>
        /// <param name="locationScore">交通位置分数</param>
        /// <param name="environmentScore">设施环境分数</param>
        /// <param name="serviceScore">房东服务分数</param>
        /// <param name="performanceScore">性价比分数</param>
        /// <returns>评价结果</returns>
        [HttpPost, ValidateInput(false)]
        public ActionResult SubmitEvaluate(long orderId, int cleanScore, int locationScore, int environmentScore, int serviceScore, int performanceScore, string evaluateContent)
        {
            string status = "fail";
            string msg = "提交评价失败！";
            if (orderId > 0)
            {
                OrderInfo orderInfo = OperateContext.Current.BLLSession.IOrderInfoBLL.GetListBy(m => m.ID == orderId).FirstOrDefault();

                if (orderInfo != null)
                {
                    // 订单信息
                    OrderInfo order = OperateContext.Current.BLLSession.IOrderInfoBLL.GetListBy(m => m.ID == orderId).FirstOrDefault();
                    if (order != null)
                    {
                        // 房屋信息
                        HouseInfo houseInfo = OperateContext.Current.BLLSession.IHouseInfoBLL.GetListBy(m => m.ID == order.HouseInfoID).FirstOrDefault();

                        if (houseInfo != null)
                        {
                            int averageScore = 0;
                            // 平均分
                            if (cleanScore != 0 || locationScore != 0 || environmentScore != 0 || serviceScore != 0 || performanceScore != 0)
                            {
                                averageScore = (cleanScore + locationScore + environmentScore + serviceScore + performanceScore) / 5;
                            }

                            // 评价信息
                            int result = OperateContext.Current.BLLSession.IHouseEvaluateBLL.Add(new HouseEvaluate()
                            {
                                AverageScore = averageScore,
                                CleanScore = cleanScore,
                                CreateTime = DateTime.Now,
                                EnvironmentScore = environmentScore,
                                EvaluateContent = evaluateContent,
                                HouseInfoID = order.HouseInfoID,
                                HouseTitle = order.HouseTitle,
                                LocationScore = locationScore,
                                PerformanceScore = performanceScore,
                                ServiceScore = serviceScore,
                                State = 0,
                                UserInfoID = order.UserInfoID,
                                Username = order.Username,
                                UserPhone = order.UserPhone,
                                UserImg = OperateContext.Current.UserInfo.Img

                            });

                            if (result == 1)
                            {
                                // 更新订单评价状态
                                order.IsEvaluate = true;
                                result = OperateContext.Current.BLLSession.IOrderInfoBLL.Modify(order, "IsEvaluate");
                            }

                            if (result == 1)
                            {
                                // 更新房屋评价信息
                                double averageScoreTotal = OperateContext.Current.BLLSession.IHouseEvaluateBLL.GetListBy(m => m.HouseInfoID == order.HouseInfoID).Sum(m => m.AverageScore);
                                int evaluateCount = OperateContext.Current.BLLSession.IHouseEvaluateBLL.CountRow(m => m.HouseInfoID == order.HouseInfoID);

                                int houseAverageScore = Convert.ToInt32(averageScoreTotal / evaluateCount);

                                houseInfo.EvaluateAvgScore = houseAverageScore;
                                houseInfo.EvaluateNum = evaluateCount;
                                OperateContext.Current.BLLSession.IHouseInfoBLL.Modify(houseInfo, "EvaluateNum", "EvaluateAvgScore");
                            }

                            if (result == 1)
                            {
                                status = "ok";
                                msg = "提交评价成功！";
                            }
                        }
                        else
                        {
                            msg = "房源信息不存在！";
                        }
                    }
                    else
                    {
                        msg = "订单信息不存在！";
                    }
                }
                else
                {
                    msg = "订单不存在！";
                }
            }
            else
            {
                msg = "参数错误！";
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 订单页面
        /// add by fruitchan
        /// 2016-12-20 23:08:57
        /// </summary>
        /// <returns>View</returns>
        public ActionResult OrderDetails()
        {
            long userInfoId = OperateContext.Current.UserInfo.ID;
            IList<OrderInfoView> orderInfoList = OperateContext.Current.BLLSession.IOrderInfoViewBLL.GetListBy(m => m.UserInfoID == userInfoId && m.State != -1).OrderByDescending(m => m.CreateTime).ToList();

            if (orderInfoList != null && orderInfoList.Count > 0)
            {
                foreach (var orderInfo in orderInfoList)
                {
                    HouseImg houseImg = OperateContext.Current.BLLSession.IHouseImgBLL.GetListBy(m => m.HouseInfoID == orderInfo.HouseInfoID).FirstOrDefault();
                    if (houseImg != null)
                    {
                        orderInfo.FirstImg = houseImg.ImgUrl;
                    }
                }
            }

            return View(orderInfoList);
        }

        /// <summary>
        /// 更改订单状态
        /// add by fruitchan
        /// 2017-1-20 16:12:06
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="state">订单状态</param>
        /// <returns>更改订单状态结果</returns>
        [HttpPost]
        public ActionResult UpdateOrderState(int orderId, int state)
        {
            string status = "fail";
            string msg = "操作失败！";
            if (orderId > 0 && (state == -1 || state == 3))
            {
                OrderInfo orderInfo = OperateContext.Current.BLLSession.IOrderInfoBLL.GetListBy(m => m.ID == orderId).FirstOrDefault();

                if (orderInfo != null)
                {
                    if (orderInfo.State == 0 || orderInfo.State == 1 || orderInfo.State == 3)
                    {
                        orderInfo.State = state;
                        int result = OperateContext.Current.BLLSession.IOrderInfoBLL.Modify(orderInfo, "State");

                        if (result == 1)
                        {
                            status = "ok";
                            msg = "操作成功！";
                        }
                    }
                    else
                    {
                        msg = "该订单不能操作！";
                    }
                }
                else
                {
                    msg = "订单不存在！";
                }
            }
            else
            {
                msg = "参数错误！";
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 提交订单页面
        /// add by fruitchan
        /// 2016-12-20 23:49:49
        /// </summary>
        /// <returns>View</returns>
        public ActionResult SubmitOrderView(int? id)
        {
            HouseInfoView houseInfo = null;
            UserInfoView loginUserInfo = OperateContext.Current.UserInfo;

            if (id.HasValue)
            {
                houseInfo = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetListBy(m => m.ID == id && m.State == 0).FirstOrDefault();

                if (houseInfo != null)
                {
                    houseInfo.HouseImgList = OperateContext.Current.BLLSession.IHouseImgBLL.GetListBy(m => m.HouseInfoID == houseInfo.ID, m => m.CreateTime, true);
                }
            }

            return View(houseInfo);
        }

        /// <summary>
        /// 提交订单
        /// add by fruitchan
        /// 2017-1-17 21:03:23
        /// </summary>
        /// <param name="order">订单信息</param>
        /// <returns>提交订单结果</returns>
        [HttpPost]
        public ActionResult SubmitOrder(OrderInfo order)
        {
            string status = "fail";
            string msg = "提交订单失败！";

            // 房屋信息
            HouseInfo houseInfo = OperateContext.Current.BLLSession.IHouseInfoBLL.GetListBy(m => m.ID == order.HouseInfoID).FirstOrDefault();

            // 房东信息
            UserInfo userInfo = OperateContext.Current.BLLSession.IUserInfoBLL.GetListBy(m => m.ID == order.LandlordID).FirstOrDefault();

            // 校验数据
            if (houseInfo != null && userInfo != null)
            {
                order.LandlordName = userInfo.Username;
                order.LandlordPhone = userInfo.PhoneNo;

                // 平台提成信息
                SystemConfig config = null;
                if (houseInfo.IsTrusteeship)
                {
                    config = OperateContext.Current.BLLSession.ISystemConfigBLL.GetListBy(m => m.Type == 2).FirstOrDefault();
                }
                else
                {
                    config = OperateContext.Current.BLLSession.ISystemConfigBLL.GetListBy(m => m.Type == 1).FirstOrDefault();
                }

                if (config != null)
                {
                    string strRand = "";
                    Random rand = new Random();
                    for (int i = 0; i < 6; i++)
                    {
                        strRand += rand.Next(0, 10);
                    }

                    // 生成订单编号
                    string orderNo = DateTime.Now.ToString("yyyyMMddHHmmss") + strRand;

                    order.OrderNo = orderNo;
                    order.UserInfoID = OperateContext.Current.UserInfo.ID;
                    order.HouseTitle = houseInfo.HouseTitle;
                    order.Price = 0;
                    order.BuyCount = (order.EndDate - order.StartDate).Days;
                    order.DownPayment = houseInfo.Price * Convert.ToDecimal(config.Value) / 100 * order.BuyCount;
                    order.TotalPrice = houseInfo.Price * order.BuyCount;
                    order.BalancePayment = order.TotalPrice;
                    order.PlatformRoyalty = houseInfo.Price * Convert.ToDecimal(config.Value) / 100 * order.BuyCount;
                    order.IsFullPrice = true;
                    order.State = 0;
                    order.CreateTime = DateTime.Now;
                    order.IsEvaluate = false;

                    int result = OperateContext.Current.BLLSession.IOrderInfoBLL.Add(order);

                    if (result == 1)
                    {
                        status = "ok";
                        msg = orderNo;
                    }
                    else
                    {
                        msg = "提交订单失败！";
                    }
                }
                else
                {
                    msg = "获取平台信息失败！";
                }
            }
            else
            {
                msg = "获取房源信息失败！";
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 订单支付页面
        /// add by fruitchan
        /// 2016-12-20 23:32:58
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public ActionResult OrderPayView(string orderNo, int? isFullPrice)
        {
            ViewBag.IsFullPrice = isFullPrice;

            OrderInfo order = null;
            if (!String.IsNullOrEmpty(orderNo))
            {
                order = OperateContext.Current.BLLSession.IOrderInfoBLL.GetListBy(m => m.OrderNo == orderNo && (m.State == 0 || m.State == 1)).FirstOrDefault();
            }

            return View(order);
        }

        /// <summary>
        /// 处理订单并跳转至第三方支付平台
        /// add by fruitchan
        /// 2017-1-17 22:34:52
        /// </summary>
        /// <param name="orderNo">订单编号</param>
        /// <param name="isFullPrice">是否全款</param>
        /// <param name="payType">支付方式</param>
        /// <returns>View</returns>
        public ActionResult OrderPay(string orderNo, int? isFullPrice, int? payType)
        {
            // 校验数据
            if (!String.IsNullOrEmpty(orderNo) && isFullPrice.HasValue && payType.HasValue)
            {
                if ((isFullPrice.Value != 0 && isFullPrice != 1) || (payType.Value != 1 && payType.Value != 2))
                {
                    return Content("参数错误！");
                }

                bool bIsFullPrice = isFullPrice.Value == 1;

                OrderInfo order = OperateContext.Current.BLLSession.IOrderInfoBLL.GetListBy(m => m.OrderNo == orderNo).FirstOrDefault();

                if (order == null || (order.State != 0 && order.State != 1))
                {
                    return Content("未查询到订单信息或订单状态错误！");
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

                order.PayType = payType.Value;
                order.IsFullPrice = bIsFullPrice;

                int result = OperateContext.Current.BLLSession.IOrderInfoBLL.Modify(order, "PayType", "IsFullPrice");

                if (result == 1)
                {
                    if (payType.Value == 1)
                    {
                        #region 支付宝订单
                        ////////////////////////////////////////////请求参数////////////////////////////////////////////

                        //商户订单号，商户网站订单系统中唯一订单号，必填
                        string out_trade_no = orderItem.OrderNo;

                        //订单名称，必填
                        string subject = order.HouseTitle;

                        //付款金额，必填
                        string total_fee = price.ToString();

                        //商品描述，可空
                        string body = String.Empty;

                        ////////////////////////////////////////////////////////////////////////////////////////////////

                        //把请求参数打包成数组
                        SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                        sParaTemp.Add("service", Config.service);
                        sParaTemp.Add("partner", Config.partner);
                        sParaTemp.Add("seller_id", Config.seller_id);
                        sParaTemp.Add("_input_charset", Config.input_charset.ToLower());
                        sParaTemp.Add("payment_type", Config.payment_type);
                        sParaTemp.Add("notify_url", Config.notify_url);
                        sParaTemp.Add("return_url", Config.return_url);
                        sParaTemp.Add("anti_phishing_key", Config.anti_phishing_key);
                        sParaTemp.Add("exter_invoke_ip", Config.exter_invoke_ip);
                        sParaTemp.Add("out_trade_no", out_trade_no);
                        sParaTemp.Add("subject", subject);
                        sParaTemp.Add("total_fee", total_fee);
                        sParaTemp.Add("body", body);
                        //其他业务参数根据在线开发文档，添加参数.文档地址:https://doc.open.alipay.com/doc2/detail.htm?spm=a219a.7629140.0.0.O9yorI&treeId=62&articleId=103740&docType=1
                        //如sParaTemp.Add("参数名","参数值");

                        //建立请求
                        string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
                        return Content(sHtmlText);

                        #endregion
                    }
                    else if (payType.Value == 2)
                    {
                        try
                        {
                            // 微信支付
                            NativePay nativePay = new NativePay();
                            string url = nativePay.GetPayUrl(orderItem.OrderNo, order.HouseTitle, Convert.ToInt32((price * 100)).ToString());
                            return OperateContext.Current.RedirectAjax("ok", HttpUtility.UrlEncode(url), orderItem.OrderNo, null);
                        }
                        catch (Exception)
                        {
                            return Content("对不起，系统繁忙，请稍后再试！");
                        }
                    }
                }
            }

            return Content("请使用正确方式提交订单！");
        }

        /// <summary>
        /// 检查微信订单支付状态
        /// add by fruitchan
        /// 2017-1-19 10:59:34
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns>检查结果</returns>
        public ActionResult QueryWeixinOrder(string orderNo)
        {
            try
            {
                string result = OrderQuery.Run(String.Empty, orderNo);//调用订单查询业务逻辑
                return Content(result);
            }
            catch (WxPayException ex)
            {
                return Content(ex.Message);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}

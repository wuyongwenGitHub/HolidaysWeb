using Holidays.Common;
using Holidays.Common.Attributes;
using Holidays.Model.Entites;
using Holidays.Model.FormatModel;
using Holidays.Web.Areas.Admin;
using Holidays.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Areas.Weixin.Controllers
{
    public class WeUserController : BaseController
    {
        public bool UserHasPerm()
        {
            bool IsPermission = false;
            //以主账号形式登录
            if (OperateContext.Current.ChildUserInfo==null)
            {
                IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;
            }
            //以子账号形式登录
            else
            {
                //判断权限
                User currentUser = OperateContext.Current.ChildUserInfo;
               
                if (currentUser != null)
                {
                    if (currentUser.ParentId == -1)//房东本人
                    {
                        return IsPermission = true;
                    }
                    //子账户判断权限
                    List<Permission> funcPermissions = AccountHelper.GetUserMenuPermission(currentUser.GUIID);
                    if (funcPermissions != null)
                    {
                        var hasPerm = funcPermissions.Where(s => s.Url == HttpContext.Request.Path).Count() > 0;
                        if (hasPerm)
                        {
                            IsPermission = true;
                        }
                    }
                }
            }
            return IsPermission;
        }
        //
        // GET: /Weixin/WeUser/

        /// <summary>
        /// 个人中心首页
        /// add by fruitchan
        /// 2017-2-7 21:01:42
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            // 用户信息
            ViewBag.UserInfo = ViewBag.UserInfo as UserInfoView;
            ViewBag.IsLogin = ViewBag.UserInfo != null;
            // 订单信息
            IList<OrderInfoView> orderInfoList = null;
            if (OperateContext.Current.UserInfo != null)
            {
                long userInfoId = OperateContext.Current.UserInfo.ID;
                orderInfoList = OperateContext.Current.BLLSession.IOrderInfoViewBLL.GetListBy(m => m.UserInfoID == userInfoId && m.State != -1).OrderByDescending(m => m.CreateTime).ToList();

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
            }

            return View(orderInfoList);
        }

        /// <summary>
        /// 用户信息页面
        /// add by fruitchan
        /// 2017-2-9 18:37:39
        /// </summary>
        /// <returns>View</returns>
        public ActionResult UserInfo()
        {
            UserInfoExtView userInfo = null;
            UserInfoView user = ViewBag.UserInfo as UserInfoView;
            if (user != null)
            {
                userInfo = OperateContext.Current.BLLSession.IUserInfoExtViewBLL.GetListBy(m => m.ID == user.ID).FirstOrDefault();
                ViewBag.UserInfoCertificate = OperateContext.Current.BLLSession.IUserInfoCertificateBLL.GetListBy(m => m.UserInfoID == user.ID).OrderByDescending(m => m.CreateTime).FirstOrDefault();
            }
            return View(userInfo);
        }

        /// <summary>
        /// 实名认证页面
        /// </summary>
        /// <returns>View</returns>
        public ActionResult NameVerified()
        {
            UserInfoExtView userInfo = null;
            UserInfoView user = OperateContext.Current.UserInfo;
            if (user != null)
            {
                userInfo = OperateContext.Current.BLLSession.IUserInfoExtViewBLL.GetListBy(m => m.ID == user.ID).FirstOrDefault();
                ViewBag.UserInfoCertificate = OperateContext.Current.BLLSession.IUserInfoCertificateBLL.GetListBy(m => m.UserInfoID == user.ID).OrderByDescending(m => m.CreateTime).FirstOrDefault();
            }
            return View(userInfo);
        }

        /// <summary>
        /// 用户中心
        /// </summary>
        /// <returns></returns>
        public ActionResult UserCenter()
        {
            ShopInfo shop = new ShopInfo();
            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                //ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;
                ViewBag.IsPermission = UserHasPerm();
                //
                shop = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.UserId == OperateContext.Current.UserInfo.ID).FirstOrDefault();
                ViewBag.HasShop = shop != null ? true : false;

                // ViewBag.IsCheck = ViewBag.HasShop && shopInfo.IsCheck == 1 ? true : false;
            }
            return View(shop);
        }

        /// <summary>
        /// 管理中心登录
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginCenter()
        {

            return View();
        }

        /// <summary>
        /// 关于
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetPassword()
        {
            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="newpwd"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetPwd(string pwd, string newpwd)
        {
            string status = "fail";
            string msg = "修改密码失败！";
            if (string.IsNullOrWhiteSpace(pwd))
            {
                msg = "旧密码不能为空！";
            }
            if (string.IsNullOrWhiteSpace(newpwd))
            {
                msg = "新密码不能为空！";
            }
            var encryptPwd = Common.Encrypt.MD5Encrypt32(pwd.Trim());
            var loginUser = OperateContext.Current.UserInfo;

            if (OperateContext.Current.ChildUserInfo!=null)
            {
                loginUser.LoginPwd = OperateContext.Current.ChildUserInfo.Password;
            }
            if (loginUser.LoginPwd != encryptPwd)
            {
                msg = "密码错误！";
            }
            else
            {
                var encryptNewPwd = Common.Encrypt.MD5Encrypt32(newpwd.Trim());
                var result=0;
                if (OperateContext.Current.ChildUserInfo!=null)//子账号
                {
                    result = OperateContext.Current.BLLSession.IUserBLL.Modify(new User {Id=OperateContext.Current.ChildUserInfo.Id,  Password = encryptNewPwd }, "Password");
               
                }
                else//房东本人
                {
                    result = OperateContext.Current.BLLSession.IUserInfoBLL.Modify(new UserInfo { ID = loginUser.ID, LoginPwd = encryptNewPwd }, "LoginPwd");
                }
                if (result == 1)
                {
                    status = "ok";
                    msg = "修改密码成功！";
                    var accountName = OperateContext.Current.ChildUserInfo == null ? loginUser.LoginAccount : OperateContext.Current.ChildUserInfo.LoginName;
                    //重新登录
                    OperateContext.Current.UserLogin(new UserInfoView { LoginAccount = accountName, LoginPwd = encryptNewPwd }, 3, true);
                }
            }
            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 回复评论
        /// </summary>
        /// <returns></returns>
        

        public ActionResult ReplyCommentView()
        {
            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                //ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;
                ViewBag.IsPermission = UserHasPerm();
                //
            }
            return View();
        }

        /// <summary>
        /// 回复评论
        /// </summary>
        /// <returns></returns>
        public ActionResult ReplyView(long? id)
        {
            HouseEvaluateView hev = new HouseEvaluateView();
            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                //ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;
                ViewBag.IsPermission = UserHasPerm();
                //
                if (id.HasValue)
                {
                    hev = OperateContext.Current.BLLSession.IHouseEvaluateViewBLL.GetListBy(h => h.ID == id).FirstOrDefault();
                    if (hev == null)
                    {
                        hev = new HouseEvaluateView();
                    }
                }
            }
            return View(hev);
        }

        /// <summary>
        /// 回复评论
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReplyComments(long id, string comments)
        {
            string status = "fail";
            string msg = null;
            if (string.IsNullOrWhiteSpace(comments))
            {
                msg = "请输入回复内容！";
            }
            if (msg == null)
            {
                var houseEvaluate = OperateContext.Current.BLLSession.IHouseEvaluateBLL.GetListBy(h => h.ID == id).FirstOrDefault();
                var houseComments = OperateContext.Current.BLLSession.IHouseCommentBLL.GetListBy(h => h.EvaluateId == id).FirstOrDefault();
                if (houseComments == null)
                {
                    houseComments = new HouseComment();
                    houseComments.Comments = comments;
                    houseComments.HouseInfoId = houseEvaluate.HouseInfoID;
                    houseComments.EvaluateId = id;
                    houseComments.ToUserId = houseEvaluate.UserInfoID;
                    houseComments.ToUserName = houseEvaluate.Username;
                    houseComments.FromUserId = OperateContext.Current.UserInfo.ID;
                    houseComments.FromUserName = OperateContext.Current.UserInfo.Nikename;
                    houseComments.CreateOn = DateTime.Now;

                }
                houseComments.Comments = comments;
                var result = houseComments.ID > 0 ? OperateContext.Current.BLLSession.IHouseCommentBLL.Modify(houseComments) : OperateContext.Current.BLLSession.IHouseCommentBLL.Add(houseComments);
                if (result == 1)
                {
                    status = "ok";
                    msg = "保存成功！";
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCommentList(string keywords, int page = 1, int limit = 10)
        {
            var result = new ResponseData<HouseEvaluateView> { code = 0, msg = "", count = 0, data = new List<HouseEvaluateView> { } };
            var shopInfo = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.UserId == OperateContext.Current.UserInfo.ID).FirstOrDefault();
            if (shopInfo != null)
            {
                var whereLambda = PredicateBuilder.True<HouseEvaluateView>();
                Expression<Func<HouseEvaluateView, bool>> shopExpression = h => h.ShopId == shopInfo.ID;
                whereLambda = whereLambda.And(shopExpression);
                #region 查询条件
                if (!string.IsNullOrEmpty(keywords))
                {
                    Expression<Func<HouseEvaluateView, bool>> keywordsExpression = h => h.HouseTitle.Contains(keywords) || h.Username.Contains(keywords);
                    whereLambda = whereLambda.And(keywordsExpression);
                }
                #endregion

                int rowCount = 0;
                result.data = OperateContext.Current.BLLSession.IHouseEvaluateViewBLL.GetPagedList(page, limit, ref rowCount, whereLambda, h => h.CreateTime, false);
                result.count = rowCount;
            }

            return new JsonResult { Data = result };
        }

        /// <summary>
        /// 订单管理
        /// </summary>
        /// <returns></returns>
        

        public ActionResult OrderManageView()
        {
            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                //ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;
                ViewBag.IsPermission = UserHasPerm();
                //
            }
            return View();
        }

        /// <summary>
        /// 获取店铺订单
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="shopType"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetOrderList(string keywords, int? state, int page = 1, int limit = 10)
        {
            var result = new ResponseData<OrderInfoView> { code = 0, msg = "", count = 0, data = new List<OrderInfoView> { } };
            var shopInfo = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.UserId == OperateContext.Current.UserInfo.ID).FirstOrDefault();
            if (shopInfo != null)
            {
                var whereLambda = PredicateBuilder.True<OrderInfoView>();
                #region 查询条件
                //店铺
                Expression<Func<OrderInfoView, bool>> shopIdExpression = h => h.ShopId == shopInfo.ID;
                whereLambda = whereLambda.And(shopIdExpression);
                //
                if (!string.IsNullOrWhiteSpace(keywords))
                {
                    Expression<Func<OrderInfoView, bool>> keywordsExpression = h => h.HouseTitle.Contains(keywords) || h.Username.Contains(keywords) || h.UserPhone.Contains(keywords);
                    whereLambda = whereLambda.And(keywordsExpression);
                }
                //
                if (state.HasValue)
                {
                    Expression<Func<OrderInfoView, bool>> stateExpression = h => h.State == state;
                    whereLambda = whereLambda.And(stateExpression);
                }


                #endregion

                int rowCount = 0;
                result.data = OperateContext.Current.BLLSession.IOrderInfoViewBLL.GetPagedList(page, limit, ref rowCount, whereLambda, h => h.CreateTime, false);
                result.count = rowCount;
            }
            return new JsonResult { Data = result };
        }


        public ActionResult CouponCodeView(long id)
        {
            OrderInfo order = null;
            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                //ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;
                ViewBag.IsPermission = UserHasPerm();
                //
                order = OperateContext.Current.BLLSession.IOrderInfoBLL.GetListBy(h => h.ID == id).FirstOrDefault();
            }
            if (order == null)
            {
                order = new OrderInfo();
            }
            return View(order);
        }

        /// <summary>
        /// 验证兑换码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult VerifyCouponCode(long id, string code)
        {
            string status = "fail";
            string msg = null;
            if (string.IsNullOrWhiteSpace(code))
            {
                msg = "请输入兑换码！";
            }
            if (msg == null)
            {
                var order = OperateContext.Current.BLLSession.IOrderInfoBLL.GetListBy(h => h.ID == id).FirstOrDefault();
                if (order == null)
                {
                    msg = "该订单不存在！";
                }
                if (order.State == 4)
                {
                    msg = "该订单交易已经完成！";
                }
                else if (order.State == 3)
                {
                    msg = "该订单已经被取消！";
                }
                else if (order.State == -1)
                {
                    msg = "该订单已经被删除！";
                }
                code = code.ToUpper();
                if (!string.IsNullOrEmpty(order.CouponCode) && order.CouponCode.Equals(code))
                {
                    order.State = 4; //完成交易
                    var result = OperateContext.Current.BLLSession.IOrderInfoBLL.Modify(order);
                    if (result == 1)
                    {
                        status = "ok";
                        msg = "兑换成功！";
                    }
                }
                else
                {
                    msg = "兑换码无效！";
                }

            }
            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }
        /// <summary>
        /// 财务管理
        /// </summary>
        /// <returns></returns>
        public ActionResult FinanceView()
        {
            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                //ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;
                ViewBag.IsPermission = UserHasPerm();
                var shopInfo = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.UserId == OperateContext.Current.UserInfo.ID).FirstOrDefault();
                ViewBag.TotalMoney = "￥ 0.00";
                ViewBag.TodayTotal = "￥ 0.00";
                ViewBag.SevenDayTotal = "￥ 0.00";
                ViewBag.MonthTotal = "￥ 0.00";
                if (shopInfo != null)
                {
                    Expression<Func<OrderInfoView, bool>> stateExpression = h => h.State == 1 || h.State == 2;
                    Expression<Func<OrderInfoView, bool>> shopExpression = h => h.ShopId == shopInfo.ID;
                    //累计交易额
                    var whereLambda = PredicateBuilder.True<OrderInfoView>();
                    whereLambda = whereLambda.And(stateExpression).And(shopExpression);
                    decimal totalMoney = OperateContext.Current.BLLSession.IOrderInfoViewBLL.GetListBy(whereLambda).Sum(h => h.Price);
                    ViewBag.TotalMoney = "￥ " + totalMoney.ToString("F2");
                    //今日交易额
                    decimal todayTotal = OperateContext.Current.BLLSession.IOrderInfoViewBLL.GetListBy(whereLambda).Where(h => h.CreateTime >= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00") && h.CreateTime <= DateTime.Now.AddDays(1)).Sum(h => h.Price);
                    ViewBag.TodayTotal = "￥ " + todayTotal.ToString("F2");
                    //最近七日交易额
                    decimal SevenDayTotal = OperateContext.Current.BLLSession.IOrderInfoViewBLL.GetListBy(whereLambda).Where(h => h.CreateTime >= DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + " 00:00:00") && h.CreateTime <= DateTime.Now.AddDays(1)).Sum(h => h.Price);
                    ViewBag.SevenDayTotal = "￥ " + SevenDayTotal.ToString("F2");
                    //最近一个月交易额
                    decimal MonthTotal = OperateContext.Current.BLLSession.IOrderInfoViewBLL.GetListBy(whereLambda).Where(h => h.CreateTime >= DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd") + " 00:00:00") && h.CreateTime <= DateTime.Now.AddDays(1)).Sum(h => h.Price);
                    ViewBag.MonthTotal = "￥ " + MonthTotal.ToString("F2");
                }


            }
            return View();
        }

        /// <summary>
        /// 获取财务管理订单
        /// </summary>
        /// <param name="sdate"></param>
        /// <param name="edate"></param>
        /// <param name="state"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFinanceOrderList(DateTime? sdate, DateTime? edate, int? state)
        {
            var result = new ResponseData<OrderInfoView> { code = 0, msg = "", count = 0, data = new List<OrderInfoView> { } };
            var shopInfo = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.UserId == OperateContext.Current.UserInfo.ID).FirstOrDefault();
            if (shopInfo != null)
            {
                var whereLambda = PredicateBuilder.True<OrderInfoView>();
                #region 查询条件
                //店铺
                Expression<Func<OrderInfoView, bool>> shopIdExpression = h => h.ShopId == shopInfo.ID;
                whereLambda = whereLambda.And(shopIdExpression);
                if (sdate.HasValue)
                {
                    //交易时间（起始）
                    Expression<Func<OrderInfoView, bool>> sdateExpression = h => h.CreateTime >= sdate.Value;
                    whereLambda = whereLambda.And(sdateExpression);
                }
                if (edate.HasValue)
                {
                    // 交易时间（结束）
                    edate = edate.Value.AddDays(1);
                    Expression<Func<OrderInfoView, bool>> edateExpression = h => h.CreateTime <= edate;
                    whereLambda = whereLambda.And(edateExpression);
                }
                if (state.HasValue)
                {
                    //订单状态
                    Expression<Func<OrderInfoView, bool>> stateExpression = h => h.State == state;
                    whereLambda = whereLambda.And(stateExpression);
                }


                #endregion

                result.data = OperateContext.Current.BLLSession.IOrderInfoViewBLL.GetListBy(whereLambda, h => h.CreateTime, false);
                if (result.data != null)
                {
                    result.count = result.data.Count();
                }
            }
            return new JsonResult { Data = result };
        }

        public ActionResult HouseScoreView(long id)
        {
            OrderInfoView orderInfo = null;
            orderInfo = OperateContext.Current.BLLSession.IOrderInfoViewBLL.GetListBy(m => m.ID == id).FirstOrDefault();
            return View(orderInfo);
        }

        public ActionResult HouseCommentView(long id)
        {
            HouseEvaluate ev = null;
            var order = OperateContext.Current.BLLSession.IOrderInfoBLL.GetListBy(m => m.ID == id).FirstOrDefault();
            if (order != null)
            {
                ev = OperateContext.Current.BLLSession.IHouseEvaluateBLL.GetListBy(h => h.HouseInfoID == order.HouseInfoID && h.UserInfoID == order.UserInfoID, h => h.CreateTime, false).FirstOrDefault();
            }
            if (ev == null)
            {
                ev = new HouseEvaluate();
            }
            if (order == null)
            {
                order = new OrderInfo();
            }
            ViewBag.Order = order;
            return View(ev);
        }

        [HttpPost]
        public ActionResult SaveHouseScore(HouseEvaluateView model)
        {
            string status = "fail";
            string msg = "评论失败！";
            var order = OperateContext.Current.BLLSession.IOrderInfoBLL.GetListBy(h => h.ID == model.ID).FirstOrDefault();
            if (order == null)
            {
                status = "fail";
                msg = "未查询到订单信息！";
                return OperateContext.Current.RedirectAjax(status, msg, null, null);
            }
            order.IsEvaluate = true;
            OperateContext.Current.BLLSession.IOrderInfoBLL.Modify(order, "IsEvaluate");
            var averageScore = (model.CleanScore + model.LocationScore + model.EnvironmentScore + model.ServiceScore + model.PerformanceScore) / 5;
            var house = OperateContext.Current.BLLSession.IHouseInfoBLL.GetListBy(h => h.ID == order.HouseInfoID).FirstOrDefault();
            if (house != null)
            {
                //
                //var houseEvaluates = OperateContext.Current.BLLSession.IHouseEvaluateBLL.GetListBy(h => h.HouseInfoID == house.ID).ToList();
                //if(houseEvaluates.Count > 0)
                //{

                //}
                house.EvaluateNum += 1;
                house.EvaluateAvgScore += averageScore;
                OperateContext.Current.BLLSession.IHouseInfoBLL.Modify(house, "EvaluateNum", "EvaluateAvgScore");
            }
            HouseEvaluate evaluate = new HouseEvaluate();
            evaluate.HouseInfoID = order.HouseInfoID;
            evaluate.UserInfoID = order.UserInfoID;
            evaluate.UserPhone = order.UserPhone;
            evaluate.CleanScore = model.CleanScore;
            evaluate.LocationScore = model.LocationScore;
            evaluate.EnvironmentScore = model.EnvironmentScore;
            evaluate.ServiceScore = model.ServiceScore;
            evaluate.PerformanceScore = model.PerformanceScore;
            evaluate.AverageScore = averageScore;
            evaluate.EvaluateContent = model.Comments;
            evaluate.State = 0;
            evaluate.HouseTitle = order.HouseTitle;
            evaluate.Username = order.Username;
            evaluate.CreateTime = DateTime.Now;
            var userInfo = OperateContext.Current.BLLSession.IUserInfoBLL.GetListBy(h => h.ID == order.UserInfoID).FirstOrDefault();
            if (userInfo != null)
            {
                evaluate.UserImg = userInfo.Img;
            }
            var result = OperateContext.Current.BLLSession.IHouseEvaluateBLL.Add(evaluate);
            if (result > 0)
            {
                status = "ok";
                msg = "评论成功！";
            }
            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

    }
}

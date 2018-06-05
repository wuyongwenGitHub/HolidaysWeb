using Holidays.Common;
using Holidays.Model.Entites;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /User/

        /// <summary>
        /// 房东信息页面
        /// add by fruitchan
        /// 2016-12-21 20:44:21
        /// </summary>
        /// <returns>View</returns>
        public ActionResult LandlordInfo()
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
        /// 会员信息页面
        /// add by fruitchan
        /// 2016-12-21 20:44:40
        /// </summary>
        /// <returns>View</returns>
        public ActionResult UserInfo()
        {
            UserInfoView user = ViewBag.UserInfo as UserInfoView;
            return View(user);
        }

        /// <summary>
        /// 保存用户数据
        /// add by fruitchan
        /// 2016-12-21 22:44:18
        /// </summary>
        /// <param name="userInfo">用户数据</param>
        /// <returns>保存结果</returns>
        public ActionResult SaveUserInfo(UserInfo userInfo)
        {
            string status = "fail";
            string msg = null;
            UserInfoView loginUserInfo = OperateContext.Current.UserInfo;

            #region 校验数据
            if (userInfo != null)
            {
                // 用户名
                msg = Validate.ValidateString(new CustomValidate()
                {
                    FieldName = "用户名",
                    FieldValue = userInfo.Nikename,
                    IsRequired = true,
                    MaxLength = 20,
                    MinLength = 3
                });

                if (msg == null)
                {
                    // 真实姓名
                    msg = Validate.ValidateString(new CustomValidate
                    {
                        FieldName = "真实姓名",
                        FieldValue = userInfo.Username,
                        IsRequired = true,
                        MaxLength = 20,
                        MinLength = 2
                    });
                }

                if (msg == null && !String.IsNullOrWhiteSpace(userInfo.IDCardNo))
                {
                    // 身份证号
                    msg = Validate.ValidateString(new CustomValidate
                    {
                        FieldName = "身份证号",
                        FieldValue = userInfo.IDCardNo,
                        IsIdCard = true
                    });
                }

                if (msg == null && !String.IsNullOrWhiteSpace(userInfo.Email))
                {
                    // 邮箱地址
                    msg = Validate.ValidateString(new CustomValidate
                    {
                        FieldName = "邮箱地址",
                        FieldValue = userInfo.Email,
                        IsEmail = true
                    });
                }

                if (msg == null)
                {
                    // 手机号
                    msg = Validate.ValidateString(new CustomValidate
                    {
                        FieldName = "手机号",
                        FieldValue = userInfo.PhoneNo,
                        IsRequired = true,
                        IsPhone = true
                    });
                }

                if (msg == null && !String.IsNullOrWhiteSpace(userInfo.PhoneNo2))
                {
                    // 手机号
                    msg = Validate.ValidateString(new CustomValidate
                    {
                        FieldName = "备用手机号",
                        FieldValue = userInfo.PhoneNo2,
                        IsPhone = true
                    });
                }

                // 判断用户名是否存在
                if (userInfo.Nikename != loginUserInfo.Nikename && OperateContext.Current.BLLSession.IUserInfoBLL.IsExist(m => m.Nikename == userInfo.Nikename))
                {
                    msg = "该用户名已被使用，请更换！";
                }

                // 判断是否正在实名认证
                if (OperateContext.Current.BLLSession.IUserInfoCertificateBLL.IsExist(m => m.UserInfoID == loginUserInfo.ID && m.State == 0))
                {
                    msg = "你正在进行实名认证，请实名认证后再修改信息！";
                }
            }
            else
            {
                msg = "请求数据错误！";
            }
            #endregion

            if (msg == null)
            {
                userInfo.ID = loginUserInfo.ID;
                int result = OperateContext.Current.BLLSession.IUserInfoBLL.Modify(userInfo,
                    "Img", "Nikename", "Username", "Gender", "IDCardNo", "Email", "PhoneNo", "PhoneNo2");

                if (result == 1)
                {
                    // 更新缓存
                    OperateContext.Current.UserInfo = OperateContext.Current.BLLSession.IUserInfoViewBLL.GetListBy(m => m.ID == loginUserInfo.ID).FirstOrDefault();
                    status = "ok";
                    msg = "修改成功！";
                }
                else
                {
                    msg = "系统繁忙，请稍后再试！";
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 实名认证
        /// add by fruitchan
        /// 2016-12-24 22:27:11
        /// </summary>
        /// <param name="userInfoExt"></param>
        /// <returns></returns>
        public ActionResult SaveUserInfoExt(UserInfoExtView userInfoExt)
        {
            string status = "fail";
            string msg = null;
            UserInfoView loginUserInfo = OperateContext.Current.UserInfo;

            #region 校验数据
            if (userInfoExt != null)
            {
                if (msg == null)
                {
                    // 真实姓名
                    msg = Validate.ValidateString(new CustomValidate
                    {
                        FieldName = "真实姓名",
                        FieldValue = userInfoExt.Username,
                        IsRequired = true,
                        MaxLength = 20,
                        MinLength = 2
                    });
                }

                if (msg == null)
                {
                    // 身份证号
                    msg = Validate.ValidateString(new CustomValidate
                    {
                        FieldName = "身份证号",
                        FieldValue = userInfoExt.IDCardNo,
                        IsIdCard = true
                    });
                }

                if (msg == null)
                {
                    // 身份证正面照
                    msg = Validate.ValidateString(new CustomValidate
                    {
                        FieldName = "身份证正面照",
                        FieldValue = userInfoExt.IDCardImg1,
                        IsRequired = true,
                        MaxLength = 200
                    });
                }

                if (msg == null)
                {
                    // 身份证背面照
                    msg = Validate.ValidateString(new CustomValidate
                    {
                        FieldName = "身份证背面照",
                        FieldValue = userInfoExt.IDCardImg2,
                        IsRequired = true,
                        MaxLength = 200
                    });
                }

                if (msg == null)
                {
                    // 支付宝账号
                    msg = Validate.ValidateString(new CustomValidate
                    {
                        FieldName = "支付宝账号",
                        FieldValue = userInfoExt.AlipayAccount,
                        IsRequired = true,
                        MaxLength = 40
                    });
                }

                if (msg == null)
                {
                    // 微信账号
                    msg = Validate.ValidateString(new CustomValidate
                    {
                        FieldName = "微信账号",
                        FieldValue = userInfoExt.WeixinAccount,
                        IsRequired = true,
                        MaxLength = 40
                    });
                }

                if (msg == null)
                {
                    // 房源地址
                    msg = Validate.ValidateString(new CustomValidate
                    {
                        FieldName = "房源地址",
                        FieldValue = userInfoExt.HouseAddress,
                        IsRequired = true,
                        MaxLength = 200
                    });
                }

                if (msg == null)
                {
                    // 房产证照
                    msg = Validate.ValidateString(new CustomValidate
                    {
                        FieldName = "房产证照",
                        FieldValue = userInfoExt.Housecertificate,
                        IsRequired = true,
                        MaxLength = 200
                    });
                }
            }
            else
            {
                msg = "请求数据错误！";
            }
            #endregion

            if (msg == null)
            {
                // 用户信息
                int result = OperateContext.Current.BLLSession.IUserInfoBLL.Modify(new UserInfo()
                {
                    ID = loginUserInfo.ID,
                    Username = userInfoExt.Username,
                    IDCardNo = userInfoExt.IDCardNo,
                }, "Username", "IDCardNo");

                // 扩展信息
                if (result == 1)
                {
                    UserInfoExt oldUserInfoExt = OperateContext.Current.BLLSession.IUserInfoExtBLL.GetListBy(m => m.UserInfoID == loginUserInfo.ID).FirstOrDefault();

                    if (oldUserInfoExt != null)
                    {
                        // 修改
                        oldUserInfoExt.IDCardImg1 = userInfoExt.IDCardImg1;
                        oldUserInfoExt.IDCardImg2 = userInfoExt.IDCardImg2;
                        oldUserInfoExt.AlipayAccount = userInfoExt.AlipayAccount;
                        oldUserInfoExt.WeixinAccount = userInfoExt.WeixinAccount;
                        oldUserInfoExt.HouseAddress = userInfoExt.HouseAddress;
                        oldUserInfoExt.Housecertificate = userInfoExt.Housecertificate;

                        result = OperateContext.Current.BLLSession.IUserInfoExtBLL.Modify(oldUserInfoExt, "IDCardImg1",
                            "IDCardImg2", "AlipayAccount", "WeixinAccount", "HouseAddress", "Housecertificate");
                    }
                    else
                    {
                        // 新增
                        result = OperateContext.Current.BLLSession.IUserInfoExtBLL.Add(new UserInfoExt()
                        {
                            UserInfoID = loginUserInfo.ID,
                            IsCertification = 0,
                            IDCardImg1 = userInfoExt.IDCardImg1,
                            IDCardImg2 = userInfoExt.IDCardImg2,
                            AlipayAccount = userInfoExt.AlipayAccount,
                            WeixinAccount = userInfoExt.WeixinAccount,
                            HouseAddress = userInfoExt.HouseAddress,
                            Housecertificate = userInfoExt.Housecertificate
                        });
                    }
                }

                // 提交认证
                if (result == 1)
                {
                    result = OperateContext.Current.BLLSession.IUserInfoCertificateBLL.Add(new UserInfoCertificate()
                    {
                        UserInfoID = loginUserInfo.ID,
                        State = 0,
                        CreateTime = DateTime.Now
                    });
                }

                if (result == 1)
                {
                    // 更新缓存
                    OperateContext.Current.UserInfo = OperateContext.Current.BLLSession.IUserInfoViewBLL.GetListBy(m => m.ID == loginUserInfo.ID).FirstOrDefault();
                    status = "ok";
                    msg = "提交认证申请成功，我们会尽快审核！";
                }
                else
                {
                    msg = "系统繁忙，请稍后再试！";
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 用户管理中心
        /// </summary>
        /// <returns></returns>
        public ActionResult UserCenter()
        {
            ShopInfo shop = new ShopInfo();
            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;
                ViewBag.HasShop = false;
                shop = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.UserId == OperateContext.Current.UserInfo.ID).FirstOrDefault();
                if (shop != null)
                {
                    ViewBag.HasShop = true;
                }
            }

            return View(shop);
        }

        /// <summary>
        /// 用户中心登录
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginCenter()
        {
            return View();
        }

        /// <summary>
        /// 房源管理
        /// </summary>
        /// <returns></returns>
        public ActionResult HouseManager()
        {
            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;
            }
            return View();
        }

        /// <summary>
        /// 订单管理
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderManager()
        {
            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;
            }
            return View();
        }

        /// <summary>
        /// 财务管理
        /// </summary>
        /// <returns></returns>
        public ActionResult FinanceManager()
        {
            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;
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
                    whereLambda.And(stateExpression).And(shopExpression);
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
        /// 评论管理
        /// </summary>
        /// <returns></returns>
        public ActionResult CommentManager()
        {
            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;
            }
            return View();
        }

        /// <summary>
        /// 回复评论
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ReplyView(long? id)
        {
            HouseEvaluateView hev = new HouseEvaluateView();
            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;
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
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetPassword()
        {
            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;
            }
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
            if (loginUser.LoginPwd != encryptPwd)
            {
                msg = "密码错误！";
            }
            else
            {
                var encryptNewPwd = Common.Encrypt.MD5Encrypt32(newpwd.Trim());
                var result = OperateContext.Current.BLLSession.IUserInfoBLL.Modify(new UserInfo { ID = loginUser.ID, LoginPwd = encryptNewPwd }, "LoginPwd");
                if (result == 1)
                {
                    status = "ok";
                    msg = "修改密码成功！";
                    //重新登录
                    OperateContext.Current.UserLogin(new UserInfoView { LoginAccount = loginUser.LoginAccount, LoginPwd = encryptNewPwd }, 3, true);
                }
            }
            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 添加店铺信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ShopInfoView()
        {
            ShopInfo shop = new ShopInfo();
            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;
                shop = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.UserId == OperateContext.Current.UserInfo.ID).FirstOrDefault();
                if (shop == null)
                {
                    shop = new ShopInfo();
                }
                //风景
                ViewBag.SpotList = OperateContext.Current.BLLSession.ISpotInfoBLL.GetListBy(h => h.ID > 0, h => h.Sort).ToList();
                //风格
                ViewBag.ShopCategoryList = OperateContext.Current.BLLSession.IShopCategoryBLL.GetListBy(h => h.ID > 0, h => h.SortBy).ToList();
            }
            return View(shop);
        }

        /// <summary>
        /// 发布房源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CreateHouseInfoView(long? id)
        {
            HouseInfo houseInfo = new HouseInfo();
            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;


                if (id.HasValue)
                {
                    // 根据编号查询房源信息
                    houseInfo = OperateContext.Current.BLLSession.IHouseInfoBLL.GetListBy(m => m.ID == id.Value && m.UserInfoID == OperateContext.Current.UserInfo.ID).FirstOrDefault();
                }
                if (houseInfo != null)
                {
                    // 房源图片
                    houseInfo.HouseImgList = OperateContext.Current.BLLSession.IHouseImgBLL.GetListBy(m => m.HouseInfoID == id.Value);

                    if (houseInfo.HouseImgList != null)
                    {
                        houseInfo.JsonHouseImgList = JsonConvert.SerializeObject(houseInfo.HouseImgList);
                    }

                }
                else
                {
                    houseInfo = new HouseInfo();
                }
                // 配套设施
                ViewBag.HouseConfigureList = OperateContext.Current.BLLSession.IHouseConfigureBLL.GetListBy(m => m.ID > 0, m => m.Sort, true);

            }


            return View(houseInfo);
        }
    }
}

using Holidays.Common;
using Holidays.Common.Attributes;
using Holidays.Model.Entites;
using Holidays.Model.FormatModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Areas.Admin.Controllers
{
    public class LandlordController : Controller
    {
        //
        // GET: /Admin/Landlord/

        /// <summary>
        /// 房东实名认证审核页面
        /// add by fruitchan
        /// 2016-11-22 20:37:34
        /// </summary>
        /// <returns>View</returns>
        

        public ActionResult LandlordCertificationView()
        {
            // 房东审核待办数
            ViewBag.CertificateTotal = OperateContext.Current.BLLSession.IUserInfoCertificateBLL.CountRow(m => m.State == 0);
            return View();
        }

        /// <summary>
        /// 获取房东认证列表
        /// add by fruitchan
        /// 2016-12-9 21:14:42
        /// </summary>
        /// <param name="username">房东姓名</param>
        /// <param name="phoneNo">手机号</param>
        /// <param name="idcard">身份证号</param>
        /// <param name="page">页码</param>
        /// <param name="row">每页数据大小</param>
        /// <returns>房东认证列表</returns>
        public ActionResult GetCertificateList(string username, string phoneNo, string idcard, int page = 1, int row = 10)
        {
            var whereLambda = PredicateBuilder.True<UserInfoCertificateView>();

            #region 查询条件

            // 状态
            Expression<Func<UserInfoCertificateView, bool>> stateExpression = p => p.State == 0;
            whereLambda = whereLambda.And(stateExpression);

            if (!String.IsNullOrWhiteSpace(username))
            {
                // 房东姓名
                Expression<Func<UserInfoCertificateView, bool>> usernameExpression = p => p.Username.Contains(username);
                whereLambda = whereLambda.And(usernameExpression);
            }

            if (!String.IsNullOrWhiteSpace(phoneNo))
            {
                // 手机号
                Expression<Func<UserInfoCertificateView, bool>> phoneNoExpression = p => p.PhoneNo.Contains(phoneNo);
                whereLambda = whereLambda.And(phoneNoExpression);
            }

            if (!String.IsNullOrWhiteSpace(idcard))
            {
                // 身份证号
                Expression<Func<UserInfoCertificateView, bool>> idcardExpression = p => p.IDCardNo.Contains(idcard);
                whereLambda = whereLambda.And(idcardExpression);
            }

            #endregion

            int rowCount = 0;
            IList<UserInfoCertificateView> certificateList = OperateContext.Current.BLLSession.IUserInfoCertificateViewBLL.GetPagedList(page, row, ref rowCount, whereLambda, m => m.CreateTime, false);

            PageModel<UserInfoCertificateView> model = PageModel<UserInfoCertificateView>.GetPageModel(certificateList, page, rowCount, row);

            return new JsonResult() { Data = model };
        }

        /// <summary>
        /// 保存审核信息
        /// add by fruitchan
        /// 2016-12-9 21:27:09
        /// </summary>
        /// <param name="id">审核认证编号</param>
        /// <param name="state">状态</param>
        /// <param name="failReason">失败原因</param>
        /// <returns>保存结果</returns>
        public ActionResult SaveCheckState(long id, int state, string failReason)
        {
            string status = "fail";
            string msg = "操作失败！";

            UserInfoCertificate uic = OperateContext.Current.BLLSession.IUserInfoCertificateBLL.GetListBy(m => m.ID == id).FirstOrDefault();

            if (uic != null)
            {
                uic.State = state;
                uic.FailReason = failReason;

                // 审核信息
                int result = OperateContext.Current.BLLSession.IUserInfoCertificateBLL.Modify(uic);

                if (result == 1 && state == 1)
                {
                    // 房东信息
                    result = OperateContext.Current.BLLSession.IUserInfoBLL.Modify(new UserInfo()
                    {
                        ID = uic.UserInfoID,
                        UserType = 2,
                        IsRealName = 1
                    }, "UserType", "IsRealName");

                    // 房东扩展信息
                    UserInfoExt uie = OperateContext.Current.BLLSession.IUserInfoExtBLL.GetListBy(m => m.UserInfoID == uic.UserInfoID).FirstOrDefault();
                    if (uie != null)
                    {
                        uie.IsCertification = 1;
                        result = OperateContext.Current.BLLSession.IUserInfoExtBLL.Modify(uie, "IsCertification");
                    }
                }

                if (result == 1)
                {
                    status = "ok";
                    msg = "操作成功！";
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 房东信息管理页面
        /// add by fruitchan
        /// 2016-11-22 20:39:47
        /// </summary>
        /// <returns>View</returns>
        

        public ActionResult LandlordManageView()
        {
            // 房东审核待办数
            ViewBag.CertificateTotal = OperateContext.Current.BLLSession.IUserInfoCertificateBLL.CountRow(m => m.State == 0);
            return View();
        }

        /// <summary>
        /// 分页查询房东信息
        /// add by fruitchan
        /// 2016-12-9 23:14:01
        /// </summary>
        /// <param name="username">房东姓名</param>
        /// <param name="phoneNo">手机号</param>
        /// <param name="idcard">身份证号</param>
        /// <param name="page">页码</param>
        /// <param name="row">每页显示行数</param>
        /// <returns>房东信息列表</returns>
        public ActionResult GetUserInfoList(string username, string phoneNo, string idcard, int page = 1, int row = 10)
        {
            var whereLambda = PredicateBuilder.True<UserInfoExtView>();

            #region 查询条件

            // 类型
            Expression<Func<UserInfoExtView, bool>> userTypeExpression = p => p.UserType == 2;
            whereLambda = whereLambda.And(userTypeExpression);

            if (!String.IsNullOrWhiteSpace(username))
            {
                // 房东姓名
                Expression<Func<UserInfoExtView, bool>> usernameExpression = p => p.Username.Contains(username);
                whereLambda = whereLambda.And(usernameExpression);
            }

            if (!String.IsNullOrWhiteSpace(phoneNo))
            {
                // 手机号
                Expression<Func<UserInfoExtView, bool>> phoneNoExpression = p => p.PhoneNo.Contains(phoneNo);
                whereLambda = whereLambda.And(phoneNoExpression);
            }

            if (!String.IsNullOrWhiteSpace(idcard))
            {
                // 身份证号
                Expression<Func<UserInfoExtView, bool>> idcardExpression = p => p.IDCardNo.Contains(idcard);
                whereLambda = whereLambda.And(idcardExpression);
            }

            #endregion

            int rowCount = 0;
            IList<UserInfoExtView> userInfoExtViewList = OperateContext.Current.BLLSession.IUserInfoExtViewBLL.GetPagedList(page, row, ref rowCount, whereLambda, m => m.CreateTime, false);

            PageModel<UserInfoExtView> model = PageModel<UserInfoExtView>.GetPageModel(userInfoExtViewList, page, rowCount, row);

            return new JsonResult() { Data = model };
        }

        /// <summary>
        /// 更新房东账户状态
        /// add by fruitchan
        /// 2016-12-9 23:37:11
        /// </summary>
        /// <param name="id">房东账户编号</param>
        /// <param name="state">新的状态</param>
        /// <returns>更新结果</returns>
        public ActionResult UpdateState(int accountID, int state)
        {
            string status = "fail";
            string msg = "操作失败！";

            UserAccount userAccount = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == accountID).FirstOrDefault();
            //获取房东信息
           var info= OperateContext.Current.BLLSession.IUserInfoBLL.GetListBy(s => s.AccountID == accountID).FirstOrDefault();
            if (userAccount != null)
            {
                //更改权限用户表用户状态
                var user = OperateContext.Current.BLLSession.IUserBLL.GetListBy(s => s.AccountId == info.ID).FirstOrDefault();
                user.IsDeleted = true;
                var editResult = OperateContext.Current.BLLSession.IUserBLL.Modify(user);
                userAccount.State = state;
                int result = OperateContext.Current.BLLSession.IUserAccountBLL.Modify(userAccount);
              
                if (result == 1&&editResult==1 && (state == 1 || state == 2))
                {
                    // 下架房东房源
                    UserInfo userInfo = OperateContext.Current.BLLSession.IUserInfoBLL.GetListBy(m => m.AccountID == accountID).FirstOrDefault();

                    if (userInfo != null)
                    {
                        HouseInfo house = new HouseInfo() { State = 1 };
                        OperateContext.Current.BLLSession.IHouseInfoBLL.ModifyBy(house, m => m.UserInfoID == userInfo.ID, "State");
                    }

                }

                if (result == 1)
                {
                    status = "ok";
                    msg = "操作成功！";
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }


        /// <summary>
        /// 分页查询房东信息
        /// add by fruitchan
        /// 2016-12-9 23:14:01
        /// </summary>
        /// <param name="username">房东姓名</param>
        /// <param name="phoneNo">手机号</param>
        /// <param name="idcard">身份证号</param>
        /// <param name="page">页码</param>
        /// <param name="row">每页显示行数</param>
        /// <returns>房东信息列表</returns>
        [HttpPost]
        public ActionResult GetLandlordInfoList(string username, string phoneNo, string idcard, int page = 1, int limit = 10)
        {
            var result = new ResponseData<UserInfoExtView> { code = 0, msg = "", count = 0, data = new List<UserInfoExtView> { } };
            var whereLambda = PredicateBuilder.True<UserInfoExtView>();

            #region 查询条件

            // 类型
            Expression<Func<UserInfoExtView, bool>> userTypeExpression = p => p.UserType == 2;
            whereLambda = whereLambda.And(userTypeExpression);

            if (!String.IsNullOrWhiteSpace(username))
            {
                // 房东姓名
                Expression<Func<UserInfoExtView, bool>> usernameExpression = p => p.Username.Contains(username);
                whereLambda = whereLambda.And(usernameExpression);
            }

            if (!String.IsNullOrWhiteSpace(phoneNo))
            {
                // 手机号
                Expression<Func<UserInfoExtView, bool>> phoneNoExpression = p => p.PhoneNo.Contains(phoneNo);
                whereLambda = whereLambda.And(phoneNoExpression);
            }

            if (!String.IsNullOrWhiteSpace(idcard))
            {
                // 身份证号
                Expression<Func<UserInfoExtView, bool>> idcardExpression = p => p.IDCardNo.Contains(idcard);
                whereLambda = whereLambda.And(idcardExpression);
            }

            #endregion

            int rowCount = 0;
            result.data = OperateContext.Current.BLLSession.IUserInfoExtViewBLL.GetPagedList(page, limit, ref rowCount, whereLambda, m => m.CreateTime, false);
            result.count = rowCount;

            return new JsonResult() { Data = result };
        }

        /// <summary>
        /// 查看房东信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        

        public ActionResult LandlordInfoView(int? id)
        {
            UserInfoExtView userinfo = null;
            if (id.HasValue)
            {
                userinfo = OperateContext.Current.BLLSession.IUserInfoExtViewBLL.GetListBy(h => h.ID == id).FirstOrDefault();
            }
            if (userinfo == null)
            {
                userinfo = new UserInfoExtView();
            }
            return View(userinfo);
        }

        /// <summary>
        /// 分配账号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        

        public ActionResult CreateAccountView(int id)
        {
            var userinfo = OperateContext.Current.BLLSession.IUserInfoExtViewBLL.GetListBy(h => h.ID == id).FirstOrDefault();
            if (string.IsNullOrEmpty(userinfo.LoginAccount))
            {
                userinfo.LoginAccount = userinfo.PhoneNo;
            }
            if (string.IsNullOrEmpty(userinfo.LoginPwd))
            {
                userinfo.LoginPwd = Encrypt.GetRdNumber().ToString();
            }
            return View(userinfo);
        }

        /// <summary>
        /// 分配账号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateUserAccount(UserInfo user)
        {
            string status = "fail";
            string msg = "保存失败！";
            if (user != null)
            {
                msg = Validate.ValidateString(new CustomValidate()
                {
                    FieldName = "登录账号",
                    FieldValue = user.LoginAccount,
                    IsRequired = true
                }, new CustomValidate()
                {
                    FieldName = "登录密码",
                    FieldValue = user.LoginPwd,
                    IsRequired = true
                }, new CustomValidate()
                {
                    FieldName = "用户ID",
                    FieldValue = user.ID.ToString(),
                    IsRequired = true
                });

                if (msg == null)
                {
                    string encryptPwd = Encrypt.MD5Encrypt32(user.LoginPwd.Trim());
                    var userObj = OperateContext.Current.BLLSession.IUserInfoBLL.GetListBy(h => h.ID == user.ID).FirstOrDefault();
                    userObj.LoginAccount = user.LoginAccount.Trim();
                    userObj.LoginPwd = encryptPwd;
                    var result = OperateContext.Current.BLLSession.IUserInfoBLL.Modify(userObj);
                    if (result == 1)
                    {
                        status = "ok";
                        msg = "保存成功！";
                    }
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newPass"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetPassword(long id, string newPass)
        {
            string status = "fail";
            string msg = "保存失败！";
            if (string.IsNullOrWhiteSpace(newPass))
            {
                msg = "密码不能为空！";
            }
            else
            {
                string encryptPwd = Encrypt.MD5Encrypt32(newPass.Trim());
                var userObj = OperateContext.Current.BLLSession.IUserInfoBLL.GetListBy(h => h.ID == id).FirstOrDefault();
                userObj.LoginPwd = encryptPwd;
                var result = OperateContext.Current.BLLSession.IUserInfoBLL.Modify(userObj);
                if (result == 1)
                {
                    status = "ok";
                    msg = "保存成功！";
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }


        /// <summary>
        /// 创建房东
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        

        public ActionResult CreateLandlordView(long? id)
        {
            UserInfoExtView user = null;
            if (id.HasValue)
            {
                user = OperateContext.Current.BLLSession.IUserInfoExtViewBLL.GetListBy(h => h.ID == id).FirstOrDefault();
            }
            if (user == null)
            {
                user = new UserInfoExtView();
            }
            if (string.IsNullOrEmpty(user.LoginPwd))
            {
                user.LoginPwd = Encrypt.GetRdNumber().ToString();
            }
            return View(user);
        }

        /// <summary>
        /// 保存房东信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveLandlord(UserInfoExtView model)
        {
            string status = "fail";
            string msg = "保存失败！";
            if (model != null)
            {
                msg = Validate.ValidateString(new CustomValidate
                {
                    FieldName = "真实姓名",
                    FieldValue = model.Username,
                    IsRequired = true,
                    MaxLength = 100
                }, new CustomValidate
                {
                    FieldName = "登录账号",
                    FieldValue = model.LoginAccount,
                    IsRequired = true
                }, new CustomValidate
                {
                    FieldName = "登录密码",
                    FieldValue = model.LoginPwd,
                    IsRequired = true
                }, new CustomValidate
                {
                    FieldName = "手机号码",
                    FieldValue = model.PhoneNo,
                    IsRequired = true,
                    IsPhone = true
                });
                //验证备用手机号码
                if(msg == null && !string.IsNullOrEmpty(model.PhoneNo2) && !Validate.ValidatePhone(model.PhoneNo2))
                {
                    msg = "备注手机号码格式不正确";
                }
                else if(msg == null && !string.IsNullOrEmpty(model.Email) && !Validate.ValidateEmail(model.Email))
                {
                    msg = "邮箱格式不正确";
                }
                //验证账号是否重复
                var queryObj = OperateContext.Current.BLLSession.IUserInfoBLL.GetListBy(h => h.LoginAccount == model.LoginAccount && h.ID != model.ID).FirstOrDefault();
                if(queryObj != null)
                {
                    msg = "登录账号已经存在！请重新填写。";
                }
                if(msg == null)
                {
                    if(model.ID > 0) //修改
                    {
                        var modifyUser = OperateContext.Current.BLLSession.IUserInfoBLL.GetListBy(h => h.ID == model.ID).FirstOrDefault();
                        modifyUser.Img = model.Img;
                        modifyUser.Nikename = string.IsNullOrEmpty(model.Nikename) ? modifyUser.Nikename : model.Nikename;
                        modifyUser.Username = model.Username;
                        modifyUser.Gender = model.Gender;
                        modifyUser.PhoneNo = model.PhoneNo;
                        modifyUser.PhoneNo2 = model.PhoneNo2;
                        modifyUser.UserType = 2;
                        modifyUser.Email = model.Email;
                        modifyUser.IDCardNo = model.IDCardNo;
                        modifyUser.LoginAccount = model.LoginAccount;
                        var result = OperateContext.Current.BLLSession.IUserInfoBLL.Modify(modifyUser);

                        var userAccount = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(h => h.ID == modifyUser.AccountID).FirstOrDefault();
                        userAccount.PhoneAccount = modifyUser.PhoneNo;
                        OperateContext.Current.BLLSession.IUserAccountBLL.Modify(userAccount);
                        var userExt = OperateContext.Current.BLLSession.IUserInfoExtBLL.GetListBy(h => h.UserInfoID == modifyUser.ID).FirstOrDefault();
                        if(userExt != null)
                        {
                            userExt.IDCardImg1 = model.IDCardImg1;
                            userExt.IDCardImg2 = model.IDCardImg2;
                            userExt.HouseAddress = model.HouseAddress;
                            userExt.Housecertificate = model.Housecertificate;
                            userExt.WeixinAccount = model.WeixinAccount;
                            userExt.AlipayAccount = model.AlipayAccount;

                            OperateContext.Current.BLLSession.IUserInfoExtBLL.Modify(userExt);
                        }
                        //编辑权限用户表
                        var user = OperateContext.Current.BLLSession.IUserBLL.GetListBy(h => h.Id == model.ID).FirstOrDefault();
                        user.LoginName = model.LoginAccount;
                        user.Password = model.LoginPwd;
                        user.Email = model.Email;
                        var editUserTable = OperateContext.Current.BLLSession.IUserBLL.Add(user);
                        if (result == 1&&editUserTable==1)
                        {

                            status = "ok";
                            msg = "保存成功！";
                        }
                    }
                    else //新增
                    {
                        //用户账户信息
                        UserAccount userAccount = new UserAccount
                        {
                            PhoneAccount = model.PhoneNo,
                            State = 0,
                            CreateTime = DateTime.Now
                        };
                        //用户信息
                        OperateContext.Current.BLLSession.IUserAccountBLL.Add(userAccount);
                        var newUser = new UserInfo();
                        newUser.AccountID = userAccount.ID;
                        newUser.Img = model.Img;
                        newUser.Nikename = string.IsNullOrEmpty(model.Nikename) ? "我要去度假用户" + userAccount.ID : model.Nikename;
                        newUser.Username = model.Username;
                        newUser.Gender = model.Gender;
                        newUser.PhoneNo = model.PhoneNo;
                        newUser.PhoneNo2 = model.PhoneNo2;
                        newUser.UserType = 2;
                        newUser.Email = model.Email;
                        newUser.IDCardNo = model.IDCardNo;
                        newUser.CreateTime = DateTime.Now;
                        newUser.LoginAccount = model.LoginAccount;
                        newUser.LoginPwd = Encrypt.MD5Encrypt32(model.LoginPwd.Trim());
                        var result = OperateContext.Current.BLLSession.IUserInfoBLL.Add(newUser);
                        //添加权限用户表
                        var user = new User();
                        user.GUIID = Guid.NewGuid();
                        user.LoginName = model.LoginAccount;
                        user.Password =Common.Encrypt.MD5Encrypt32( model.LoginPwd);
                        user.IsDeleted = false;
                        user.CreateTime = DateTime.Now;
                        user.Description = "主账户，该账户可以分配子账户以及权限！";
                        user.Email = model.Email;
                        user.ParentId = -1;
                        user.AccountId = newUser.ID;
                        var addUserTable = OperateContext.Current.BLLSession.IUserBLL.Add(user);
                        //认证信息
                        //UserInfoCertificate uic = new UserInfoCertificate
                        //{
                        //    UserInfoID = newUser.ID,
                        //    State = 0,
                        //    CreateTime = DateTime.Now
                        //};
                        //用户扩展信息
                        UserInfoExt userExt = new UserInfoExt();
                        userExt.IsCertification = 1;
                        userExt.UserInfoID = newUser.ID;
                        userExt.IDCardImg1 = model.IDCardImg1;
                        userExt.IDCardImg2 = model.IDCardImg2;
                        userExt.HouseAddress = model.HouseAddress;
                        userExt.Housecertificate = model.Housecertificate;
                        userExt.WeixinAccount = model.WeixinAccount;
                        userExt.AlipayAccount = model.AlipayAccount;
                        OperateContext.Current.BLLSession.IUserInfoExtBLL.Add(userExt);

                        if (result == 1&&addUserTable==1)
                        {
                            status = "ok";
                            msg = "保存成功！";
                        }

                    }

                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

    }
}

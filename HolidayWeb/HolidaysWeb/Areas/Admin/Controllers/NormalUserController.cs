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
    public class NormalUserController : Controller
    {
        //
        // GET: /Admin/NormalUser/

        /// <summary>
        /// 普通用户列表页面
        /// add by fruitchan
        /// 2016-11-22 20:35:19
        /// </summary>
        /// <returns>View</returns>
        [ValidMenuPerm]

        public ActionResult UserlistView()
        {
            // 房东审核待办数
            ViewBag.CertificateTotal = OperateContext.Current.BLLSession.IUserInfoCertificateBLL.CountRow(m => m.State == 0);
            return View();
        }

        /// <summary>
        /// 分页查询会员信息
        /// add by fruitchan
        /// 2016-12-9 23:14:01
        /// </summary>
        /// <param name="username">会员姓名</param>
        /// <param name="phoneNo">手机号</param>
        /// <param name="idcard">身份证号</param>
        /// <param name="page">页码</param>
        /// <param name="row">每页显示行数</param>
        /// <returns>会员信息列表</returns>
        public ActionResult GetUserInfoList(string username, string phoneNo, string idcard, int page = 1, int row = 10)
        {
            var whereLambda = PredicateBuilder.True<UserInfoView>();

            #region 查询条件

            // 状态
            Expression<Func<UserInfoView, bool>> userTypeExpression = p => p.UserType == 1;
            whereLambda = whereLambda.And(userTypeExpression);

            if (!String.IsNullOrWhiteSpace(username))
            {
                // 会员姓名
                Expression<Func<UserInfoView, bool>> usernameExpression = p => p.Username.Contains(username);
                whereLambda = whereLambda.And(usernameExpression);
            }

            if (!String.IsNullOrWhiteSpace(phoneNo))
            {
                // 手机号
                Expression<Func<UserInfoView, bool>> phoneNoExpression = p => p.PhoneNo.Contains(phoneNo);
                whereLambda = whereLambda.And(phoneNoExpression);
            }

            if (!String.IsNullOrWhiteSpace(idcard))
            {
                // 身份证号
                Expression<Func<UserInfoView, bool>> idcardExpression = p => p.IDCardNo.Contains(idcard);
                whereLambda = whereLambda.And(idcardExpression);
            }

            #endregion

            int rowCount = 0;
            IList<UserInfoView> userInfoList = OperateContext.Current.BLLSession.IUserInfoViewBLL.GetPagedList(page, row, ref rowCount, whereLambda, m => m.CreateTime, false);

            PageModel<UserInfoView> model = PageModel<UserInfoView>.GetPageModel(userInfoList, page, rowCount, row);

            return new JsonResult() { Data = model };
        }

        /// <summary>
        /// 更新会员账户状态
        /// add by fruitchan
        /// 2016-12-9 23:37:11
        /// </summary>
        /// <param name="id">会员账户编号</param>
        /// <param name="state">新的状态</param>
        /// <returns>更新结果</returns>
        public ActionResult UpdateState(int accountID, int state)
        {
            string status = "fail";
            string msg = "操作失败！";

            UserAccount userAccount = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == accountID).FirstOrDefault();

            if (userAccount != null)
            {
                userAccount.State = state;
                int result = OperateContext.Current.BLLSession.IUserAccountBLL.Modify(userAccount);

                if (result == 1)
                {
                    status = "ok";
                    msg = "操作成功！";
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 用户收藏列表页面
        /// add by fruitchan
        /// 2016-11-23 00:12:18
        /// </summary>
        /// <returns>View</returns>
        [ValidMenuPerm]

        public ActionResult UserFavoriteListView()
        {
            // 房东审核待办数
            ViewBag.CertificateTotal = OperateContext.Current.BLLSession.IUserInfoCertificateBLL.CountRow(m => m.State == 0);
            return View();
        }

        /// <summary>
        /// 分页查询会员收藏房源
        /// add by fruithan
        /// 2016-12-9 20:13:32
        /// </summary>
        /// <param name="username">会员姓名</param>
        /// <param name="phoneNo">手机号</param>
        /// <param name="houseTitle">房源标题</param>
        /// <param name="page">页码</param>
        /// <param name="row">每页显示数据条数</param>
        /// <returns>会员收藏房源列表</returns>
        [HttpPost]
        public ActionResult GetUserFavoriteList(string username, string phoneNo, string houseTitle, int page = 1, int row = 10)
        {
            var whereLambda = PredicateBuilder.True<UserFavoriteView>();

            #region 查询条件

            if (!String.IsNullOrWhiteSpace(username))
            {
                // 会员姓名
                Expression<Func<UserFavoriteView, bool>> usernameExpression = p => p.Username.Contains(username);
                whereLambda =  whereLambda.And(usernameExpression);
            }

            if (!String.IsNullOrWhiteSpace(phoneNo))
            {
                // 手机号
                Expression<Func<UserFavoriteView, bool>> phoneNoExpression = p => p.PhoneNo.Contains(phoneNo);
                whereLambda = whereLambda.And(phoneNoExpression);
            }

            if (!String.IsNullOrWhiteSpace(houseTitle))
            {
                // 房源标题
                Expression<Func<UserFavoriteView, bool>> houseTitleExpression = p => p.HouseTitle.Contains(houseTitle);
                whereLambda = whereLambda.And(houseTitleExpression);
            }

            #endregion

            int rowCount = 0;
            IList<UserFavoriteView> userFavoriteList = OperateContext.Current.BLLSession.IUserFavoriteViewBLL.GetPagedList(page, row, ref rowCount, whereLambda, m => m.CreateTime, false);

            PageModel<UserFavoriteView> model = PageModel<UserFavoriteView>.GetPageModel(userFavoriteList, page, rowCount, row);

            return new JsonResult() { Data = model };
        }

        /// <summary>
        /// 根据编号删除会员收藏的房源
        /// add by fruitchan
        /// 2016-12-9 20:15:01
        /// </summary>
        /// <param name="id">收藏编号</param>
        /// <returns>删除结果</returns>
        public ActionResult DelUserFavoriteByID(long id)
        {
            int result = OperateContext.Current.BLLSession.IUserFavoriteBLL.DelBy(m => m.ID == id);

            return OperateContext.Current.RedirectAjax(result == 1 ? "ok" : "fail", result == 1 ? "删除成功！" : "删除失败！", null, null);
        }
    }
}

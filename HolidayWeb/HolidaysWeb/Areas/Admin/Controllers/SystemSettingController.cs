using Holidays.Common;
using Holidays.Common.Attributes;
using Holidays.Model.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Areas.Admin.Controllers
{
    public class SystemSettingController : Controller
    {
        //
        // GET: /Admin/SystemSetting/

        /// <summary>
        /// 修改密码页面
        /// add by fruitchan
        /// 2016-11-22 20:57:12
        /// </summary>
        /// <returns>View</returns>
        [ValidMenuPerm]

        public ActionResult UpdatePasswordView()
        {
            // 房东审核待办数
            ViewBag.CertificateTotal = OperateContext.Current.BLLSession.IUserInfoCertificateBLL.CountRow(m => m.State == 0);
            return View();
        }

        /// <summary>
        /// 修改密码
        /// add by fruitchan
        /// 2016-12-7 23:22:16
        /// </summary>
        /// <param name="oldPassword">原来密码</param>
        /// <param name="newPassword">新的密码</param>
        /// <param name="confirmPassword">确认密码</param>
        /// <returns>修改结果</returns>
        [HttpPost]
        public ActionResult UpdatePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            string status = "-1";
            string msg = "修改密码失败！";

            msg = Validate.ValidateString(new CustomValidate()
            {
                FieldName = "原来密码",
                FieldValue = oldPassword,
                IsRequired = true,
                MaxLength = 20,
                MinLength = 6
            },
            new CustomValidate()
            {
                FieldName = "新的密码",
                FieldValue = newPassword,
                IsRequired = true,
                MaxLength = 20,
                MinLength = 6
            });

            if (msg == null)
            {
                msg = newPassword == confirmPassword ? null : "两次密码不一致";

                if (msg == null)
                {
                    oldPassword = Encrypt.MD5Encrypt32(oldPassword);

                    int id = OperateContext.Current.AdminUser.ID;
                    AdminUser adminUser = OperateContext.Current.BLLSession.IAdminUserBLL.GetListBy(m => m.ID == id).FirstOrDefault();

                    // 验证旧密码
                    if (adminUser != null && adminUser.LoginPassword == oldPassword)
                    {
                        newPassword = Encrypt.MD5Encrypt32(newPassword);
                        adminUser.LoginPassword = newPassword;

                        int result = OperateContext.Current.BLLSession.IAdminUserBLL.Modify(adminUser, "LoginPassword");

                        if (result == 1)
                        {
                            status = "ok";
                            msg = "修改成功！";
                        }
                    }
                    else
                    {
                        msg = "原来密码不正确！";
                    }
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 平台提成设置页面
        /// add by fruitchan
        /// 2016-11-23 00:29:53
        /// </summary>
        /// <returns>View</returns>
        [ValidMenuPerm]

        public ActionResult RoyaltyRateSettingView()
        {
            // 房东审核待办数
            ViewBag.CertificateTotal = OperateContext.Current.BLLSession.IUserInfoCertificateBLL.CountRow(m => m.State == 0);

            SystemConfig normalRateConfig = OperateContext.Current.BLLSession.ISystemConfigBLL.GetListBy(m => m.Type == 1).FirstOrDefault();
            SystemConfig hostingRateConfig = OperateContext.Current.BLLSession.ISystemConfigBLL.GetListBy(m => m.Type == 2).FirstOrDefault();

            ViewBag.NormalRateConfig = normalRateConfig == null ? "0" : normalRateConfig.Value;
            ViewBag.HostingRateConfig = hostingRateConfig == null ? "0" : hostingRateConfig.Value;

            return View();
        }

        /// <summary>
        /// 更新平台提成比例
        /// add by fruitchan
        /// 2016-12-11 15:34:55
        /// </summary>
        /// <param name="normalRate">普通租房提成比例</param>
        /// <param name="hostingRate">托管租房提成比例</param>
        /// <returns>更新结果</returns>
        [HttpPost]
        public ActionResult UpdateRoyaltyRate(float? normalRate, float? hostingRate)
        {
            string status = "-1";
            string msg = null;

            if (!hostingRate.HasValue || hostingRate < 0 || hostingRate > 100)
            {
                msg = "托管租房提成比例不能低于0且不能大于100";
            }

            if (!normalRate.HasValue || normalRate < 0 || normalRate > 100)
            {
                msg = "普通租房提成比例不能低于0且不能大于100";
            }

            if (msg == null)
            {
                SystemConfig normalRateConfig = OperateContext.Current.BLLSession.ISystemConfigBLL.GetListBy(m => m.Type == 1).FirstOrDefault();
                SystemConfig hostingRateConfig = OperateContext.Current.BLLSession.ISystemConfigBLL.GetListBy(m => m.Type == 2).FirstOrDefault();

                int result1 = 0;
                int result2 = 0;

                // 普通租房提成比例
                if (normalRateConfig == null)
                {
                    result1 = OperateContext.Current.BLLSession.ISystemConfigBLL.Add(new SystemConfig()
                    {
                        Type = 1,
                        Name = "普通租房提成比例",
                        Value = normalRate.ToString()
                    });
                }
                else
                {
                    normalRateConfig.Value = normalRate.ToString();
                    result1 = OperateContext.Current.BLLSession.ISystemConfigBLL.Modify(normalRateConfig, "Value");
                }

                // 托管租房提成比例
                if (hostingRateConfig == null)
                {
                    result2 = OperateContext.Current.BLLSession.ISystemConfigBLL.Add(new SystemConfig()
                    {
                        Type = 2,
                        Name = "托管租房提成比例",
                        Value = hostingRate.ToString()
                    });
                }
                else
                {
                    hostingRateConfig.Value = hostingRate.ToString();
                    result2 = OperateContext.Current.BLLSession.ISystemConfigBLL.Modify(hostingRateConfig, "Value");
                }

                if (result1 != 0 && result2 != 0)
                {
                    status = "ok";
                    msg = "保存成功！";
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }
    }
}

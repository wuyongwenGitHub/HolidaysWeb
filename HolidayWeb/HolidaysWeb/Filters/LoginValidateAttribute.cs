using Holidays.Model.Entites;
using Holidays.Web.Areas.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Filters
{
    /// <summary>
    /// 管理员 身份验证 过滤器
    /// </summary>
    public class LoginValidateAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        #region 1.0 验证方法 - 在 ActionExcuting过滤器之前执行
        /// <summary>
        /// 验证方法 - 在 ActionExcuting过滤器之前执行
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
        
            
            //1.如果请求的 Admin 区域里的 控制器类和方法，那么就要验证权限
            // 当前请求匹配的 路由对象中 是否 有 area区域
            // 监测区域名 是否为 admin weixin
            if (!filterContext.RouteData.DataTokens.Keys.Contains("area") || filterContext.RouteData.DataTokens["area"].ToString().ToLower() != "admin"
               )
            {
                return;
            }

            //2.检查 被请求的 方法 和 控制器是否有 Skip 标签，如果有，则不验证；如果没有，则验证
            if (filterContext.ActionDescriptor.IsDefined(typeof(Common.Attributes.SkipAttribute), false) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(Common.Attributes.SkipAttribute), false))
            {
                return;
            }
            //3、检查当前后台用户是否有访问页面的权限
            //获取当前用户权限列表
            User currentUser = OperateContext.Current.User;
            if (currentUser!=null)
            {
                List<Permission> funcPermissions = AccountHelper.GetUserMenuPermission(currentUser.GUIID);
                if (funcPermissions != null)
                {
                    var hasPerm = funcPermissions.Where(s =>s.Url == filterContext.HttpContext.Request.Path).Count()>0;
                    if (filterContext.ActionDescriptor.IsDefined(typeof(Common.Attributes.ValidMenuPermAttribute), false)
                        ||
                        filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(Common.Attributes.ValidMenuPermAttribute), false))
                    {
                        if (hasPerm)
                        {
                            return;
                        }
                        else
                        {
                            filterContext.HttpContext.Response.Redirect("~/Admin/NoPermission/Index");
                            filterContext.HttpContext.Response.End();
                        }
                    }
                }
            }
         
            // 4.判断用户是否登录
            if (!OperateContext.Current.AdminIsLogin())
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest() || filterContext.HttpContext.Request.Headers["X-Requested-With"] != null)
                {
                    filterContext.Result = OperateContext.Current.RedirectAjax("-1", "登录失效，请重新登录！", null, null);
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Admin/Login?backUrl=" + filterContext.HttpContext.Request.Url.AbsolutePath);
                }
            }

            return;
        }
        #endregion
    }
}
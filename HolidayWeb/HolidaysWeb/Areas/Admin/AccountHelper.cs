using Holidays.Model.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Holidays.Web.Areas.Admin
{
    /// <summary>
    /// 账户帮助类，用户获取用户权限
    /// </summary>
    public static class AccountHelper
    {
        /// <summary>
        /// 获取当前用户的所有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<Permission> GetUserMenuPermission(Guid userId)
        {
            if (HttpContext.Current.Cache["UserPermissions"] == null)
            {
                //获取当前用户所有角色
               List<UserRole> userRoles= OperateContext.Current.BLLSession.IUserRoleBLL.GetListBy(s => s.UserId == userId);
                if (userRoles==null)
                {
                    return new List<Permission>();
                }
                List<Guid> roleGUIDList = (from a in userRoles
                                           select a.RoleId).ToList();

              List<FuncPermission> currentUserAllMenuPerms=  OperateContext.Current.BLLSession.IFuncPermissionBLL.GetListBy(s =>roleGUIDList.Contains(s.MemberId));
                if (currentUserAllMenuPerms==null)
                {
                    return new List<Permission>();
                }
                List<Guid> PermGUIDList = (from a in currentUserAllMenuPerms
                                           select a.FuncId).ToList();
                List<Permission> Permissions = OperateContext.Current.BLLSession.IPermissionBLL.GetListBy(s => s.IsDeleted == false && PermGUIDList.Contains(s.GUID)&&s.PermValue==1);
                HttpContext.Current.Cache.Insert("UserPermissions", Permissions, null,System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(5), System.Web.Caching.CacheItemPriority.Normal, null);
               return (List<Permission>)Permissions;
            }
            else
            {
                return (List<Permission>)HttpContext.Current.Cache["UserPermissions"];
            }
        }
    }
}
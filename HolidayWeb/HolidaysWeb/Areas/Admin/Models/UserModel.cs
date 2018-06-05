using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Holidays.Web.Areas.Admin.Models
{
    /// <summary>
    /// 管理员创建的用户信息
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 登录名字
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string UserRealName { get; set; }
        /// <summary>
        /// 用户描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email { get; set; }

    }
}
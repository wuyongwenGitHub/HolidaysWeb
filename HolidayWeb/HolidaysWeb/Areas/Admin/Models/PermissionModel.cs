using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Holidays.Web.Areas.Admin.Models
{
    public class PermissionModel
    {

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// 0:操作 1:菜单
        /// </summary>
        public long Type { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 父级id 菜单权限默认为0 操作权限为对应的菜单权限ID
        /// </summary>
        public long ParentId { get; set; } = 0;
    }
}
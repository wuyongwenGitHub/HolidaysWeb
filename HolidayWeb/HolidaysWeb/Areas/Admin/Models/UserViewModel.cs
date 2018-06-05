using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Holidays.Web.Areas.Admin.Models
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string UserRealName { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string Email { get; set; }
        public System.Guid GUIID { get; set; }

        public string RoleIds { get; set; }
    }
}
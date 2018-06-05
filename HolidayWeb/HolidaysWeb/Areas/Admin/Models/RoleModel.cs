using Holidays.Model.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Holidays.Web.Areas.Admin.Models
{
    public class RoleModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; } = false;
        public List<Permission> Permissions { get; set; }
    }
}
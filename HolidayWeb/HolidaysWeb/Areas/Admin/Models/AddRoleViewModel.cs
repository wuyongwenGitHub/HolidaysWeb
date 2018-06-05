using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Holidays.Web.Areas.Admin.Models
{
    public class AddRoleViewModel
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string MenuIds { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Holidays.Web.Areas.Admin.Models
{
    public class MenuFuncViewModel
    {
        public long Id { get; set; }
        public string MenuName { get; set; }
        public string Url { get; set; }
        public string MenuLevel { get; set; }
        public long ParentId { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}
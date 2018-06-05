using Holidays.Model.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Holidays.Web.Areas.Admin.Models
{
    public class RolePermViewModel
    {

        public Role Role { get; set; }
        public  List<MenuFuncViewModel> MenuFuncViewModels { get; set; }
    }
}
using Holidays.Model.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Holidays.Web.Areas.Admin.Models
{
    public class QueryUserByIdViewModel
    {
        public User User { get; set; }
        public List<RoleWithSelectedViewModel> RoleWithSelectedViewModels { get; set; }
    }
}
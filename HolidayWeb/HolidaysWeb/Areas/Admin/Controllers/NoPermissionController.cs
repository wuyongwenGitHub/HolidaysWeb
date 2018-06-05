using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Areas.Admin.Controllers
{
    public class NoPermissionController : Controller
    {
        //
        // GET: /Admin/NoPermission/

        public ActionResult Index()
        {
            return View();
        }

    }
}

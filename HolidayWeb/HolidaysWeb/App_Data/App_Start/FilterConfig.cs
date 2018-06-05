using System.Web;
using System.Web.Mvc;

namespace Holidays.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new Filters.LogExceptionAttribute());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Holidays.Web.Filters.LoginValidateAttribute());
        }
    }
}
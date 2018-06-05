using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class LogExceptionAttribute: HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                Exception ex = filterContext.Exception;
                Model.Entites.SysLog log = new Model.Entites.SysLog();
                log.CreateOn = DateTime.Now;
                log.ErrorMsg = ex.StackTrace;
                OperateContext.Current.BLLSession.ISysLogBLL.Add(log);
            }
            base.OnException(filterContext);
        }
    }
}
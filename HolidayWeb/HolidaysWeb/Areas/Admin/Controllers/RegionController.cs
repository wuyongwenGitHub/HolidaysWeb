using Holidays.Model.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Areas.Admin.Controllers
{
    public class RegionController : Controller
    {
        //
        // GET: /Admin/Region/

        /// <summary>
        /// 获取省份列表
        /// add by fruitchan
        /// 2017-1-10 19:44:51
        /// </summary>
        /// <returns>省份列表数据</returns>
        public ActionResult GetProvinceList()
        {
            IList<Region> regionList = OperateContext.Current.BLLSession.IRegionBLL.GetListBy(m => m.ParentId == 100000).ToList();
            return OperateContext.Current.RedirectAjax("ok", null, regionList, null);
        }

        /// <summary>
        /// 根据父级编号获取城市列表
        /// add by fruitchan
        /// 2017-1-10 19:46:20
        /// </summary>
        /// <param name="parentId">父级编号</param>
        /// <returns>城市列表</returns>
        public ActionResult GetCityByParentId(int parentId)
        {
            IList<Region> regionList = OperateContext.Current.BLLSession.IRegionBLL.GetListBy(m => m.ParentId == parentId).ToList();
            return OperateContext.Current.RedirectAjax("ok", null, regionList, null);
        }
    }
}

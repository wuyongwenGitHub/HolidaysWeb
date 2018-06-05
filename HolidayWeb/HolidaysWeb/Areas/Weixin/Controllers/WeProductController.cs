using Holidays.Model.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Areas.Weixin.Controllers
{
    public class WeProductController : Controller
    {
        //
        // GET: /Weixin/WeProduct/

        /// <summary>
        /// 商品列表页面
        /// add by fruitchan
        /// 2017-2-14 20:40:35
        /// </summary>
        /// <returns>View</returns>
        public ActionResult ProductListView(int? type, string keywords)
        {
            keywords = keywords == null ? "" : keywords;

            ViewBag.Type = type;
            ViewBag.Keywords = keywords;

            IList<ProductInfo> productList = new List<ProductInfo>();
            ViewBag.CategoryList = OperateContext.Current.BLLSession.IProductCategoryBLL.GetListBy(m => m.ID > 0, m => m.Sort).ToList();
            if (type.HasValue)
            {
                productList = OperateContext.Current.BLLSession.IProductInfoBLL.GetListBy(m => m.CityId == OperateContext.Current.CurrentCity.Id && m.CategoryID == type.Value && m.ProductName.Contains(keywords), m => m.Sort, true);
            }
            else
            {
                productList = OperateContext.Current.BLLSession.IProductInfoBLL.GetListBy(m => m.CityId == OperateContext.Current.CurrentCity.Id && m.ProductName.Contains(keywords), m => m.Sort, true);
            }
            return View(productList);
        }

    }
}

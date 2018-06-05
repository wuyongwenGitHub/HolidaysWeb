using Holidays.Common;
using Holidays.Model.Entites;
using Holidays.Model.FormatModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Controllers
{
    public class ProductController : BaseController
    {
        //
        // GET: /Product/

        /// <summary>
        /// 商品列表
        /// add by fruitchan
        /// 2016-12-16 23:23:59
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            ViewBag.CategoryList = OperateContext.Current.BLLSession.IProductCategoryBLL.GetListBy(m => m.ID > 0, m => m.Sort).ToList();
            return View();
        }

        /// <summary>
        /// 分页查询商品信息
        /// add by fruitchan
        /// 2016-12-6 22:28:22
        /// </summary>
        /// <param name="productName">商品名称</param>
        /// <param name="categoryID">商品分类编号</param>
        /// <param name="page">页码</param>
        /// <param name="row">每行显示数据条数</param>
        /// <returns>商品列表</returns>
        [HttpPost]
        public ActionResult GetProductList(string productName, long? categoryID, int page = 1, int row = 12)
        {
            var whereLambda = PredicateBuilder.True<ProductInfo>();

            #region 查询条件

            // 城市
            Expression<Func<ProductInfo, bool>> cityIdExpression = p => p.CityId == OperateContext.Current.CurrentCity.Id;
            whereLambda = whereLambda.And(cityIdExpression);

            if (!String.IsNullOrWhiteSpace(productName))
            {
                // 商品名称
                Expression<Func<ProductInfo, bool>> productNameExpression = p => p.ProductName.Contains(productName);
                whereLambda = whereLambda.And(productNameExpression);
            }

            if (categoryID.HasValue && categoryID.Value > 0)
            {
                // 联系人
                Expression<Func<ProductInfo, bool>> categoryIDExpression = p => p.CategoryID == categoryID.Value;
                whereLambda = whereLambda.And(categoryIDExpression);
            }

            #endregion

            int rowCount = 0;
            IList<ProductInfo> productList = OperateContext.Current.BLLSession.IProductInfoBLL.GetPagedList(page, row, ref rowCount, whereLambda, m => m.Sort, true);

            PageModel<ProductInfo> model = PageModel<ProductInfo>.GetPageModel(productList, page, rowCount, row);

            return new JsonResult() { Data = model };
        }

    }
}

using Holidays.Common;
using Holidays.Common.Attributes;
using Holidays.Model.Entites;
using Holidays.Model.FormatModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Areas.Admin.Controllers
{
    public class TourismProductController : Controller
    {
        //
        // GET: /Admin/TourismProduct/

        #region 商品分类管理 + add by fruitchan
        /// <summary>
        /// 旅游商品分类管理页面
        /// add by fruitchan
        /// 2016-11-22 20:50:55
        /// </summary>
        /// <returns>View</returns>
        

        public ActionResult CategoryManageView()
        {
            // 房东审核待办数
            ViewBag.CertificateTotal = OperateContext.Current.BLLSession.IUserInfoCertificateBLL.CountRow(m => m.State == 0);
            return View();
        }

        /// <summary>
        /// 查询分类列表数据
        /// add by fruitchan
        /// 2016-12-6 21:49:47
        /// </summary>
        /// <returns>分类列表数据</returns>
        public ActionResult GetCategoryList()
        {
            IList<ProductCategory> productCategoryList = OperateContext.Current.BLLSession.IProductCategoryBLL.GetListBy(m => m.ID > 0, m => m.Sort).ToList();

            return OperateContext.Current.RedirectAjax("ok", null, productCategoryList, null);
        }

        /// <summary>
        /// 根据编号查询商品分类信息
        /// add by fruitchan
        /// 2016-12-6 21:46:43
        /// </summary>
        /// <param name="id">商品分类编号</param>
        /// <returns>商品分类信息</returns>
        public ActionResult QueryCategoryByID(long id)
        {
            return OperateContext.Current.RedirectAjax("ok", null, OperateContext.Current.BLLSession.IProductCategoryBLL.GetListBy(m => m.ID == id).FirstOrDefault(), null);
        }

        /// <summary>
        /// 根据编号删除商品分类
        /// add by fruitchan
        /// 2016-12-6 21:45:56
        /// </summary>
        /// <param name="id">商品分类编号</param>
        /// <returns>删除结果</returns>
        public ActionResult DelCategoryByID(long id)
        {
            int result = OperateContext.Current.BLLSession.IProductCategoryBLL.DelBy(m => m.ID == id);

            return OperateContext.Current.RedirectAjax(result == 1 ? "ok" : "fail", result == 1 ? "删除成功！" : "删除失败！", null, null);
        }

        /// <summary>
        /// 保存商品分类信息
        /// add by fruitchan
        /// 2016-12-6 21:44:26
        /// </summary>
        /// <param name="category">分类信息</param>
        /// <returns>保存结果</returns>
        public ActionResult SaveCategory(ProductCategory category)
        {
            string status = "fail";
            string msg = "保存失败！";

            if (category != null)
            {
                msg = Validate.ValidateString(new CustomValidate()
                {
                    FieldName = "分类名称",
                    FieldValue = category.CategoryName,
                    IsRequired = true,
                    MaxLength = 20
                });

                if (msg == null)
                {
                    category.CreateTime = DateTime.Now;
                    int result = category.ID > 0 ? OperateContext.Current.BLLSession.IProductCategoryBLL.Modify(category) : OperateContext.Current.BLLSession.IProductCategoryBLL.Add(category);

                    if (result == 1)
                    {
                        status = "ok";
                        msg = "保存成功！";
                    }
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }
        #endregion

        #region 商品管理 + add by fruitchan
        /// <summary>
        /// 旅游商品管理页面
        /// add by fruitchan
        /// 2016-11-22 20:51:37
        /// </summary>
        /// <returns>View</returns>
        

        public ActionResult ProductManageView()
        {
            // 房东审核待办数
            ViewBag.CertificateTotal = OperateContext.Current.BLLSession.IUserInfoCertificateBLL.CountRow(m => m.State == 0);
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
        public ActionResult GetProductList(string productName, long? categoryID, int page = 1, int row = 10)
        {
            var whereLambda = PredicateBuilder.True<ProductInfo>();

            #region 查询条件

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

        /// <summary>
        /// 根据编号删除商品信息
        /// add by fruitchan
        /// 2016-12-6 22:32:37
        /// </summary>
        /// <param name="id">商品编号</param>
        /// <returns>删除结果</returns>
        public ActionResult DelProductByID(long id)
        {
            int result = OperateContext.Current.BLLSession.IProductInfoBLL.DelBy(m => m.ID == id);

            return OperateContext.Current.RedirectAjax(result == 1 ? "ok" : "fail", result == 1 ? "删除成功！" : "删除失败！", null, null);
        }

        /// <summary>
        /// 根据编号查询商品信息
        /// add by fruitchan
        /// </summary>
        /// <param name="id">商品编号</param>
        /// <returns>商品信息</returns>
        public ActionResult QueryProductByID(long id)
        {
            return OperateContext.Current.RedirectAjax("ok", null, OperateContext.Current.BLLSession.IProductInfoBLL.GetListBy(m => m.ID == id).FirstOrDefault(), null);
        }

        /// <summary>
        /// 保存商品信息
        /// add by fruitchan
        /// 2016-12-6 22:33:16
        /// </summary>
        /// <param name="product">商品信息</param>
        /// <returns>保存结果</returns>
        public ActionResult SaveProduct(ProductInfo product)
        {
            string status = "fail";
            string msg = "保存失败！";

            if (product != null)
            {
                msg = Validate.ValidateString(new CustomValidate()
                {
                    FieldName = "商品名称",
                    FieldValue = product.ProductName,
                    IsRequired = true,
                    MaxLength = 20
                }, new CustomValidate()
                {
                    FieldName = "商品分类",
                    FieldValue = product.CategoryID == 0 ? null : product.CategoryID.ToString(),
                    IsRequired = true
                }, new CustomValidate()
                {
                    FieldName = "商品价格",
                    FieldValue = product.Price,
                    IsRequired = true,
                    MaxLength = 20
                });

                if (msg == null && (product.ProvinceId == null || product.ProvinceId <= 0))
                {
                    msg = "请选择省份！";
                }

                if (msg == null && (product.CityId == null || product.CityId <= 0))
                {
                    msg = "请选择城市！";
                }

                if (msg == null)
                {
                    product.CreateTime = DateTime.Now;
                    ProductCategory category = OperateContext.Current.BLLSession.IProductCategoryBLL.GetListBy(m => m.ID == product.CategoryID).FirstOrDefault();

                    if (category != null)
                    {
                        product.CategoryName = category.CategoryName;
                    }

                    int result = 0;

                    if (product.ID > 0)
                    {
                        result = OperateContext.Current.BLLSession.IProductInfoBLL.Modify(product,
                            "ProductName", "CategoryID", "CategoryName", "Price", "Sort", "ProvinceId", "CityId","ProductImg");
                    }
                    else
                    {
                        result = OperateContext.Current.BLLSession.IProductInfoBLL.Add(product);
                    }

                    if (result == 1)
                    {
                        status = "ok";
                        msg = "保存成功！";
                    }
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 设置商品首页展示图片
        /// add by fruitchan
        /// 2016-12-19 20:48:19
        /// </summary>
        /// <param name="product">商品信息</param>
        /// <returns>设置结果</returns>
        public ActionResult SetProductImg(ProductInfo product)
        {
            string status = "fail";
            string msg = "保存失败！";

            if (product != null)
            {
                int result = OperateContext.Current.BLLSession.IProductInfoBLL.Modify(product, "IsHomeShow", "HomeImg");

                if (result == 1)
                {
                    status = "ok";
                    msg = "保存成功！";
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        #endregion

    }
}

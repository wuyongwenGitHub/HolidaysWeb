using Holidays.Common;
using Holidays.Common.Attributes;
using Holidays.Model.Entites;
using Holidays.Model.FormatModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Holidays.Web.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        //
        // GET: /Admin/Shop/
        [ValidMenuPerm]

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 店铺列表
        /// </summary>
        /// <param name="shopName"></param>
        /// <param name="page"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public ActionResult GetShopList(string shopName, int page = 1, int row = 10)
        {
            var whereString = PredicateBuilder.True<ShopInfo>();

            #region 查询条件
            Expression<Func<ShopInfo, bool>> stateExpression = h => h.IsDel == 0;
            whereString = whereString.And(stateExpression);
            if (!string.IsNullOrWhiteSpace(shopName))
            {
                Expression<Func<ShopInfo, bool>> shopNameExpression = h => h.ShopName.Contains(shopName) || h.UserName.Contains(shopName) || h.SpotName.Contains(shopName);
                whereString = whereString.And(shopNameExpression);
            }
            #endregion

            int rowCount = 0;
            IList<ShopInfo> shopInfoList = OperateContext.Current.BLLSession.IShopInfoBLL.GetPagedList(page, row, ref rowCount, whereString, m => m.SortBy);
            PageModel<ShopInfo> model = PageModel<ShopInfo>.GetPageModel(shopInfoList, page, rowCount, row);

            return new JsonResult() { Data = model };
        }

        public ActionResult QueryShopByID(long id)
        {
            return OperateContext.Current.RedirectAjax("ok", null, OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.ID == id).FirstOrDefault(), null);
        }

        public ActionResult SaveShop(ShopInfo shopInfo)
        {
            string status = "fail";
            string msg = "保存失败！";
            if (shopInfo != null)
            {
                msg = Validate.ValidateString(new CustomValidate()
                {
                    FieldName = "店铺名称",
                    FieldValue = shopInfo.ShopName,
                    IsRequired = true,
                    MaxLength = 100
                }, new CustomValidate()
                {
                    FieldName = "店铺图片",
                    FieldValue = shopInfo.ShopImgs,
                    IsRequired = true
                }
                , new CustomValidate()
                {
                    FieldName = "景点名称",
                    FieldValue = shopInfo.SpotId.HasValue ? shopInfo.SpotId.ToString() : null,
                    IsRequired = true
                }, new CustomValidate()
                {
                    FieldName = "起订价格",
                    FieldValue = shopInfo.StartPrice.HasValue ? shopInfo.StartPrice.ToString() : null,
                    IsRequired = true
                });
                if (msg == null)
                {
                    shopInfo.CreateOn = shopInfo.CreateOn.HasValue ? shopInfo.CreateOn : DateTime.Now;
                    var spotObj = OperateContext.Current.BLLSession.ISpotInfoBLL.GetListBy(h => h.ID == shopInfo.SpotId).FirstOrDefault();
                    if (spotObj != null)
                    {
                        shopInfo.SpotName = spotObj.ScenicSpotName;
                    }
                    var shopCategory = OperateContext.Current.BLLSession.IShopCategoryBLL.GetListBy(h => h.ID == shopInfo.ShopType).FirstOrDefault();
                    if (shopCategory != null)
                    {
                        shopInfo.ShopTypeName = shopCategory.CategoryName;
                        shopInfo.DecorativeStyle = shopCategory.CategoryName;
                    }
                    var userOjb = OperateContext.Current.BLLSession.IUserInfoExtViewBLL.GetListBy(h => h.PhoneNo == shopInfo.PhoneNo || h.PhoneNo2 == shopInfo.PhoneNo).FirstOrDefault();
                    if (userOjb == null)
                    {
                        msg = "未找到电话号码对应的房东！";
                        status = "fail";
                    }
                    else
                    {
                        shopInfo.UserId = userOjb.ID;
                        shopInfo.UserName = userOjb.Username;
                        if (shopInfo.ID == 0)
                        {
                            shopInfo.IsCheck = 1;
                            shopInfo.State = 1;
                            shopInfo.IsDel = 0;
                        }
                        int result = shopInfo.ID > 0 ? OperateContext.Current.BLLSession.IShopInfoBLL.Modify(shopInfo) : OperateContext.Current.BLLSession.IShopInfoBLL.Add(shopInfo);
                        if (result == 1)
                        {
                            status = "ok";
                            msg = "保存成功！";
                        }
                    }

                }
            }
            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        public ActionResult DelShopByID(long id)
        {
            int result = 0;
            var shop = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.ID == id).FirstOrDefault();
            if (shop != null)
            {
                shop.State = -1;
                shop.IsDel = 1;
                result = OperateContext.Current.BLLSession.IShopInfoBLL.Modify(shop);
            }
            return OperateContext.Current.RedirectAjax(result == 1 ? "ok" : "fail", result == 1 ? "删除成功！" : "删除失败！", null, null);
        }

        /// <summary>
        /// 修改店铺状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult SaveShopState(long id, int state)
        {
            string status = "fail";
            string msg = "保存失败！";
            var shop = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.ID == id).FirstOrDefault();
            if (shop != null)
            {
                shop.State = state;
                var result = OperateContext.Current.BLLSession.IShopInfoBLL.Modify(shop);
                if (result == 1)
                {
                    status = "ok";
                    msg = "保存成功！";
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        #region 店铺审核
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ValidMenuPerm]

        public ActionResult ShopCheckManageView()
        {
            ViewBag.ShopCategoryList = OperateContext.Current.BLLSession.IShopCategoryBLL.GetListBy(h => h.ID > 0, h => h.SortBy).ToList();
            return View();
        }
        [ValidMenuPerm]

        public ActionResult ShopView(long? id)
        {
            ShopInfo shopInfo = null;
            if (id.HasValue)
            {
                shopInfo = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.ID == id).FirstOrDefault();
            }

            if (shopInfo == null)
            {
                shopInfo = new ShopInfo();
            }
            return View(shopInfo);
        }

        [HttpPost]
        public ActionResult GetShopCheckList(string keywords, int? shopType, int page = 1, int limit = 10)
        {
            var result = new ResponseData<ShopInfoCertificateView> { code = 0, msg = "", count = 0, data = new List<ShopInfoCertificateView> { } };
            var whereLambda = PredicateBuilder.True<ShopInfoCertificateView>();
            #region 查询条件
            Expression<Func<ShopInfoCertificateView, bool>> isCheckExpression = h => !h.IsCheck.HasValue || h.IsCheck == 0;
            whereLambda = whereLambda.And(isCheckExpression);

            if (!string.IsNullOrWhiteSpace(keywords))
            {
                Expression<Func<ShopInfoCertificateView, bool>> keywordExpresion = h => h.ShopName.Contains(keywords) || h.SpotName.Contains(keywords) || h.UserName.Contains(keywords);
                whereLambda = whereLambda.And(keywordExpresion);
            }
            if (shopType.HasValue && shopType.Value != -1)
            {
                Expression<Func<ShopInfoCertificateView, bool>> shopTypeExpresion = h => h.ShopType == shopType.Value;
                whereLambda = whereLambda.And(shopTypeExpresion);
            }

            #endregion
            int rowCount = 0;
            result.data = OperateContext.Current.BLLSession.IShopInfoCertificateViewBLL.GetPagedList(page, limit, ref rowCount, whereLambda, h => h.CreateOn);
            result.count = rowCount;

            return new JsonResult { Data = result };
        }

        [HttpPost]
        public ActionResult SaveCheckState(long id, int state, string failReason)
        {
            string status = "fail";
            string msg = "操作失败！";
            var sic = OperateContext.Current.BLLSession.IShopInfoCertificateBLL.GetListBy(h => h.ID == id).FirstOrDefault();
            if (sic != null)
            {
                sic.State = state;
                sic.FailReason = failReason;
                //审核信息
                int result = OperateContext.Current.BLLSession.IShopInfoCertificateBLL.Modify(sic);
                if (result == 1)
                {
                    OperateContext.Current.BLLSession.IShopInfoBLL.Modify(new ShopInfo()
                    {
                        ID = sic.ShopId,
                        IsCheck = state
                    }, "IsCheck");
                }
                if (result == 1)
                {
                    status = "ok";
                    msg = "操作成功！";
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        #endregion

        #region 店铺分类
        [ValidMenuPerm]

        public ActionResult ShopCategoryView()
        {
            return View();
        }

        public ActionResult GetShopCategoryList()
        {
            IList<ShopCategory> shopCategoryList = OperateContext.Current.BLLSession.IShopCategoryBLL.GetListBy(h => h.ID > 0, h => h.SortBy).ToList();

            return OperateContext.Current.RedirectAjax("ok", null, shopCategoryList, null);
        }

        public ActionResult QueryShopCategoryByID(long id)
        {
            return OperateContext.Current.RedirectAjax("ok", null, OperateContext.Current.BLLSession.IShopCategoryBLL.GetListBy(h => h.ID == id).FirstOrDefault(), null);
        }

        public ActionResult DelShopCategoryByID(long id)
        {
            int result = OperateContext.Current.BLLSession.IShopCategoryBLL.DelBy(h => h.ID == id);

            return OperateContext.Current.RedirectAjax(result == 1 ? "ok" : "fail", result == 1 ? "删除成功！" : "删除失败！", null, null);
        }

        public ActionResult SaveShopCategory(ShopCategory shopCategory)
        {
            string status = "fail";
            string msg = "保存失败！";
            if (shopCategory != null)
            {
                msg = Validate.ValidateString(new CustomValidate
                {
                    FieldName = "分类名称",
                    FieldValue = shopCategory.CategoryName,
                    IsRequired = true,
                    MaxLength = 100
                });

                if (msg == null)
                {
                    shopCategory.CreateTime = shopCategory.ID > 0 ? shopCategory.CreateTime : DateTime.Now;
                    int result = shopCategory.ID > 0 ? OperateContext.Current.BLLSession.IShopCategoryBLL.Modify(shopCategory) : OperateContext.Current.BLLSession.IShopCategoryBLL.Add(shopCategory);
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
        public ActionResult SavePriceAuto(string PriceAutos)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            List<ShopToDayPrice> ShopToDayPrices = Serializer.Deserialize<List<ShopToDayPrice>>(PriceAutos);
            //json字符串转为对象, 反序列化
            string status = "fail";
            string msg = "保存失败！";
            int statu = 0;
            ShopToDayPrices.ForEach(x =>
            {
                //查看当前日期是否存在
                ShopToDayPrice ShopToDayPrice = OperateContext.Current.BLLSession.IShopToDaySetBll.GetListBy(h => h.date == x.date&&h.ShopId==x.ShopId).FirstOrDefault<ShopToDayPrice>();
                int result = 0;
                if (ShopToDayPrice!=null&&ShopToDayPrice.Id > 0) {
                     x.Id = ShopToDayPrice.Id;
                    result = OperateContext.Current.BLLSession.IShopToDaySetBll.Modify(x);
                }
                else
                {
                    result=OperateContext.Current.BLLSession.IShopToDaySetBll.Add(x);
                }
                if (result != 1)
                {
                    statu = 1;
                    return;
                }

            });
            if (statu!=1)
            {
                status = "ok";
                msg = "保存成功！";
            }
            return OperateContext.Current.RedirectAjax(status, msg, null, null);

        }
        public ActionResult GetPriceAutoByShopID(int ShopId)
        {
            List<ShopToDayPrice> shopToDayPricelist = OperateContext.Current.BLLSession.IShopToDaySetBll.GetListBy(h => h.ShopId == ShopId).ToList<ShopToDayPrice>();
            //List<TodayPrice> a = OperateContext.Current.CustomSql<TodayPrice>();
            return OperateContext.Current.RedirectAjax("ok", null, shopToDayPricelist, null);
        }
            
    }
}

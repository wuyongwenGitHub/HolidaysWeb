using Holidays.Common;
using Holidays.Common.Attributes;
using Holidays.Model.Entites;
using Holidays.Model.FormatModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Holidays.Web.Areas.Admin.Controllers
{
    public class HouseResourceController : Controller
    {
        //
        // GET: /Admin/HouseResource/

        /// <summary>
        /// 房屋列表管理页面
        /// add by fruitchan
        /// 2016-11-22 20:43:04
        /// </summary>
        /// <returns>View</returns>
        

        public ActionResult HouseListView()
        {
            // 房东审核待办数
            ViewBag.CertificateTotal = OperateContext.Current.BLLSession.IUserInfoCertificateBLL.CountRow(m => m.State == 0);
            return View();
        }

        /// <summary>
        /// 分页查询房源信息
        /// add by fruitchan
        /// 2016-12-11 21:35:10
        /// </summary>
        /// <param name="houseTitle">房源标题</param>
        /// <param name="priceMin">房价（最小）</param>
        /// <param name="priceMax">房价（最大）</param>
        /// <param name="page">当前页面</param>
        /// <param name="row">每页显示数据条数</param>
        /// <returns>房源列表数据</returns>
        public ActionResult GetHouseList(string houseTitle, decimal? priceMin, decimal? priceMax, int page = 1, int row = 10)
        {
            var whereLambda = PredicateBuilder.True<HouseInfo>();

            #region 查询条件

            // 状态
            //Expression<Func<HouseInfo, bool>> userTypeExpression = p => p.State == 0;
            //whereLambda = whereLambda.And(userTypeExpression);

            if (!String.IsNullOrWhiteSpace(houseTitle))
            {
                // 房源标题
                Expression<Func<HouseInfo, bool>> houseTitleExpression = p => p.HouseTitle.Contains(houseTitle);
                whereLambda = whereLambda.And(houseTitleExpression);
            }

            if (priceMin.HasValue)
            {
                // 价格（最低）
                Expression<Func<HouseInfo, bool>> priceExpression = p => p.Price >= priceMin.Value;
                whereLambda = whereLambda.And(priceExpression);
            }

            if (priceMax.HasValue)
            {
                // 价格（最高）
                Expression<Func<HouseInfo, bool>> priceExpression = p => p.Price <= priceMax.Value;
                whereLambda = whereLambda.And(priceExpression);
            }

            #endregion

            int rowCount = 0;
            IList<HouseInfo> houseList = OperateContext.Current.BLLSession.IHouseInfoBLL.GetPagedList(page, row, ref rowCount, whereLambda, m => m.CreateTime, false);

            PageModel<HouseInfo> model = PageModel<HouseInfo>.GetPageModel(houseList, page, rowCount, row);

            return new JsonResult() { Data = model };
        }

        /// <summary>
        /// 更改房源状态
        /// add by fruitchan
        /// 2016-12-11 21:37:51
        /// </summary>
        /// <param name="id">房源编号</param>
        /// <param name="state">状态</param>
        /// <returns>更改结果</returns>
        public ActionResult UpdateHouseStateByID(long id, int state)
        {
            string status = "fail";
            string msg = "操作失败！";

            HouseInfo house = OperateContext.Current.BLLSession.IHouseInfoBLL.GetListBy(m => m.ID == id).FirstOrDefault();

            if (house != null)
            {
                house.State = state;
                int result = OperateContext.Current.BLLSession.IHouseInfoBLL.Modify(house);

                if (result == 1)
                {
                    status = "ok";
                    msg = "操作成功！";
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 房屋评价管理页面
        /// </summary>
        /// <returns>View</returns>
        

        public ActionResult HouseEvaluateListView()
        {
            // 房东审核待办数
            ViewBag.CertificateTotal = OperateContext.Current.BLLSession.IUserInfoCertificateBLL.CountRow(m => m.State == 0);
            return View();
        }

        /// <summary>
        /// 分页查询房源评价
        /// add by fruitchan
        /// 2016-12-11 20:54:51
        /// </summary>
        /// <param name="houseTitle">房源标题</param>
        /// <param name="username">评价人</param>
        /// <param name="content">评价内容</param>
        /// <param name="page">当前页码</param>
        /// <param name="row">每页显示数据条数</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetEvaluateList(string houseTitle, string username, string content, int page = 1, int row = 10)
        {
            var whereLambda = PredicateBuilder.True<HouseEvaluate>();

            #region 查询条件

            // 状态
            Expression<Func<HouseEvaluate, bool>> userTypeExpression = p => p.State == 0;
            whereLambda.And(userTypeExpression);

            if (!String.IsNullOrWhiteSpace(houseTitle))
            {
                // 房源标题
                Expression<Func<HouseEvaluate, bool>> houseTitleExpression = p => p.HouseTitle.Contains(houseTitle);
                whereLambda = whereLambda.And(houseTitleExpression);
            }

            if (!String.IsNullOrWhiteSpace(username))
            {
                // 评价人
                Expression<Func<HouseEvaluate, bool>> usernameExpression = p => p.Username.Contains(username);
                whereLambda = whereLambda.And(usernameExpression);
            }

            if (!String.IsNullOrWhiteSpace(content))
            {
                // 评价内容
                Expression<Func<HouseEvaluate, bool>> contentExpression = p => p.EvaluateContent.Contains(content);
                whereLambda = whereLambda.And(contentExpression);
            }

            #endregion

            int rowCount = 0;
            IList<HouseEvaluate> houseEvaluateList = OperateContext.Current.BLLSession.IHouseEvaluateBLL.GetPagedList(page, row, ref rowCount, whereLambda, m => m.CreateTime, false);

            PageModel<HouseEvaluate> model = PageModel<HouseEvaluate>.GetPageModel(houseEvaluateList, page, rowCount, row);

            return new JsonResult() { Data = model };
        }

        /// <summary>
        /// 根据编号删除评价信息
        /// add by fruitchan
        /// 2016-12-11 20:55:33
        /// </summary>
        /// <param name="id">评价编号</param>
        /// <returns>删除是否成功</returns>
        [HttpPost]
        public ActionResult DelEvaluateByID(long id)
        {
            string status = "fail";
            string msg = "删除失败！";

            HouseEvaluate houseEvaluate = OperateContext.Current.BLLSession.IHouseEvaluateBLL.GetListBy(m => m.ID == id).FirstOrDefault();

            if (houseEvaluate != null)
            {
                houseEvaluate.State = -1;
                int result = OperateContext.Current.BLLSession.IHouseEvaluateBLL.Modify(houseEvaluate);

                if (result == 1)
                {
                    status = "ok";
                    msg = "删除成功！";
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// add by fishby
        /// 2018-03-31
        /// </summary>
        /// <returns></returns>
        

        public ActionResult AddHouseView(long? id)
        {
            HouseInfo houseInfo = new HouseInfo();
            ViewBag.ShopList = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.ID > 0, h => h.SortBy).ToList();
            ViewBag.LandorList = OperateContext.Current.BLLSession.IUserInfoExtViewBLL.GetListBy(h => h.ID > 0, h => h.CreateTime, false);
            if (id.HasValue)
            {
                // 根据编号查询房源信息
                houseInfo = OperateContext.Current.BLLSession.IHouseInfoBLL.GetListBy(m => m.ID == id.Value && m.ID == id).FirstOrDefault();
                if (houseInfo != null)
                {
                    // 房源图片
                    houseInfo.HouseImgList = OperateContext.Current.BLLSession.IHouseImgBLL.GetListBy(m => m.HouseInfoID == id.Value);

                    if (houseInfo.HouseImgList != null)
                    {
                        houseInfo.JsonHouseImgList = JsonConvert.SerializeObject(houseInfo.HouseImgList);
                    }
                }
                else
                {
                    houseInfo = new HouseInfo();
                }
            }
            // 配套设施
            ViewBag.HouseConfigureList = OperateContext.Current.BLLSession.IHouseConfigureBLL.GetListBy(m => m.ID > 0, m => m.Sort, true);
            return View(houseInfo);
        }

        /// <summary>
        /// 保存房源信息
        /// add by fishby
        /// 2018-03-31 20:28:28
        /// </summary>
        /// <param name="houseInfoModel">房源实体信息</param>
        /// <returns>保存结果</returns>
        [HttpPost]
        public ActionResult SaveHouseInfo(HouseInfo houseInfoModel,long? landlorId)
        {
            string status = "fail";
            string msg = null;
            UserInfoView loginUserInfo = OperateContext.Current.UserInfo;

            if (houseInfoModel != null)
            {
                #region 校验数据

                // TODO:校验数据
                #endregion
                //if(!landlorId.HasValue)
                //{
                //    msg = "请选择房东！";
                //    return OperateContext.Current.RedirectAjax(status, msg, null, null);
                //}
                //获取房东信息
                //var userOjb = OperateContext.Current.BLLSession.IUserInfoExtViewBLL.GetListBy(h => h.ID == landlorId).FirstOrDefault();
                //if (userOjb == null)
                //{
                //    msg = "未找到您选择的房东！";
                //    return OperateContext.Current.RedirectAjax(status, msg, null, null);
                //}
                if (!houseInfoModel.ShopId.HasValue)
                {
                    msg = "请选择店铺";
                    return OperateContext.Current.RedirectAjax(status, msg, null, null);
                }
                var shop = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.ID == houseInfoModel.ShopId).FirstOrDefault();
                if(shop == null)
                {
                    msg = "未找到您选择的店铺！";
                    return OperateContext.Current.RedirectAjax(status, msg, null, null);
                }
                houseInfoModel.ShopId = shop.ID;
                houseInfoModel.ShopName = shop.ShopName;
                houseInfoModel.DecorativeStyle = (int)shop.ShopType.Value;
                houseInfoModel.Address = shop.Locations;
                houseInfoModel.Rules = shop.Rules;
                houseInfoModel.ChargesNotes = shop.ChargesNotes;
                houseInfoModel.About = shop.About;
                houseInfoModel.UserInfoID = shop.UserId.Value;
                houseInfoModel.IsBack = houseInfoModel.IsBack ?? 0;
                houseInfoModel.IsCancel = houseInfoModel.IsCancel ?? 0;
                // 房源图片
                if (!String.IsNullOrWhiteSpace(houseInfoModel.JsonHouseImgList))
                {
                    houseInfoModel.HouseImgList = JsonConvert.DeserializeObject<IList<HouseImg>>(houseInfoModel.JsonHouseImgList);
                }

                int result = 0;

                // 房源基本信息
                if (houseInfoModel.ID > 0)
                {
                    // 修改
                    HouseInfo houseInfo = OperateContext.Current.BLLSession.IHouseInfoBLL.GetListBy(m => m.ID == houseInfoModel.ID).FirstOrDefault();

                    if (houseInfo != null)
                    {
                        houseInfo.HouseTitle = houseInfoModel.HouseTitle;
                        houseInfo.DecorativeStyle = houseInfoModel.DecorativeStyle;
                        houseInfo.LeaseType = houseInfoModel.LeaseType;
                        houseInfo.BedroomNum = houseInfoModel.BedroomNum;
                        houseInfo.LivingRoomNum = houseInfoModel.LivingRoomNum;
                        houseInfo.ToiletNum = houseInfoModel.ToiletNum;
                        houseInfo.Price = houseInfoModel.Price;
                        houseInfo.IsTrusteeship = houseInfoModel.IsTrusteeship;
                        houseInfo.IsSell = houseInfoModel.IsSell;
                        houseInfo.SellPrice = houseInfoModel.SellPrice;
                        houseInfo.IsCooking = houseInfoModel.IsCooking;
                        houseInfo.IsPet = houseInfoModel.IsPet;
                        houseInfo.StayPersonNum = houseInfoModel.StayPersonNum;
                        houseInfo.HouseSize = houseInfoModel.HouseSize;
                        houseInfo.Facilities = houseInfoModel.Facilities;
                        houseInfo.Address = houseInfoModel.Address;
                        houseInfo.About = houseInfoModel.About;
                        houseInfo.Rules = houseInfoModel.Rules;
                        houseInfo.ChargesNotes = houseInfoModel.ChargesNotes;
                        houseInfo.BaseInfo = houseInfoModel.BaseInfo;
                        houseInfo.IsReals = houseInfoModel.IsReals;
                        houseInfo.ProvinceId = houseInfoModel.ProvinceId;
                        houseInfo.CityId = houseInfoModel.CityId;
                        houseInfo.CountyId = houseInfoModel.CountyId;
                        houseInfo.UserInfoID = houseInfoModel.UserInfoID;
                        houseInfo.ShopId = shop.ID;
                        houseInfo.ShopName = shop.ShopName;
                        houseInfo.IsBack = houseInfoModel.IsBack;
                        houseInfo.IsCancel = houseInfoModel.IsCancel;

                        result = OperateContext.Current.BLLSession.IHouseInfoBLL.Modify(houseInfo);
                    }
                    else
                    {
                        msg = "系统未查询到房源信息！";
                    }
                }
                else
                {
                    // 新增
                    houseInfoModel.State = 0;
                    houseInfoModel.CreateTime = DateTime.Now;
                    houseInfoModel.IsEmpty = 0;
                    result = OperateContext.Current.BLLSession.IHouseInfoBLL.Add(houseInfoModel);
                }

                // 房源图片
                if (houseInfoModel.HouseImgList != null && houseInfoModel.HouseImgList.Count > 0)
                {
                    OperateContext.Current.BLLSession.IHouseImgBLL.DelBy(m => m.HouseInfoID == houseInfoModel.ID);

                    foreach (var houseImg in houseInfoModel.HouseImgList)
                    {
                        houseImg.HouseInfoID = houseInfoModel.ID;
                        houseImg.CreateTime = DateTime.Now;

                        OperateContext.Current.BLLSession.IHouseImgBLL.Add(houseImg);
                    }
                }

                if (result == 1)
                {
                    status = "ok";
                    msg = "保存成功！";
                }
            }
            else
            {
                msg = "获取数据失败！";
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }
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
                ShopToDayPrice ShopToDayPrice = OperateContext.Current.BLLSession.IShopToDaySetBll.GetListBy(h => h.date == x.date && h.ShopId == x.ShopId).FirstOrDefault<ShopToDayPrice>();
                int result = 0;
                if (ShopToDayPrice != null && ShopToDayPrice.Id > 0)
                {
                    x.Id = ShopToDayPrice.Id;
                    result = OperateContext.Current.BLLSession.IShopToDaySetBll.Modify(x);
                }
                else
                {
                    result = OperateContext.Current.BLLSession.IShopToDaySetBll.Add(x);
                }
                if (result != 1)
                {
                    statu = 1;
                    return;
                }

            });
            if (statu != 1)
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

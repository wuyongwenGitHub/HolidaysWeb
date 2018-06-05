using Holidays.Common;
using Holidays.Model.Entites;
using Holidays.Model.FormatModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Controllers
{
    public class HouseController : BaseController
    {
        //
        // GET: /House/

        /// <summary>
        /// 销售房列表
        /// add by fruitchan
        /// 2016-12-18 23:30:26
        /// </summary>
        /// <param name="keywords">关键字（房源标题）</param>
        /// <returns>View</returns>
        [ValidateInput(false)]
        public ActionResult SellHouseList(string keywords)
        {
            ViewBag.Keywords = HttpUtility.HtmlEncode(keywords);
            return View();
        }

        /// <summary>
        /// 租房列表
        /// add by fruitchan
        /// 2016-12-18 23:30:43
        /// </summary>
        /// <param name="keywords">关键字（房源标题）</param>
        /// <returns>View</returns>
        [ValidateInput(false)]
        public ActionResult RentHouseList(string keywords)
        {
            ViewBag.Keywords = HttpUtility.HtmlEncode(keywords);
            return View();
        }

        /// <summary>
        /// 分页查询租房信息
        /// add by fruitchan
        /// 2016-12-26 21:00:23
        /// </summary>
        /// <param name="keywords">关键字（房源标题）</param>
        /// <param name="minPrice">最小价格</param>
        /// <param name="maxPrice">最大价格</param>
        /// <param name="bedroomNum">室</param>
        /// <param name="livingRoomNum">厅</param>
        /// <param name="toiletNum">卫</param>
        /// <param name="decorativeStyle">装修风格</param>
        /// <param name="bySort">排序方式</param>
        /// <param name="page">当前页码</param>
        /// <param name="row">每页显示数据条数</param>
        /// <returns>租房列表数据</returns>
        [HttpPost]
        public ActionResult GetRentHouseList(string keywords, decimal? minPrice, decimal? maxPrice, int? bedroomNum, int? livingRoomNum, int? toiletNum, int? decorativeStyle, int bySort, int page = 1, int row = 12)
        {
            var whereLambda = PredicateBuilder.True<HouseInfoView>();

            #region 查询条件

            // 租、售类型
            //Expression<Func<HouseInfoView, bool>> isSellExpression = p => p.IsSell == false;
            //whereLambda = whereLambda.And(isSellExpression);

            // 状态
            Expression<Func<HouseInfoView, bool>> stateExpression = p => p.State == 0;
            whereLambda = whereLambda.And(stateExpression);

            // 城市
            Expression<Func<HouseInfoView, bool>> cityIdExpression = p => p.CityId == OperateContext.Current.CurrentCity.Id;
            whereLambda = whereLambda.And(cityIdExpression);

            if (!String.IsNullOrWhiteSpace(keywords))
            {
                long shopId = -1;
                var shop = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.ShopName.Contains(keywords)).FirstOrDefault();
                if (shop != null)
                {
                    shopId = shop.ID;
                }
                // 房源标题
                Expression<Func<HouseInfoView, bool>> keywordsExpression = p => p.ShopId == shopId;
                whereLambda = whereLambda.And(keywordsExpression);
            }

            if (minPrice.HasValue && minPrice.Value > 0)
            {
                // 最小价格
                Expression<Func<HouseInfoView, bool>> minPriceExpression = p => p.Price >= minPrice.Value;
                whereLambda = whereLambda.And(minPriceExpression);
            }

            if (maxPrice.HasValue && maxPrice.Value > 0)
            {
                // 最大价格
                Expression<Func<HouseInfoView, bool>> maxPriceExpression = p => p.Price <= maxPrice.Value;
                whereLambda = whereLambda.And(maxPriceExpression);
            }

            if (bedroomNum.HasValue && bedroomNum.Value > 0)
            {
                // 室
                Expression<Func<HouseInfoView, bool>> bedroomNumExpression = p => p.BedroomNum == bedroomNum.Value;
                whereLambda = whereLambda.And(bedroomNumExpression);
            }

            if (livingRoomNum.HasValue && livingRoomNum.Value > 0)
            {
                // 厅
                Expression<Func<HouseInfoView, bool>> livingRoomNumExpression = p => p.LivingRoomNum == livingRoomNum.Value;
                whereLambda = whereLambda.And(livingRoomNumExpression);
            }

            if (toiletNum.HasValue && toiletNum.Value > 0)
            {
                // 卫
                Expression<Func<HouseInfoView, bool>> toiletNumExpression = p => p.ToiletNum == toiletNum.Value;
                whereLambda = whereLambda.And(toiletNumExpression);
            }

            if (decorativeStyle.HasValue && decorativeStyle.Value > 0)
            {
                // 装修风格
                Expression<Func<HouseInfoView, bool>> decorativeStyleExpression = p => p.DecorativeStyle == decorativeStyle.Value;
                whereLambda = whereLambda.And(decorativeStyleExpression);
            }

            #endregion

            int rowCount = 0;
            IList<HouseInfoView> houseList = null;

            if (bySort == 1)
            {
                // 价格排序
                houseList = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetPagedList(page, row, ref rowCount, whereLambda, m => m.Price, true);
            }
            else if (bySort == 2)
            {
                // 评论数排序
                houseList = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetPagedList(page, row, ref rowCount, whereLambda, m => m.EvaluateNum, false);
            }
            else if (bySort == 3)
            {
                // 预定数排序
                houseList = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetPagedList(page, row, ref rowCount, whereLambda, m => m.BuyNum, false);
            }
            else
            {
                // 默认排序
                houseList = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetPagedList(page, row, ref rowCount, whereLambda, m => m.EvaluateAvgScore | m.BuyNum, false);
            }

            if (houseList != null && houseList.Count > 0)
            {
                // 房源图片
                foreach (var house in houseList)
                {
                    house.HouseImgList = OperateContext.Current.BLLSession.IHouseImgBLL.GetListBy(m => m.HouseInfoID == house.ID, m => m.CreateTime, true);
                }
            }

            PageModel<HouseInfoView> model = PageModel<HouseInfoView>.GetPageModel(houseList, page, rowCount, row);

            return new JsonResult() { Data = model };
        }

        /// <summary>
        /// 分页查询售房信息
        /// add by fruitchan
        /// 2016-12-26 21:00:23
        /// </summary>
        /// <param name="keywords">关键字（房源标题）</param>
        /// <param name="minPrice">最小价格</param>
        /// <param name="maxPrice">最大价格</param>
        /// <param name="bedroomNum">室</param>
        /// <param name="livingRoomNum">厅</param>
        /// <param name="toiletNum">卫</param>
        /// <param name="decorativeStyle">装修风格</param>
        /// <param name="bySort">排序方式</param>
        /// <param name="page">当前页码</param>
        /// <param name="row">每页显示数据条数</param>
        /// <returns>售房列表数据</returns>
        [HttpPost]
        public ActionResult GetSellHouseList(string keywords, decimal? minPrice, decimal? maxPrice, int? bedroomNum, int? livingRoomNum, int? toiletNum, int? decorativeStyle, int bySort, int page = 1, int row = 12)
        {
            var whereLambda = PredicateBuilder.True<HouseInfoView>();

            #region 查询条件

            // 租、售类型
            Expression<Func<HouseInfoView, bool>> isSellExpression = p => p.IsSell == true;
            whereLambda = whereLambda.And(isSellExpression);

            // 状态
            Expression<Func<HouseInfoView, bool>> stateExpression = p => p.State == 0;
            whereLambda = whereLambda.And(stateExpression);

            // 城市
            Expression<Func<HouseInfoView, bool>> cityIdExpression = p => p.CityId == OperateContext.Current.CurrentCity.Id;
            whereLambda = whereLambda.And(cityIdExpression);

            if (!String.IsNullOrWhiteSpace(keywords))
            {
                // 房源标题
                Expression<Func<HouseInfoView, bool>> keywordsExpression = p => p.HouseTitle.Contains(keywords);
                whereLambda = whereLambda.And(keywordsExpression);
            }

            if (minPrice.HasValue && minPrice.Value > 0)
            {
                // 最小价格
                Expression<Func<HouseInfoView, bool>> minPriceExpression = p => p.SellPrice >= minPrice.Value;
                whereLambda = whereLambda.And(minPriceExpression);
            }

            if (maxPrice.HasValue && maxPrice.Value > 0)
            {
                // 最大价格
                Expression<Func<HouseInfoView, bool>> maxPriceExpression = p => p.SellPrice <= maxPrice.Value;
                whereLambda = whereLambda.And(maxPriceExpression);
            }

            if (bedroomNum.HasValue && bedroomNum.Value > 0)
            {
                // 室
                Expression<Func<HouseInfoView, bool>> bedroomNumExpression = p => p.BedroomNum == bedroomNum.Value;
                whereLambda = whereLambda.And(bedroomNumExpression);
            }

            if (livingRoomNum.HasValue && livingRoomNum.Value > 0)
            {
                // 厅
                Expression<Func<HouseInfoView, bool>> livingRoomNumExpression = p => p.LivingRoomNum == livingRoomNum.Value;
                whereLambda = whereLambda.And(livingRoomNumExpression);
            }

            if (toiletNum.HasValue && toiletNum.Value > 0)
            {
                // 卫
                Expression<Func<HouseInfoView, bool>> toiletNumExpression = p => p.ToiletNum == toiletNum.Value;
                whereLambda = whereLambda.And(toiletNumExpression);
            }

            if (decorativeStyle.HasValue && decorativeStyle.Value > 0)
            {
                // 装修风格
                Expression<Func<HouseInfoView, bool>> decorativeStyleExpression = p => p.DecorativeStyle == decorativeStyle.Value;
                whereLambda = whereLambda.And(decorativeStyleExpression);
            }

            #endregion

            int rowCount = 0;
            IList<HouseInfoView> houseList = null;

            if (bySort == 1)
            {
                // 价格排序
                houseList = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetPagedList(page, row, ref rowCount, whereLambda, m => m.SellPrice, true);
            }
            else if (bySort == 2)
            {
                // 评论数排序
                houseList = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetPagedList(page, row, ref rowCount, whereLambda, m => m.EvaluateNum, false);
            }
            else if (bySort == 3)
            {
                // 预定数排序
                houseList = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetPagedList(page, row, ref rowCount, whereLambda, m => m.BuyNum, false);
            }
            else
            {
                // 默认排序
                houseList = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetPagedList(page, row, ref rowCount, whereLambda, m => m.EvaluateAvgScore | m.BuyNum, false);
            }

            if (houseList != null && houseList.Count > 0)
            {
                // 房源图片
                foreach (var house in houseList)
                {
                    house.HouseImgList = OperateContext.Current.BLLSession.IHouseImgBLL.GetListBy(m => m.HouseInfoID == house.ID, m => m.CreateTime, true);
                }
            }

            PageModel<HouseInfoView> model = PageModel<HouseInfoView>.GetPageModel(houseList, page, rowCount, row);

            return new JsonResult() { Data = model };
        }

        /// <summary>
        /// 房屋详情页面
        /// add by fruitchan
        /// 2016-12-20 23:41:18
        /// </summary>
        /// <param name="id">房源编号</param>
        /// <returns>View</returns>
        public ActionResult HouseDetails(int? id)
        {
            HouseInfoView houseInfo = null;
            UserInfoView loginUserInfo = OperateContext.Current.UserInfo;

            if (id.HasValue)
            {
                houseInfo = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetListBy(m => m.ID == id && m.State == 0).FirstOrDefault();

                if (houseInfo != null)
                {
                    houseInfo.HouseImgList = OperateContext.Current.BLLSession.IHouseImgBLL.GetListBy(m => m.HouseInfoID == houseInfo.ID, m => m.CreateTime, true);

                    if (loginUserInfo != null)
                    {
                        ViewBag.IsFavorite = OperateContext.Current.BLLSession.IUserFavoriteBLL.IsExist(m => m.HouseInfoID == houseInfo.ID && m.UserInfoID == loginUserInfo.ID);
                    }
                    var sumScore = 0;
                    if (houseInfo.EvaluateNum > 0)
                    {
                        // 房源评价分
                        double cleanScoreTotal = OperateContext.Current.BLLSession.IHouseEvaluateBLL.GetListBy(m => m.HouseInfoID == houseInfo.ID).Sum(m => m.CleanScore);
                        ViewBag.CleanScore = Convert.ToInt32(cleanScoreTotal / houseInfo.EvaluateNum);
                        sumScore += ViewBag.CleanScore;
                        double locationScoreTotal = OperateContext.Current.BLLSession.IHouseEvaluateBLL.GetListBy(m => m.HouseInfoID == houseInfo.ID).Sum(m => m.LocationScore);
                        ViewBag.LocationScore = Convert.ToInt32(locationScoreTotal / houseInfo.EvaluateNum);
                        sumScore += ViewBag.LocationScore;
                        double environmentScoreTotal = OperateContext.Current.BLLSession.IHouseEvaluateBLL.GetListBy(m => m.HouseInfoID == houseInfo.ID).Sum(m => m.EnvironmentScore);
                        ViewBag.EnvironmentScore = Convert.ToInt32(environmentScoreTotal / houseInfo.EvaluateNum);
                        sumScore += ViewBag.EnvironmentScore;
                        double serviceScoreTotal = OperateContext.Current.BLLSession.IHouseEvaluateBLL.GetListBy(m => m.HouseInfoID == houseInfo.ID).Sum(m => m.ServiceScore);
                        ViewBag.ServiceScore = Convert.ToInt32(serviceScoreTotal / houseInfo.EvaluateNum);
                        sumScore += ViewBag.ServiceScore;
                        double performanceScoreScoreTotal = OperateContext.Current.BLLSession.IHouseEvaluateBLL.GetListBy(m => m.HouseInfoID == houseInfo.ID).Sum(m => m.PerformanceScore);
                        ViewBag.PerformanceScore = Convert.ToInt32(performanceScoreScoreTotal / houseInfo.EvaluateNum);
                        sumScore += ViewBag.PerformanceScore;
                    }
                    houseInfo.EvaluateAvgScore = sumScore;
                    #region 省、市、县
                    string cityName = "";

                    if (houseInfo.CityId.HasValue && houseInfo.CityId.Value > 0)
                    {
                        // 城市
                        Region city = OperateContext.Current.BLLSession.IRegionBLL.GetListBy(m => m.Id == houseInfo.CityId.Value).FirstOrDefault();

                        if (city != null)
                        {
                            // 省份
                            Region province = OperateContext.Current.BLLSession.IRegionBLL.GetListBy(m => m.Id == city.ParentId).FirstOrDefault();

                            if (province != null)
                            {
                                cityName = province.Name;
                            }

                            cityName += " " + city.Name;
                        }
                    }

                    if (houseInfo.CountyId.HasValue && houseInfo.CountyId.Value > 0)
                    {
                        // 区县
                        Region county = OperateContext.Current.BLLSession.IRegionBLL.GetListBy(m => m.Id == houseInfo.CountyId.Value).FirstOrDefault();

                        if (county != null)
                        {
                            cityName += " " + county.Name;
                        }
                    }

                    // 已预订订单
                    IList<OrderInfo> orderList = OperateContext.Current.BLLSession.IOrderInfoBLL.GetListBy(m => m.HouseInfoID == houseInfo.ID && (m.State == 0 || m.State == 1 || m.State == 2) && m.EndDate >= DateTime.Now, m => m.StartDate, true);
                    ViewBag.HouseOrderList = orderList;

                    ViewBag.City = cityName;
                    #endregion

                    //
                    var shop = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.ID == houseInfo.ShopId).FirstOrDefault();
                    if (shop != null)
                    {
                        ViewBag.ShopTypeName = shop.ShopTypeName;
                        houseInfo.About = shop.About;
                        houseInfo.ChargesNotes = shop.ChargesNotes;
                        houseInfo.Rules = shop.Rules;
                        houseInfo.Address = shop.Locations;
                    }
                }
            }

            return View(houseInfo);
        }

        /// <summary>
        /// 分页查询房屋评价信息
        /// add by fruitchan
        /// 2017-1-22 21:20:33
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="houseId">房源编号</param>
        /// <returns>房屋评价信息列表</returns>
        [HttpPost]
        public ActionResult GetHouseEvaluateList(int page, int houseId)
        {
            var whereLambda = PredicateBuilder.True<HouseEvaluate>();
            whereLambda = whereLambda.And(m => m.HouseInfoID == houseId);

            int rowCount = 0;
            IList<HouseEvaluate> houseEvaluateList = OperateContext.Current.BLLSession.IHouseEvaluateBLL.GetPagedList(page, 5, ref rowCount, whereLambda, m => m.CreateTime, false);

            if (houseEvaluateList != null && houseEvaluateList.Count > 0)
            {
                // 隐藏手机号
                for (int i = 0; i < houseEvaluateList.Count; i++)
                {
                    if (!String.IsNullOrWhiteSpace(houseEvaluateList[i].UserPhone))
                    {
                        var phone = houseEvaluateList[i].UserPhone;

                        if (phone.Length > 6)
                        {
                            houseEvaluateList[i].UserPhone = phone.Substring(0, 3) + "****" + phone.Substring(phone.Length - 3, 3);
                        }
                    }
                }
            }

            PageModel<HouseEvaluate> model = PageModel<HouseEvaluate>.GetPageModel(houseEvaluateList, page, rowCount, 5);

            return new JsonResult() { Data = model };
        }

        /// <summary>
        /// 添加房屋收藏
        /// add by fruitchan
        /// 2016-12-27 00:39:56
        /// </summary>
        /// <param name="id">房屋编号</param>
        /// <returns>添加结果</returns>
        [HttpPost]
        public ActionResult AddFavorite(int id)
        {
            string status = "fail";
            string msg = "收藏失败！";

            int result = OperateContext.Current.BLLSession.IUserFavoriteBLL.Add(new UserFavorite()
            {
                UserInfoID = OperateContext.Current.UserInfo.ID,
                HouseInfoID = id,
                CreateTime = DateTime.Now
            });

            if (result == 1)
            {
                status = "ok";
                msg = "收藏成功！";
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 取消收藏
        /// add by fruitchan
        /// 2016-12-27 00:42:37
        /// </summary>
        /// <param name="id">房源编号</param>
        /// <returns>取消结果</returns>
        [HttpPost]
        public ActionResult CancelFavorite(int id)
        {
            string status = "fail";
            string msg = "取消收藏失败！";

            int result = OperateContext.Current.BLLSession.IUserFavoriteBLL.DelBy(m => m.UserInfoID == OperateContext.Current.UserInfo.ID && m.HouseInfoID == id);

            if (result > 0)
            {
                status = "ok";
                msg = "取消收藏成功！";
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 房源管理页面
        /// add by fruitchan
        /// 2016-12-20 23:20:06
        /// </summary>
        /// <param name="id">房源编号</param>
        /// <returns>View</returns>
        public ActionResult HouseManager(int? id, int? tab)
        {
            HouseInfo houseInfo = new HouseInfo();

            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                ViewBag.IsRequireRealName = false;
                if (OperateContext.Current.UserInfo != null && OperateContext.Current.UserInfo.IsRealName == 0)
                {
                    ViewBag.IsRequireRealName = true;
                }

                ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;

                ViewBag.TabIndex = tab;

                if (tab.HasValue && tab.Value == 2)
                {
                    IList<HouseInfo> houseList = OperateContext.Current.BLLSession.IHouseInfoBLL.GetListBy(m => m.UserInfoID == OperateContext.Current.UserInfo.ID);

                    // 房源图片
                    if (houseList != null && houseList.Count > 0)
                    {
                        foreach (var house in houseList)
                        {
                            house.HouseImgList = OperateContext.Current.BLLSession.IHouseImgBLL.GetListBy(m => m.HouseInfoID == house.ID, m => m.CreateTime, true);
                        }
                    }

                    ViewBag.HouseList = houseList;
                }
                else
                {
                    if (id.HasValue)
                    {
                        // 根据编号查询房源信息
                        houseInfo = OperateContext.Current.BLLSession.IHouseInfoBLL.GetListBy(m => m.ID == id.Value && m.UserInfoID == OperateContext.Current.UserInfo.ID).FirstOrDefault();
                    }

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

                    // 配套设施
                    ViewBag.HouseConfigureList = OperateContext.Current.BLLSession.IHouseConfigureBLL.GetListBy(m => m.ID > 0, m => m.Sort, true);
                }
            }

            return View(houseInfo);
        }

        /// <summary>
        /// 保存房源信息
        /// add by fruitchan
        /// 2016-12-25 20:28:28
        /// </summary>
        /// <param name="houseInfoModel">房源实体信息</param>
        /// <returns>保存结果</returns>
        [HttpPost]
        public ActionResult SaveHouseInfo(HouseInfo houseInfoModel)
        {
            string status = "fail";
            string msg = null;
            UserInfoView loginUserInfo = OperateContext.Current.UserInfo;

            if (houseInfoModel != null)
            {
                #region 校验数据

                // TODO:校验数据

                #endregion

                // 房源图片
                if (!String.IsNullOrWhiteSpace(houseInfoModel.JsonHouseImgList))
                {
                    houseInfoModel.HouseImgList = JsonConvert.DeserializeObject<IList<HouseImg>>(houseInfoModel.JsonHouseImgList);
                }

                int result = 0;

                var shopInfo = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.UserId == loginUserInfo.ID).FirstOrDefault();
                if (shopInfo != null)
                {
                    houseInfoModel.ShopId = shopInfo.ID;
                    houseInfoModel.ShopName = shopInfo.ShopName;
                    houseInfoModel.Address = shopInfo.Locations;
                    houseInfoModel.About = shopInfo.About;
                    houseInfoModel.Rules = shopInfo.Rules;
                    houseInfoModel.ChargesNotes = shopInfo.ChargesNotes;
                    houseInfoModel.IsBack = houseInfoModel.IsBack ?? 0;
                    houseInfoModel.IsCancel = houseInfoModel.IsCancel ?? 0;
                }
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
                        houseInfo.Address = houseInfoModel.Address;
                        houseInfo.ChargesNotes = houseInfoModel.ChargesNotes;
                        houseInfo.Rules = houseInfoModel.Rules;
                        houseInfo.About = houseInfoModel.About;
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
                    houseInfoModel.UserInfoID = loginUserInfo.ID;
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
                        houseInfoModel.UserInfoID = loginUserInfo.ID;
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

        /// <summary>
        /// 更改房屋状态
        /// add by fruitchan
        /// 2016-12-26 01:25:22
        /// </summary>
        /// <param name="id">房屋编号</param>
        /// <param name="state">房屋状态</param>
        /// <returns>更改结果</returns>
        public ActionResult UpdateState(int id, int state)
        {
            string status = "fail";
            string msg = "操作失败！";

            HouseInfo house = OperateContext.Current.BLLSession.IHouseInfoBLL.GetListBy(m => m.ID == id).FirstOrDefault();

            if (house != null)
            {
                house.State = state;
                int result = OperateContext.Current.BLLSession.IHouseInfoBLL.Modify(house, "State");

                if (result == 1)
                {
                    status = "ok";
                    msg = "操作成功！";
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 更改状态（可订、已满）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateEmptyState(int id)
        {
            string status = "fail";
            string msg = "操作失败！";

            HouseInfo house = OperateContext.Current.BLLSession.IHouseInfoBLL.GetListBy(m => m.ID == id).FirstOrDefault();

            if (house != null)
            {
                //0 可订 1已满
                house.IsEmpty = house.IsEmpty.HasValue && house.IsEmpty == 1 ? 0 : 1;
                int result = OperateContext.Current.BLLSession.IHouseInfoBLL.Modify(house, "IsEmpty");

                if (result == 1)
                {
                    status = "ok";
                    msg = "操作成功！";
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 设置房屋托管
        /// add by fruitchan
        /// 2016-12-26 01:28:15
        /// </summary>
        /// <param name="id">房源编号</param>
        /// <returns>设置结果</returns>
        public ActionResult SetHouseTrusteeship(int id)
        {
            string status = "fail";
            string msg = "操作失败！";

            HouseInfo house = OperateContext.Current.BLLSession.IHouseInfoBLL.GetListBy(m => m.ID == id).FirstOrDefault();

            if (house != null)
            {
                house.IsTrusteeship = true;
                int result = OperateContext.Current.BLLSession.IHouseInfoBLL.Modify(house, "IsTrusteeship");

                if (result == 1)
                {
                    status = "ok";
                    msg = "操作成功！";
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        /// <summary>
        /// 判断房屋是否被预定
        /// add by fruitchan
        /// 2017-3-2 21:12:11
        /// </summary>
        /// <param name="houseId">房源编号</param>
        /// <param name="startTime">入住时间</param>
        /// <param name="endTime">退房时间</param>
        /// <returns>是否被预定</returns>
        [HttpPost]
        public ActionResult IsReservation(int houseId, DateTime startTime, DateTime endTime)
        {
            //取消验证 2018-03-31 fishby
            var status = "fail";
            string msg = "该时间段房间已被预定，请重新选择时间!";

            //int count = OperateContext.Current.BLLSession.IOrderInfoBLL.CountRow(m => m.HouseInfoID == houseId && (m.State == 0 || m.State == 1 || m.State == 2) && ((startTime >= m.StartDate && startTime < m.EndDate) || (endTime >= m.StartDate && endTime < m.EndDate)));

            //if (count == 0)
            //{
            status = "ok";
            msg = "ok";
            // }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }


        /// <summary>
        /// 店铺列表
        /// add by fruitchan
        /// 2016-12-18 23:30:43
        /// </summary>
        /// <param name="keywords">关键字（房源标题）</param>
        /// <returns>View</returns>
        [ValidateInput(false)]
        public ActionResult ShopList(string keywords, long? spotId = null)
        {
            ViewBag.Keywords = HttpUtility.HtmlEncode(keywords);
            return View();
        }

        /// <summary>
        /// 分页查询店铺信息
        /// add by fruitchan
        /// 2016-12-26 21:00:23
        /// </summary>
        /// <param name="keywords">关键字（房源标题）</param>
        /// <param name="minPrice">最小价格</param>
        /// <param name="maxPrice">最大价格</param>
        /// <param name="bedroomNum">室</param>
        /// <param name="livingRoomNum">厅</param>
        /// <param name="toiletNum">卫</param>
        /// <param name="decorativeStyle">装修风格</param>
        /// <param name="bySort">排序方式</param>
        /// <param name="page">当前页码</param>
        /// <param name="row">每页显示数据条数</param>
        /// <returns>租房列表数据</returns>
        [HttpPost]
        public ActionResult GetShopList(string keywords, decimal? minPrice, decimal? maxPrice, int? desc, int bySort, int? byFilter, int page = 1, int row = 12)
        {
            var whereLambda = PredicateBuilder.True<ShopInfo>();

            #region 查询条件
            // 房源标题
            Expression<Func<ShopInfo, bool>> stateExpression = h => h.State == 1;
            whereLambda = whereLambda.And(stateExpression);
            if (!String.IsNullOrWhiteSpace(keywords))
            {
                // 房源标题
                Expression<Func<ShopInfo, bool>> keywordsExpression = p => p.ShopName.Contains(keywords) || p.SpotName.Contains(keywords);
                whereLambda = whereLambda.And(keywordsExpression);
            }

            if (minPrice.HasValue && minPrice.Value > 0)
            {
                // 最小价格
                Expression<Func<ShopInfo, bool>> minPriceExpression = p => p.StartPrice >= minPrice.Value;
                whereLambda = whereLambda.And(minPriceExpression);
            }

            if (maxPrice.HasValue && maxPrice.Value > 0)
            {
                // 最大价格
                Expression<Func<ShopInfo, bool>> maxPriceExpression = p => p.StartPrice <= maxPrice.Value;
                whereLambda = whereLambda.And(maxPriceExpression);
            }
            if (byFilter.HasValue && byFilter > 0)
            {
                string filterValue = string.Empty;
                switch (byFilter)
                {
                    case 1:
                        filterValue = "民宿";
                        break;
                    case 2:
                        filterValue = "农家乐";
                        break;
                    case 3:
                        filterValue = "宾馆";
                        break;
                }
                Expression<Func<ShopInfo, bool>> filterExpression = p => p.DecorativeStyle == filterValue;
                whereLambda = whereLambda.And(filterExpression);
            }
            #endregion

            int rowCount = 0;
            IList<ShopInfo> shopList = null;
            bool isDesc = desc.HasValue ? true : false;
            if (bySort == 1)
            {
                // 价格排序
                shopList = OperateContext.Current.BLLSession.IShopInfoBLL.GetPagedList(page, row, ref rowCount, whereLambda, h => h.StartPrice, isDesc);
            }
            else if (bySort == 2)
            {
                //口碑排序
                shopList = OperateContext.Current.BLLSession.IShopInfoBLL.GetPagedList(page, row, ref rowCount, whereLambda, h => h.Scores, isDesc);
            }
            else if (bySort == 3)
            {
                // 距离
                shopList = OperateContext.Current.BLLSession.IShopInfoBLL.GetPagedList(page, row, ref rowCount, whereLambda, h => h.Locations, isDesc);
            }
            else
            {
                // （推荐）默认排序
                shopList = OperateContext.Current.BLLSession.IShopInfoBLL.GetPagedList(page, row, ref rowCount, whereLambda, h => h.SortBy, false);
            }

            PageModel<ShopInfo> model = PageModel<ShopInfo>.GetPageModel(shopList, page, rowCount, row);

            return new JsonResult() { Data = model };

        }

        /// <summary>
        /// 店铺管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ShopManager()
        {
            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                ViewBag.IsRequireRealName = false;
                if (OperateContext.Current.UserInfo != null && OperateContext.Current.UserInfo.IsRealName == 0)
                {
                    ViewBag.IsRequireRealName = true;
                }
                ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;
            }
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ShopView(int? id)
        {
            ShopInfo shop = new ShopInfo();
            if (id.HasValue)
            {
                shop = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.ID == id).FirstOrDefault();
                if (shop == null)
                {
                    shop = new ShopInfo();
                }
            }
            ViewBag.SpotList = OperateContext.Current.BLLSession.ISpotInfoBLL.GetListBy(h => h.ID > 0, h => h.Sort).ToList();
            ViewBag.ShopCategoryList = OperateContext.Current.BLLSession.IShopCategoryBLL.GetListBy(h => h.ID > 0, h => h.SortBy).ToList();
            return View(shop);
        }

        [HttpPost]
        public ActionResult GetShopData(int page = 1, int limit = 10)
        {
            var result = new ResponseData<ShopInfo> { code = 0, msg = "", count = 0, data = new List<ShopInfo> { } };
            var whereLambda = PredicateBuilder.True<ShopInfo>();
            #region 查询条件
            //Expression<Func<ShopInfo, bool>> keywordsExpression = p => p.UserId == OperateContext.Current.UserInfo.ID);
            //whereLambda = whereLambda.And(keywordsExpression);
            #endregion
            int rowCount = 0;
            result.data = OperateContext.Current.BLLSession.IShopInfoBLL.GetPagedList(page, limit, ref rowCount, whereLambda, h => h.CreateOn);
            result.count = rowCount;

            return new JsonResult { Data = result };
        }

        [HttpPost]
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
                }, new CustomValidate()
                {
                    FieldName = "风格",
                    FieldValue = shopInfo.ShopType.HasValue ? shopInfo.ShopType.ToString() : null,
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
                    if (shopInfo.ID > 0)
                    {
                        //验证名称是否重复
                        var queryObj = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.ShopName == shopInfo.ShopName && h.ID != shopInfo.ID).FirstOrDefault();
                        if (queryObj != null)
                        {
                            msg = "该名称已经存在！";
                        }
                        else
                        {
                            var shop = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.ID == shopInfo.ID).FirstOrDefault();
                            if (shop != null)
                            {
                                shop.ShopName = shopInfo.ShopName;
                                shop.ShopImgs = shopInfo.ShopImgs;
                                shop.SpotId = shopInfo.SpotId;
                                shop.SpotName = shopInfo.SpotName;
                                shop.ShopType = shopInfo.ShopType;
                                shop.ShopTypeName = shopInfo.ShopTypeName;
                                shop.StartPrice = shopInfo.StartPrice ?? 0;
                                shop.Locations = shopInfo.Locations;
                                shop.About = shopInfo.About;
                                shop.Rules = shopInfo.Rules;
                                shop.ChargesNotes = shopInfo.ChargesNotes;
                                shop.IsCheck = 0; //修改信息后需重新审核
                                var result = OperateContext.Current.BLLSession.IShopInfoBLL.Modify(shop);
                                if (result == 1)
                                {
                                    status = "ok";
                                    msg = "保存成功！";
                                }
                            }
                        }

                    }
                    else
                    {
                        //验证名称是否重复
                        var queryObj = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.ShopName == shopInfo.ShopName).FirstOrDefault();
                        if (queryObj != null)
                        {
                            msg = "该名称已经存在！";
                        }
                        else
                        {
                            shopInfo.CreateOn = DateTime.Now;
                            shopInfo.UserId = OperateContext.Current.UserInfo.ID;
                            shopInfo.UserName = OperateContext.Current.UserInfo.Username;
                            shopInfo.IsCheck = 0;
                            shopInfo.State = 1; //默认上架
                            shopInfo.IsDel = 0;
                            int result = OperateContext.Current.BLLSession.IShopInfoBLL.Add(shopInfo);
                            if (result == 1)
                            {
                                status = "ok";
                                msg = "保存成功！";
                            }
                        }

                    }
                    if (status == "ok")
                    {
                        ShopInfoCertificate sic = new ShopInfoCertificate();
                        sic.ShopId = shopInfo.ID;
                        sic.State = 0;
                        sic.CreateOn = DateTime.Now;
                        OperateContext.Current.BLLSession.IShopInfoCertificateBLL.Add(sic);

                    }

                }
            }
            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

        [HttpPost]
        public ActionResult ModifyShop(ShopInfo shopInfo)
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
                }, new CustomValidate()
                {
                    FieldName = "风格",
                    FieldValue = shopInfo.ShopType.HasValue ? shopInfo.ShopType.ToString() : null,
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
                    var shop = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.ID == shopInfo.ID).FirstOrDefault();

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
                    shop.ShopName = shopInfo.ShopName;
                    shop.SpotId = shopInfo.SpotId;
                    shop.SpotName = shopInfo.SpotName;
                    shop.ShopType = shopInfo.ShopType;
                    shop.ShopTypeName = shopInfo.ShopTypeName;
                    shop.StartPrice = shopInfo.StartPrice.HasValue ? shopInfo.StartPrice : 0;
                    shop.Locations = shopInfo.Locations;
                    shop.About = shopInfo.About;
                    shop.Rules = shopInfo.Rules;
                    shop.ChargesNotes = shopInfo.ChargesNotes;
                    shop.ShopImgs = shopInfo.ShopImgs;
                    int result = OperateContext.Current.BLLSession.IShopInfoBLL.Modify(shop);
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
        /// 获取房东房屋数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetHouseList(string keywords, int? state, int page = 1, int limit = 10)
        {
            var result = new ResponseData<HouseInfo> { code = 0, msg = "", count = 0, data = new List<HouseInfo> { } };
            var shop = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.UserId == OperateContext.Current.UserInfo.ID).FirstOrDefault();
            if (shop != null)
            {
                var whereLambda = PredicateBuilder.True<HouseInfo>();

                #region 查询条件
                Expression<Func<HouseInfo, bool>> shopIdExpression = h => h.ShopId == shop.ID;
                whereLambda = whereLambda.And(shopIdExpression);
                #endregion

                int rowCount = 0;
                result.data = OperateContext.Current.BLLSession.IHouseInfoBLL.GetPagedList(page, limit, ref rowCount, whereLambda, h => h.CreateTime, false);
                result.count = rowCount;
                if (result.data != null && result.data.Count > 0)
                {
                    foreach (var house in result.data)
                    {
                        house.HouseImgList = OperateContext.Current.BLLSession.IHouseImgBLL.GetListBy(h => h.HouseInfoID == house.ID, h => h.CreateTime, true);
                    }
                }
            }

            return new JsonResult { Data = result };
        }
    }
}

using Holidays.Common;
using Holidays.Model.Entites;
using Holidays.Model.FormatModel;
using Holidays.Web.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Areas.Weixin.Controllers
{
    public class WeHouseController : BaseController
    {
        //
        // GET: /Weixin/WeHouse/

        /// <summary>
        /// 销售房源列表页面
        /// add by fruitchan
        /// 2017-2-14 20:04:53
        /// </summary>
        /// <returns>View</returns>
        public ActionResult SellHouseListView(string keywords, int? bySort)
        {
            int currentCityId = OperateContext.Current.CurrentCity.Id;
            IList<HouseInfoView> houseList = null;

            keywords = keywords == null ? "" : keywords;
            ViewBag.Keywords = keywords;
            ViewBag.BySort = bySort;

            if (bySort == 1)
            {
                // 价格排序
                houseList = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetListBy(m => m.IsSell == true && m.State == 0 && m.CityId == currentCityId && m.HouseTitle.Contains(keywords), m => m.Price, true);
            }
            else if (bySort == 2)
            {
                // 评论数排序
                houseList = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetListBy(m => m.IsSell == true && m.State == 0 && m.CityId == currentCityId && m.HouseTitle.Contains(keywords), m => m.EvaluateNum, false);
            }
            else if (bySort == 3)
            {
                // 预定数排序
                houseList = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetListBy(m => m.IsSell == true && m.State == 0 && m.CityId == currentCityId && m.HouseTitle.Contains(keywords), m => m.BuyNum, false);
            }
            else
            {
                // 默认排序
                houseList = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetListBy(m => m.IsSell == true && m.State == 0 && m.CityId == currentCityId && m.HouseTitle.Contains(keywords), m => m.EvaluateAvgScore | m.BuyNum, false);
            }

            if (houseList != null && houseList.Count > 0)
            {
                // 房源图片
                foreach (var house in houseList)
                {
                    house.HouseImgList = OperateContext.Current.BLLSession.IHouseImgBLL.GetListBy(m => m.HouseInfoID == house.ID, m => m.CreateTime, true);
                }
            }
            return View(houseList);
        }

        /// <summary>
        /// 租房列表页面
        /// add by fruitchan
        /// 2017-2-14 20:35:09
        /// </summary>
        /// <returns>View</returns>
        public ActionResult RentHouseListView(string keywords, int? bySort,int? id)
        {
            if (id != null)
            {
                var tempModel = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(o => o.ID == id).FirstOrDefault();
                if (tempModel != null)
                {
                    ViewBag.ShopInfo = tempModel;
                }
            }
            else
            {
                var tempModel = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(o => o.ShopName == keywords).FirstOrDefault();
                if (tempModel != null)
                {
                    ViewBag.ShopInfo = tempModel;
                }
            }
                

            int currentCityId = OperateContext.Current.CurrentCity.Id;
            IList<HouseInfoView> houseList = null;

            keywords = keywords == null ? "" : keywords;
            ViewBag.Keywords = keywords;
            ViewBag.BySort = bySort;

            if (bySort == 1)
            {
                // 价格排序
                houseList = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetListBy(m => m.CityId == currentCityId && m.State == 0 && (m.HouseTitle.Contains(keywords) || m.ShopName.Contains(keywords)), m => m.Price, true);
            }
            else if (bySort == 2)
            {
                // 评论数排序
                houseList = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetListBy(m => m.CityId == currentCityId && m.State == 0 && (m.HouseTitle.Contains(keywords) || m.ShopName.Contains(keywords)), m => m.EvaluateNum, false);
            }
            else if (bySort == 3)
            {
                // 预定数排序
                houseList = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetListBy(m => m.CityId == currentCityId && m.State == 0 && (m.HouseTitle.Contains(keywords) || m.ShopName.Contains(keywords)), m => m.BuyNum, false);
            }
            else
            {
                // 默认排序
                houseList = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetListBy(m => m.CityId == currentCityId && m.State == 0 && (m.HouseTitle.Contains(keywords) || m.ShopName.Contains(keywords)), m => m.EvaluateAvgScore | m.BuyNum, false);
            }

            if (houseList != null && houseList.Count > 0)
            {
                // 房源图片
                foreach (var house in houseList)
                {
                    house.HouseImgList = OperateContext.Current.BLLSession.IHouseImgBLL.GetListBy(m => m.HouseInfoID == house.ID, m => m.CreateTime, true);
                }
            }
            return View(houseList);
        }

        /// <summary>
        /// 房源详情页面
        /// add by fruitchan
        /// 2017-2-15 20:09:16
        /// </summary>
        /// <param name="id">房源编号</param>
        /// <returns>View</returns>
        public ActionResult HouseDetailsView(int? id)
        {
            HouseInfoView houseInfo = null;

            if (id.HasValue)
            {
                houseInfo = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetListBy(m => m.ID == id && m.State == 0).FirstOrDefault();

                if (houseInfo != null)
                {
                    houseInfo.HouseImgList = OperateContext.Current.BLLSession.IHouseImgBLL.GetListBy(m => m.HouseInfoID == houseInfo.ID, m => m.CreateTime, true);

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

                    ViewBag.City = cityName;
                    #endregion

                    // 已预订订单
                    IList<OrderInfo> orderList = OperateContext.Current.BLLSession.IOrderInfoBLL.GetListBy(m => m.HouseInfoID == houseInfo.ID && (m.State == 0 || m.State == 1 || m.State == 2) && m.EndDate >= DateTime.Now, m => m.StartDate, true);
                    ViewBag.HouseOrderList = orderList;
                    //获取店铺
                    var shop = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.ID == houseInfo.ShopId).FirstOrDefault();
                    if(shop != null)
                    {
                        houseInfo.About = shop.About;
                        houseInfo.ChargesNotes = shop.ChargesNotes;
                        houseInfo.Rules = shop.Rules;
                        houseInfo.Address = shop.Locations;
                        ViewBag.COORDINATE = shop.COORDINATE;
                    }
                }
            }

            return View(houseInfo);
        }

        /// <summary>
        /// 房源评价页面
        /// add by fruitchan
        /// 2017-2-15 20:56:48
        /// </summary>
        /// <param name="houseId">房源编号</param>
        /// <returns>View</returns>
        public ActionResult EvaluateView(int? houseId)
        {
            ViewBag.HouseId = houseId;

            IList<HouseEvaluate> houseEvaluateList = null;

            if (houseId.HasValue)
            {
                HouseInfoView houseInfo = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetListBy(m => m.ID == houseId && m.State == 0).FirstOrDefault();

                if (houseInfo != null)
                {
                    if (houseInfo.EvaluateNum > 0)
                    {
                        // 房源评价分
                        double cleanScoreTotal = OperateContext.Current.BLLSession.IHouseEvaluateBLL.GetListBy(m => m.HouseInfoID == houseInfo.ID).Sum(m => m.CleanScore);
                        ViewBag.CleanScore = Convert.ToInt32(cleanScoreTotal / houseInfo.EvaluateNum);

                        double locationScoreTotal = OperateContext.Current.BLLSession.IHouseEvaluateBLL.GetListBy(m => m.HouseInfoID == houseInfo.ID).Sum(m => m.LocationScore);
                        ViewBag.LocationScore = Convert.ToInt32(locationScoreTotal / houseInfo.EvaluateNum);

                        double environmentScoreTotal = OperateContext.Current.BLLSession.IHouseEvaluateBLL.GetListBy(m => m.HouseInfoID == houseInfo.ID).Sum(m => m.EnvironmentScore);
                        ViewBag.EnvironmentScore = Convert.ToInt32(environmentScoreTotal / houseInfo.EvaluateNum);

                        double serviceScoreTotal = OperateContext.Current.BLLSession.IHouseEvaluateBLL.GetListBy(m => m.HouseInfoID == houseInfo.ID).Sum(m => m.ServiceScore);
                        ViewBag.ServiceScore = Convert.ToInt32(serviceScoreTotal / houseInfo.EvaluateNum);

                        double performanceScoreScoreTotal = OperateContext.Current.BLLSession.IHouseEvaluateBLL.GetListBy(m => m.HouseInfoID == houseInfo.ID).Sum(m => m.PerformanceScore);
                        ViewBag.PerformanceScore = Convert.ToInt32(performanceScoreScoreTotal / houseInfo.EvaluateNum);
                    }
                }

                houseEvaluateList = OperateContext.Current.BLLSession.IHouseEvaluateBLL.GetListBy(m => m.HouseInfoID == houseId.Value, m => m.CreateTime, false);

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
            }

            return View(houseEvaluateList);
        }

        /// <summary>
        /// 房源管理页面
        /// add by fruitchan
        /// 2017-2-14 20:51:59
        /// </summary>
        /// <returns>View</returns>
        public ActionResult HouseManageView()
        {
            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;
                //店铺
                var shop = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.UserId == OperateContext.Current.UserInfo.ID).FirstOrDefault();
                ViewBag.HasShop = shop != null ? true : false;
                if (shop != null)
                {
                    ViewBag.IsCheck = shop.IsCheck ?? 0;
                    if(shop.IsCheck == 2)
                    {
                        var sic = OperateContext.Current.BLLSession.IShopInfoCertificateBLL.GetListBy(h => h.ShopId == shop.ID, h => h.CreateOn, false).FirstOrDefault();
                        if (sic != null)
                        {
                            ViewBag.FailReason = sic.FailReason;
                        }
                    }

                }
                if (shop != null && shop.IsCheck == 1)
                {
                    //IList<HouseInfo> houseList = OperateContext.Current.BLLSession.IHouseInfoBLL.GetListBy(m => m.UserInfoID == OperateContext.Current.UserInfo.ID);
                    IList<HouseInfo> houseList = OperateContext.Current.BLLSession.IHouseInfoBLL.GetListBy(m => m.ShopId == shop.ID);
                    // 房源图片
                    if (houseList != null && houseList.Count > 0)
                    {
                        foreach (var house in houseList)
                        {
                            house.IsEmpty = house.IsEmpty ?? 0;
                            house.HouseImgList = OperateContext.Current.BLLSession.IHouseImgBLL.GetListBy(m => m.HouseInfoID == house.ID, m => m.CreateTime, true);
                        }
                    }

                    ViewBag.HouseList = houseList;
                }

            }
            return View();
        }

        /// <summary>
        /// 发布房源页面
        /// add by fruitchan
        /// 2017-2-14 20:54:56
        /// </summary>
        /// <returns>View</returns>
        public ActionResult AddHouseView(int? id)
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
                var shopInfo = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.UserId == OperateContext.Current.UserInfo.ID).FirstOrDefault();
                if (shopInfo != null)
                {
                    houseInfo.Address = shopInfo.Locations;
                    houseInfo.About = shopInfo.About;
                    houseInfo.Rules = shopInfo.Rules;
                    houseInfo.ChargesNotes = shopInfo.ChargesNotes; 
                }
                // 配套设施
                ViewBag.HouseConfigureList = OperateContext.Current.BLLSession.IHouseConfigureBLL.GetListBy(m => m.ID > 0, m => m.Sort, true);
            }

            return View(houseInfo);
        }

        public ActionResult ShopListView(string keywords, int? bySort, int? byFilter, int? desc,long? spotId)
        {
            keywords = keywords == null ? "" : keywords;
            ViewBag.Keywords = keywords;
            ViewBag.BySort = bySort;
            byFilter = byFilter.HasValue ? byFilter : 0;
            bool isDesc = desc.HasValue ? true : false;
            if (byFilter.HasValue)
            {
                switch (byFilter.Value)
                {
                    case 1:
                        ViewBag.ByFilter = "民宿";
                        break;
                    case 2:
                        ViewBag.ByFilter = "农家乐";
                        break;
                    case 3:
                        ViewBag.ByFilter = "宾馆";
                        break;
                    default:
                        ViewBag.ByFilter = "不限";
                        break;
                }
            }
            IList<ShopInfo> shopList = null;
            if (bySort == 1)
            {
                // 价格排序
                shopList = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.State == 1 && h.SpotName.Contains(keywords), h => h.StartPrice, isDesc);
            }
            else if (bySort == 2)
            {
                //口碑排序
                shopList = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.State == 1 && h.SpotName.Contains(keywords), h => h.Scores, isDesc);
            }
            else if (bySort == 3)
            {
                // 距离
                shopList = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.State == 1 && h.SpotName.Contains(keywords), h => h.Locations, isDesc);
            }
            else
            {
                // （推荐）默认排序
                shopList = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.State == 1 && h.SpotName.Contains(keywords), h => h.SortBy, isDesc);
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
                shopList = shopList.Where(h => h.DecorativeStyle == filterValue).ToList();
            }

            return View(shopList);

        }

        /// <summary>
        /// 店铺管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ShopManageView()
        {
            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;
                //店铺列表
                IList<ShopInfo> shopList = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.IsDel != 1);
                ViewBag.ShopList = shopList;
            }
            return View();
        }

        public ActionResult AddShopView()
        {
            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;
                //风景
                ViewBag.SpotList = OperateContext.Current.BLLSession.ISpotInfoBLL.GetListBy(h => h.ID > 0, h => h.Sort).ToList();
                //风格
                ViewBag.ShopCategoryList = OperateContext.Current.BLLSession.IShopCategoryBLL.GetListBy(h => h.ID > 0, h => h.SortBy).ToList();
            }
            return View();
        }

        /// <summary>
        /// 创建店铺
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateShopView()
        {
            if (ViewBag.IsLogin)
            {
                ViewBag.IsPermission = false;
                ViewBag.IsPermission = OperateContext.Current.BLLSession.IUserAccountBLL.GetListBy(m => m.ID == OperateContext.Current.UserInfo.AccountID).FirstOrDefault().State == 0;
            }
            return View();
        }


    }
}

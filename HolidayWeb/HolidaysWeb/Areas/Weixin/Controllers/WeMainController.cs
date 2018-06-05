using Holidays.Model.Entites;
using Holidays.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Areas.Weixin.Controllers
{
    public class WeMainController : BaseController
    {
        //
        // GET: /Weixin/WeMain/

        /// <summary>
        /// 微信首页
        /// add by fruitchan
        /// 2017-2-7 19:53:33
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            Model.Entites.Region currentCity = OperateContext.Current.CurrentCity;

            // Banner
            ViewBag.BannerList = OperateContext.Current.BLLSession.IHomeDataBLL.GetListBy(m => m.Type == 2).OrderByDescending(m => m.ID).ToList();

            // 热门景点
            // ViewBag.SpotInfoList = OperateContext.Current.BLLSession.ISpotInfoBLL.GetPagedList(1, 6, m => m.IsHomeShow == true && m.CityId == currentCity.Id, m => m.Sort, true);
            ViewBag.SpotInfoList = OperateContext.Current.BLLSession.ISpotInfoBLL.GetListBy(h => h.IsHomeShow == true && h.CityId == currentCity.Id, h => h.Sort, true);

            // 热门度假房
            // 房源图片
            //IList<HouseInfoView> houseList = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetPagedList(1, 4, m => m.State == 0 && m.CityId == currentCity.Id, m => m.BuyNum, false);

            //// 房源图片
            //if (houseList != null && houseList.Count > 0)
            //{
            //    foreach (var house in houseList)
            //    {
            //        house.HouseImgList = OperateContext.Current.BLLSession.IHouseImgBLL.GetListBy(m => m.HouseInfoID == house.ID, m => m.CreateTime, true);
            //    }
            //}

            //ViewBag.HouseList = houseList;
            //热门店铺
            ViewBag.ShopList = OperateContext.Current.BLLSession.IShopInfoBLL.GetListBy(h => h.IsCheck == 1 && h.State == 1, h => h.SortBy, true);
            //建筑
            List<buildInfo> buildlist = new List<buildInfo>();
           
            for (int i = 0; i < 4; i++)
            {
                buildInfo buildInfos = new buildInfo();
                if (i < 3)
                {
                    buildInfos.buildName = String.Format("房源{0}", i + 1);
                }
                else {
                    buildInfos.buildName = String.Format("更多");
                }
                buildlist.Add(buildInfos);
            }
            ViewBag.buildInfos = buildlist;

            // 地方特产
            ViewBag.ProductList = OperateContext.Current.BLLSession.IProductInfoBLL.GetPagedList(1, 4, m => m.IsHomeShow == true && m.CityId == currentCity.Id, m => m.Sort, true);

            // 租赁车辆
            ViewBag.CarList = OperateContext.Current.BLLSession.ICarInfoBLL.GetPagedList(1, 4, m => m.ID > 0 && m.CityId == currentCity.Id, m => m.ID, true).ToList();

            return View();
        }

        /// <summary>
        /// 城市列表页面
        /// add by fruitchan
        /// 2017-2-7 21:08:36
        /// </summary>
        /// <returns>View</returns>
        public ActionResult CityList()
        {
            return View();
        }

        /// <summary>
        /// 退出登录
        /// add by fruitchan
        /// 2017-1-15 11:57:21
        /// </summary>
        /// <returns>Redirect</returns>
        public ActionResult Logout()
        {
            OperateContext.Current.CelarSession();
            return Redirect("/Weixin/WeMain/Index");
        }
    }
    public class buildInfo{

        public string buildName { get; set; }
        public int buildId { get; set; }
        public string buildPic { get; set; }
        public string buildremark { get; set; }
        //评价
        public int evaluation { get; set; }
        //建筑风格
        public string buildType { get; set; }

    }
}

using Holidays.Model.Entites;
using Holidays.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Areas.Weixin.Controllers
{
    public class WeOrderController : BaseController
    {
        //
        // GET: /Weixin/WeOrder/
        /// <summary>
        /// 提交订单页面
        /// add by fruitchan
        /// 2016-12-20 23:49:49
        /// </summary>
        [HttpGet]
        public ActionResult SubmitOrderView(int? id, int? isFullPrice)
        {
            HouseInfoView houseInfo = null;
            UserInfoView loginUserInfo = OperateContext.Current.UserInfo;
            ViewBag.IsFullPrice = isFullPrice;

            if (id.HasValue)
            {
                houseInfo = OperateContext.Current.BLLSession.IHouseInfoViewBLL.GetListBy(m => m.ID == id && m.State == 0).FirstOrDefault();

                if (houseInfo != null)
                {
                    houseInfo.HouseImgList = OperateContext.Current.BLLSession.IHouseImgBLL.GetListBy(m => m.HouseInfoID == houseInfo.ID, m => m.CreateTime, true);
                }
            }

            return View(houseInfo);
        }

    }
}

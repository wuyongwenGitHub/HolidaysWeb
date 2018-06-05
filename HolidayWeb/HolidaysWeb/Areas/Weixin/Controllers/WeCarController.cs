using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Holidays.Web.Areas.Weixin.Controllers
{
    public class WeCarController : Controller
    {
        //
        // GET: /Weixin/WeCar/

        /// <summary>
        /// 租赁车辆
        /// add by fruitchan
        /// 2017-2-14 20:43:30
        /// </summary>
        /// <returns>View</returns>
        public ActionResult CarListView()
        {
            Model.Entites.Region currentCity = OperateContext.Current.CurrentCity;
            IList<Model.Entites.CarInfo> carList = OperateContext.Current.BLLSession.ICarInfoBLL.GetListBy(m => m.ID > 0 && m.CityId == currentCity.Id, m => m.ID, true).ToList();
            return View(carList);
        }

    }
}

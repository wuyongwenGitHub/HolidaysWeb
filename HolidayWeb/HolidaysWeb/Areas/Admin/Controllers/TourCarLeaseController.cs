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
    public class TourCarLeaseController : Controller
    {
        //
        // GET: /Admin/TourCarLease/

        /// <summary>
        /// 旅游车辆租赁管理页面
        /// add by fruitchan
        /// 2016-11-22 20:29:21
        /// </summary>
        /// <returns>View</returns>
        

        public ActionResult Index()
        {
            // 房东审核待办数
            ViewBag.CertificateTotal = OperateContext.Current.BLLSession.IUserInfoCertificateBLL.CountRow(m => m.State == 0);
            return View();
        }

        /// <summary>
        /// 分页查询车辆数据
        /// add by fruitchan
        /// 2016-12-4 22:11:58
        /// </summary>
        /// <param name="carName">车辆名称</param>
        /// <param name="linkman">联系人</param>
        /// <param name="phone1">联系电话</param>
        /// <param name="page">当前页码</param>
        /// <param name="row">每页记录数</param>
        /// <returns>车辆列表数据</returns>
        public ActionResult GetCarList(string carName, string linkman, string phone1, int page = 1, int row = 10)
        {
            var whereLambda = PredicateBuilder.True<CarInfo>();

            #region 查询条件

            if (!String.IsNullOrWhiteSpace(carName))
            {
                // 车辆名称
                Expression<Func<CarInfo, bool>> carNameExpression = p => p.CarName.Contains(carName);
                whereLambda = whereLambda.And(carNameExpression);
            }

            if (!String.IsNullOrWhiteSpace(linkman))
            {
                // 联系人
                Expression<Func<CarInfo, bool>> linkmanExpression = p => p.Linkman.Contains(linkman);
                whereLambda = whereLambda.And(linkmanExpression);
            }

            if (!String.IsNullOrWhiteSpace(phone1))
            {
                // 联系电话
                Expression<Func<CarInfo, bool>> phone1Expression = p => p.Phone1.Contains(phone1);
                whereLambda = whereLambda.And(phone1Expression);
            }

            #endregion

            int rowCount = 0;
            IList<CarInfo> carList = OperateContext.Current.BLLSession.ICarInfoBLL.GetPagedList(page, row, ref rowCount, whereLambda, m => m.ID, true);

            PageModel<CarInfo> model = PageModel<CarInfo>.GetPageModel(carList, page, rowCount, row);

            return new JsonResult() { Data = model };
        }

        /// <summary>
        /// 根据编号删除车辆信息
        /// add by fruitchan
        /// 2016-12-5 20:10:38
        /// </summary>
        /// <param name="id">车辆编号</param>
        /// <returns>删除结果</returns>
        public ActionResult DelCarByID(long id)
        {
            int result = OperateContext.Current.BLLSession.ICarInfoBLL.DelBy(m => m.ID == id);

            return OperateContext.Current.RedirectAjax(result == 1 ? "ok" : "fail", result == 1 ? "删除成功！" : "删除失败！", null, null);
        }

        /// <summary>
        /// 保存车辆信息
        /// add by fruitchan
        /// 2016-12-5 20:10:57
        /// </summary>
        /// <param name="carInfo">车辆信息</param>
        /// <returns>保存结果</returns>
        public ActionResult SaveCarInfo(CarInfo carInfo)
        {
            string status = "fail";
            string msg = "保存失败！";

            if (carInfo != null)
            {
                #region 校验数据
                msg = Validate.ValidateString(new CustomValidate()
                {
                    FieldName = "车辆名称",
                    FieldValue = carInfo.CarName,
                    IsRequired = true,
                    MaxLength = 20
                }, new CustomValidate()
                {
                    FieldName = "联系人",
                    FieldValue = carInfo.Linkman,
                    IsRequired = true,
                    MaxLength = 20
                }, new CustomValidate()
                {
                    FieldName = "联系电话",
                    FieldValue = carInfo.Phone1,
                    IsRequired = true,
                    MaxLength = 20
                }, new CustomValidate()
                {
                    FieldName = "备用电话",
                    FieldValue = carInfo.Phone2,
                    MaxLength = 20
                }, new CustomValidate()
                {
                    FieldName = "联系地址",
                    FieldValue = carInfo.Address,
                    MaxLength = 100
                });

                if (msg == null && (carInfo.ProvinceId == null || carInfo.ProvinceId <= 0))
                {
                    msg = "请选择省份！";
                }

                if (msg == null && (carInfo.CityId == null || carInfo.CityId <= 0))
                {
                    msg = "请选择城市！";
                }
                #endregion

                if (msg == null)
                {
                    int result = carInfo.ID > 0 ? OperateContext.Current.BLLSession.ICarInfoBLL.Modify(carInfo) : OperateContext.Current.BLLSession.ICarInfoBLL.Add(carInfo);

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
        /// 根据编号查询车辆信息
        /// add by fruitchan
        /// 2016-12-5 21:45:16
        /// </summary>
        /// <param name="id">车辆编号</param>
        /// <returns>车辆信息</returns>
        [HttpPost]
        public ActionResult QueryCarInfoByID(long id)
        {
            return OperateContext.Current.RedirectAjax("ok", null, OperateContext.Current.BLLSession.ICarInfoBLL.GetListBy(m => m.ID == id).FirstOrDefault(), null);
        }

    }
}

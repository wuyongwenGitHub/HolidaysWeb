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
    public class ScenicSpotController : Controller
    {
        //
        // GET: /Admin/ScenicSpot/

        /// <summary>
        /// 景点管理页面
        /// add by fruitchan
        /// 2016-11-25 19:05:55
        /// </summary>
        /// <returns>View</returns>
        [ValidMenuPerm]

        public ActionResult Index()
        {
            // 房东审核待办数
            ViewBag.CertificateTotal = OperateContext.Current.BLLSession.IUserInfoCertificateBLL.CountRow(m => m.State == 0);
            return View();
        }

        /// <summary>
        /// 分页查询景点信息
        /// add by fruitchan
        /// 2016-12-6 19:56:56
        /// </summary>
        /// <param name="scenicSpotName">景点名称</param>
        /// <param name="page">页码</param>
        /// <param name="row">每页显示数据条数</param>
        /// <returns>景点列表数据</returns>
        public ActionResult GetScenicSpotList(string scenicSpotName, int page = 1, int row = 10)
        {
            var whereLambda = PredicateBuilder.True<SpotInfo>();

            #region 查询条件

            if (!String.IsNullOrWhiteSpace(scenicSpotName))
            {
                // 景点名称
                Expression<Func<SpotInfo, bool>> scenicSpotNameExpression = p => p.ScenicSpotName.Contains(scenicSpotName);
                whereLambda = whereLambda.And(scenicSpotNameExpression);
            }

            #endregion

            int rowCount = 0;
            IList<SpotInfo> spotInfoList = OperateContext.Current.BLLSession.ISpotInfoBLL.GetPagedList(page, row, ref rowCount, whereLambda, m => m.Sort);

            PageModel<SpotInfo> model = PageModel<SpotInfo>.GetPageModel(spotInfoList, page, rowCount, row);

            return new JsonResult() { Data = model };
        }

        public ActionResult GetScenicSpotDropdownList()
        {
            IList<SpotInfo> spotList = OperateContext.Current.BLLSession.ISpotInfoBLL.GetListBy(h => h.ID > 0, h => h.Sort).ToList();

            return OperateContext.Current.RedirectAjax("ok", null, spotList, null);
        }

        /// <summary>
        /// 根据编号删除景点信息
        /// </summary>
        /// <param name="id">景点编号</param>
        /// <returns>删除结果</returns>
        public ActionResult DelScenicSpotByID(long id)
        {
            int result = OperateContext.Current.BLLSession.ISpotInfoBLL.DelBy(m => m.ID == id);

            return OperateContext.Current.RedirectAjax(result == 1 ? "ok" : "fail", result == 1 ? "删除成功！" : "删除失败！", null, null);
        }

        /// <summary>
        /// 根据编号查询景点信息
        /// add by fruitchan
        /// 2016-12-6 20:02:36
        /// </summary>
        /// <param name="id">景点编号</param>
        /// <returns>景点信息</returns>
        public ActionResult QueryScenicSpotByID(long id)
        {
            return OperateContext.Current.RedirectAjax("ok", null, OperateContext.Current.BLLSession.ISpotInfoBLL.GetListBy(m => m.ID == id).FirstOrDefault(), null);
        }

        /// <summary>
        /// 保存景点信息
        /// add by fruitchan
        /// 2016-12-6 20:02:05
        /// </summary>
        /// <param name="spotInfo">景点信息</param>
        /// <returns>保存结果</returns>
        public ActionResult SaveScenicSpot(SpotInfo spotInfo)
        {
            string status = "fail";
            string msg = "保存失败！";

            if (spotInfo != null)
            {
                #region 校验数据

                msg = Validate.ValidateString(new CustomValidate()
                {
                    FieldName = "景区名称",
                    FieldValue = spotInfo.ScenicSpotName,
                    IsRequired = true,
                    MaxLength = 100
                }, new CustomValidate()
                {
                    FieldName = "景区图片",
                    FieldValue = spotInfo.ScenicSpotImgs,
                    IsRequired = true
                });

                if (msg == null && (spotInfo.ProvinceId == null || spotInfo.ProvinceId <= 0))
                {
                    msg = "请选择省份！";
                }

                if (msg == null && (spotInfo.CityId == null || spotInfo.CityId <= 0))
                {
                    msg = "请选择城市！";
                }

                #endregion

                if (msg == null)
                {
                    spotInfo.CreateTime = DateTime.Now;
                    if (spotInfo.LinkSpotId.HasValue)
                    {
                        var linkSpot = OperateContext.Current.BLLSession.ISpotInfoBLL.GetListBy(h => h.ID == spotInfo.LinkSpotId).FirstOrDefault();
                        if(linkSpot != null)
                        {
                            spotInfo.LinkSpotId = linkSpot.ID;
                            spotInfo.LinkSpotName = linkSpot.ScenicSpotName;
                        }
                    }
                    int result = spotInfo.ID > 0 ? OperateContext.Current.BLLSession.ISpotInfoBLL.Modify(spotInfo) : OperateContext.Current.BLLSession.ISpotInfoBLL.Add(spotInfo);

                    if (result == 1)
                    {
                        status = "ok";
                        msg = "保存成功！";
                    }
                }
            }

            return OperateContext.Current.RedirectAjax(status, msg, null, null);
        }

    }
}

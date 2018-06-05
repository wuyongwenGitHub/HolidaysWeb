
using Holidays.Model.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Holidays.Common;
using Holidays.Common.Attributes;

namespace Holidays.Web.Areas.Admin.Controllers
{
    public class HomeDataSettingController : Controller
    {
        //
        // GET: /Admin/HomeDataSetting/

        /// <summary>
        /// 首页数据设置页面
        /// add by fruitchan
        /// 2016-11-25 18:58:41
        /// </summary>
        /// <returns>View</returns>
        [ValidMenuPerm]
        public ActionResult Index()
        {
            // 房东审核待办数
            ViewBag.CertificateTotal = OperateContext.Current.BLLSession.IUserInfoCertificateBLL.CountRow(m => m.State == 0);

            ViewBag.AD = OperateContext.Current.BLLSession.IHomeDataBLL.GetListBy(m => m.Type == 1).FirstOrDefault();

            return View();
        }

        /// <summary>
        /// 修改广告信息
        /// add by fruitchan
        /// 2016-12-4 15:09:13
        /// </summary>
        /// <param name="data">新的广告数据</param>
        /// <returns>修改结果</returns>
        [HttpPost]
        public ActionResult UpdateAD(HomeData data)
        {
            string status = "-1";
            string msg = "保存失败！";

            if (data != null)
            {
                msg = Validate.ValidateString(new CustomValidate()
                {
                    FieldName = "广告标题",
                    FieldValue = data.Title,
                    IsRequired = true,
                    MaxLength = 100
                }, new CustomValidate()
                {
                    FieldName = "广告图片",
                    FieldValue = data.ImgUrl,
                    IsRequired = true
                });

                if (msg == null)
                {
                    int result = OperateContext.Current.BLLSession.IHomeDataBLL.Modify(data);

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
        /// 获取Banner数据
        /// add by fruitchan
        /// 2016-12-4 18:04:50
        /// </summary>
        /// <returns>Banner列表数据</returns>
        [HttpPost]
        public ActionResult GetBannerList()
        {
            IList<HomeData> bannerList = OperateContext.Current.BLLSession.IHomeDataBLL.GetListBy(m => m.Type == 2).ToList();
            return OperateContext.Current.RedirectAjax("ok", null, bannerList, null);
        }

        /// <summary>
        /// 根据编号删除Banner
        /// add by fruitchan
        /// 2016-12-4 19:59:19
        /// </summary>
        /// <param name="id">banner 编号</param>
        /// <returns>删除结果</returns>
        [HttpPost]
        public ActionResult DelBannerByID(long id)
        {
            int result = OperateContext.Current.BLLSession.IHomeDataBLL.DelBy(m => m.ID == id);

            return OperateContext.Current.RedirectAjax(result == 1 ? "ok" : "fail", result == 1 ? "删除成功！" : "删除失败！", null, null);
        }

        /// <summary>
        /// 保存Banner信息
        /// add by fruitchan
        /// 2016-12-5 20:09:40
        /// </summary>
        /// <param name="data">Banner信息</param>
        /// <returns>保存结果</returns>
        [HttpPost]
        public ActionResult SaveBanner(HomeData data)
        {
            string status = "fail";
            string msg = "保存失败！";

            if (data != null)
            {
                msg = Validate.ValidateString(new CustomValidate()
                {
                    FieldName = "Banner标题",
                    FieldValue = data.Title,
                    IsRequired = true,
                    MaxLength = 100
                }, new CustomValidate()
                {
                    FieldName = "Banner图片",
                    FieldValue = data.ImgUrl,
                    IsRequired = true
                });

                if (msg == null)
                {

                    int result = 0;

                    if (data.ID > 0)
                    {
                        result = OperateContext.Current.BLLSession.IHomeDataBLL.Modify(data);
                    }
                    else
                    {
                        data.Type = 2;
                        result = OperateContext.Current.BLLSession.IHomeDataBLL.Add(data);
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
        /// 根据编号查询banner图
        /// add by fruitchan
        /// 2016-12-5 22:02:01
        /// </summary>
        /// <param name="id">banner编号</param>
        /// <returns>banner图信息</returns>
        [HttpPost]
        public ActionResult QueryBannerByID(long id)
        {
            return OperateContext.Current.RedirectAjax("ok", null, OperateContext.Current.BLLSession.IHomeDataBLL.GetListBy(m => m.ID == id).FirstOrDefault(), null);
        }
    }
}

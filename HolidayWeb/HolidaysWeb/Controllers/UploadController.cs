using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Holidays.Web.Controllers
{
    public class UploadController : Controller
    {
        //
        // GET: /Upload/

        /// <summary>
        /// 文件上传功能
        /// add by fruitchan
        /// 2016-12-3 18:19:37
        /// </summary>
        /// <returns>上传文件结果</returns>
        [HttpPost]
        public JsonResult UploadImage()
        {
            var state = "0";
            var url = "";

            var file = HttpContext.Request.Files[0];

            if (file != null)
            {
                var fileName = file.FileName;
                var ext = fileName.Substring(fileName.LastIndexOf('.'), fileName.Length - fileName.LastIndexOf('.')).ToLower();

                if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                {
                    string newFileName = Guid.NewGuid().ToString() + ext;
                    string filePath = DateTime.Now.ToString("yyyyMMdd");
                    string fileFullPath = Server.MapPath("/Images/Upload/") + filePath;

                    if (!Directory.Exists(fileFullPath))
                    {
                        Directory.CreateDirectory(fileFullPath);
                    }

                    file.SaveAs(fileFullPath + "/" + newFileName);

                    url = filePath + "/" + newFileName;
                }
                else
                {
                    state = "上传的图片格式不正确！";
                }
            }
            else
            {
                state = "上传图片不能为空！";
            }

            JsonResult jr = new JsonResult();
            jr.Data = new
            {
                state = state,
                url = url
            };

            return jr;
        }

    }
}

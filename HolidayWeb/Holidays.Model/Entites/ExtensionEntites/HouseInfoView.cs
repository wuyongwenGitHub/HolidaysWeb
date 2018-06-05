using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holidays.Model.Entites
{
    public partial class HouseInfoView
    {
        /// <summary>
        /// 房源设施列表
        /// </summary>
        public IList<HouseConfigure> HouseConfigureList
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(Facilities))
                {
                    return JsonConvert.DeserializeObject<IList<HouseConfigure>>(Facilities);
                }

                return null;
            }
        }

        /// <summary>
        /// 房源图片列表JSON数据
        /// </summary>
        public string JsonHouseImgList { get; set; }

        /// <summary>
        /// 房源图片列表
        /// </summary>
        public IList<HouseImg> HouseImgList { get; set; }

        /// <summary>
        /// 房源首图
        /// </summary>
        public string FirstImg
        {
            get
            {
                if (HouseImgList != null && HouseImgList.Count > 0)
                {
                    return HouseImgList[0].ImgUrl;

                }
                return null;
            }
        }
    }
}

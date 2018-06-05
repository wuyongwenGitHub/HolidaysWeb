using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Holidays.Model.FormatModel
{
    /// <summary>
    /// 公众号的全局唯一票据信息
    /// add by fruitchan
    /// 2016-1-20 11:22:18
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        /// 公众号的全局唯一票据
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        /// 授权用户唯一标识
        /// </summary>
        public string openid { get; set; }
    }
}
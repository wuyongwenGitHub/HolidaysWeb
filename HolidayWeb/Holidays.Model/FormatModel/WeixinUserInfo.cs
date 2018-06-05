using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holidays.Model.FormatModel
{
    public class WeixinUserInfo
    {
        /// <summary>
        /// 普通用户的标识，对当前开发者帐号唯一
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 普通用户昵称
        /// </summary>
        public string nickname { get; set; }

        /// <summary>
        /// 普通用户性别，1为男性，2为女性
        /// </summary>
        public int sex { get; set; }

        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
        /// </summary>
        public string headimgurl { get; set; }

        /// <summary>
        ///  用户统一标识。针对一个微信开放平台帐号下的应用，同一用户的unionid是唯一的。
        /// </summary>
        public string unionid { get; set; }

    }
}

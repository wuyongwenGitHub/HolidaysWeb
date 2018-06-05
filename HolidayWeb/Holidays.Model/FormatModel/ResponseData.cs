using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holidays.Model.FormatModel
{
    public class ResponseData<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 数据总数
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public IList<T> data { get; set; }
    }
}

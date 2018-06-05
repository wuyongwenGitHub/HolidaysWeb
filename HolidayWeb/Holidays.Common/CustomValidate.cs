using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holidays.Common
{
    /// <summary>
    /// 自定义验证实体
    /// add by fruitchan
    /// 2016-12-7 21:41:47
    /// </summary>
    public class CustomValidate
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段值
        /// </summary>
        public string FieldValue { get; set; }

        /// <summary>
        /// 是否必须字段
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 长度（最长）
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// 长度最短
        /// </summary>
        public int MinLength { get; set; }

        /// <summary>
        /// 是否手机号
        /// </summary>
        public bool IsPhone { get; set; }

        /// <summary>
        /// 是否身份证号
        /// </summary>
        public bool IsIdCard { get; set; }

        /// <summary>
        /// 是否邮箱地址
        /// </summary>
        public bool IsEmail { get; set; }
    }
}

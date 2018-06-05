using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Holidays.Common
{
    public static class Validate
    {
        /// <summary>
        /// 验证数据
        /// add by fruitchan
        /// 2016-12-7 21:48:03
        /// </summary>
        /// <param name="validate">需要验证的数据集</param>
        /// <returns>验证结果</returns>
        public static string ValidateString(params CustomValidate[] validates)
        {
            if (validates != null)
            {
                foreach (var validate in validates)
                {
                    // 校验是否必须参数
                    if (validate.IsRequired)
                    {
                        if (String.IsNullOrWhiteSpace(validate.FieldValue))
                        {
                            return validate.FieldName + "不能为空！";
                        }
                    }

                    // 验证长度(最长&最短)
                    if (!String.IsNullOrWhiteSpace(validate.FieldValue) && validate.MaxLength > 0 && validate.MinLength > 0)
                    {
                        if (validate.FieldValue.Length > validate.MaxLength || validate.FieldValue.Length < validate.MinLength)
                        {
                            return validate.FieldName + "应为" + validate.MinLength + "至" + validate.MaxLength + "位字符！";
                        }
                    }

                    // 验证长度(最长)
                    if (!String.IsNullOrWhiteSpace(validate.FieldValue) && validate.MaxLength > 0)
                    {
                        if (validate.FieldValue.Length > validate.MaxLength)
                        {
                            return validate.FieldName + "不能超过" + validate.MaxLength + "个字符！";
                        }
                    }

                    // 验证长度(最短)
                    if (!String.IsNullOrWhiteSpace(validate.FieldValue) && validate.MinLength > 0)
                    {
                        if (validate.FieldValue.Length < validate.MinLength)
                        {
                            return validate.FieldName + "不能少于" + validate.MinLength + "个字符！";
                        }
                    }

                    // 校验手机号
                    if (validate.IsPhone && (String.IsNullOrWhiteSpace(validate.FieldValue) || !ValidatePhone(validate.FieldValue)))
                    {
                        return validate.FieldName + "格式不正确！";
                    }

                    // 校验身份证号
                    if (validate.IsIdCard && (String.IsNullOrWhiteSpace(validate.FieldValue) || !ValidateIdCard(validate.FieldValue)))
                    {
                        return validate.FieldName + "格式不正确！";
                    }

                    // 校验邮箱地址
                    if (validate.IsEmail && (String.IsNullOrWhiteSpace(validate.FieldValue) || !ValidateEmail(validate.FieldValue)))
                    {
                        return validate.FieldName + "格式不正确！";
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 校验手机号
        /// add by fruitchan
        /// 2016-12-23 20:39:33
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns>校验结果</returns>
        public static bool ValidatePhone(string phone)
        {
            if (String.IsNullOrWhiteSpace(phone))
            {
                return false;
            }
            Regex rgx = new Regex("^0?1[3|4|5|7|8][0-9]\\d{8}$");
            return rgx.IsMatch(phone);
        }

        /// <summary>
        /// 校验身份证号
        /// add by fruitchan
        /// 2016-12-23 20:42:15
        /// </summary>
        /// <param name="idcard">身份证号</param>
        /// <returns>校验结果</returns>
        public static bool ValidateIdCard(string idcard)
        {
            if (String.IsNullOrWhiteSpace(idcard))
            {
                return false;
            }
            Regex rgx = new Regex("^(\\d{18})|(\\d{17}[X|x])$");
            return rgx.IsMatch(idcard);
        }

        /// <summary>
        /// 校验邮箱地址
        /// add by fruitchan
        /// 2016-12-23 20:42:41
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <returns>校验结果</returns>
        public static bool ValidateEmail(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            Regex rgx = new Regex("^(\\w)+(\\w+)*@(\\w)+((\\.\\w+)+)$");
            return rgx.IsMatch(email);
        }
    }
}

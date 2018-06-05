using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holidays.Common
{
    public static class CouponHelper
    {
        public static string GetCouponCode()
        {
            long l = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                l *= ((int)b + 1);
            }
            string code = string.Format("{0:x}", l - DateTime.Now.Ticks);

            return code.FromFormat().ToUpper();
        }

        /// <summary>
        /// 截取验证码为8位 AAAABBBBCCCCDDDD
        /// </summary>
        /// <returns></returns>
        public static string Get8LCode()
        {
           return GetCouponCode().Sub8L();
        }

        /// <summary>
        /// 格式化 例如 AAAA-BBBB-CCCC-DDDD
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string ToFormat(this string code)
        {
            code = code.ToUpper();
            int charLength = 4;
            string newCode = string.Empty;
            for (var i = 0; i < code.Length; i += charLength)
            {
                newCode = string.Format("{0}-{1}", newCode, code.Substring(i, charLength));
            }
            newCode = newCode.TrimStart('-');

            return newCode;
        }

        /// <summary>
        ///  取消格式化 例如 AAAA-BBBB-CCCC-DDDD 变为 AAAABBBBCCCCDDDD
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string FromFormat(this string code)
        {
            return code.Replace("-", "").ToUpper();
        }

        /// <summary>
        /// 截取验证码为8位 AAAABBBBCCCCDDDD
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string Sub8L(this string code)
        {
            return code.Substring(0, 8).ToUpper();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holidays.Common
{
    /// <summary>
    /// 配置文件帮助类
    /// </summary>
    public static class ConfigurationHelper
    {
        public static string AppSetting(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
    }
}

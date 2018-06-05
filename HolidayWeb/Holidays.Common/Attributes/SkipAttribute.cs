using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holidays.Common.Attributes
{
    /// <summary>
    /// 跳过权限验证
    /// </summary>
    public class SkipAttribute : Attribute
    {
    }

    /// <summary>
    /// 验证当前用户菜单权限
    /// </summary>
    public class ValidMenuPermAttribute : Attribute
    {

    }

}

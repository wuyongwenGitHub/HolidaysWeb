using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Holidays.IBLL
{
    public partial interface  ICustomBLL
    {
        List<T> GetTodayPrice<T>(int ShopId, string date);
    }
}

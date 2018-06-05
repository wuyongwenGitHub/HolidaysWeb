using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Holidays.IDAL
{
    public interface ICustomDAL
    {
        List<T> GetTodayPrice<T>(long ShopId, string date);
    }
}

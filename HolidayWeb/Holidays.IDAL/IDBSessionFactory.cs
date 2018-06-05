using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holidays.IDAL
{
    /// <summary>
    /// 数据仓储工厂
    /// </summary>
    public interface IDBSessionFactory
    {
        IDAL.IDBSession GetDBSession();
    }
}

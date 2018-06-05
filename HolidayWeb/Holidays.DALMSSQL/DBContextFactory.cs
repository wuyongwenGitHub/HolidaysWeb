using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web;

namespace Holidays.DALMSSQL
{
    public class DBContextFactory : IDAL.IDBContextFactory
    {
        #region 创建 EF上下文 对象，在线程中共享 一个 上下文对象 + DbContext GetDbContext()
        /// <summary>
        /// 创建 EF上下文 对象，在线程中共享 一个 上下文对象
        /// </summary>
        /// <returns></returns>
        public DbContext GetDbContext()
        {
            //从当前线程中 获取 EF上下文对象
            DbContext dbContext = CallContext.GetData(typeof(DBContextFactory).Name) as DbContext;
            if (dbContext == null)
            {
                dbContext = new Model.Entites.HolidaysDBEntities();
                dbContext.Configuration.ValidateOnSaveEnabled = false;
                //将新创建的 ef上下文对象 存入线程
                CallContext.SetData(typeof(DBContextFactory).Name, dbContext);
            }
            dbContext.Database.Log = (sql) => {
                if (string.IsNullOrEmpty(sql) == false)
                {
                    Console.WriteLine("************sql执行*************");
                    Console.WriteLine(sql);
                    Console.WriteLine("************sql结束************");
                }
            };
            return dbContext;

        }
        #endregion
        public DbContext WrapGetDbContext()
        {
            DbContext dbContext = new Model.Entites.HolidaysDBEntities();
            dbContext.Configuration.ValidateOnSaveEnabled = false;
            CallContext.SetData(typeof(DBContextFactory).Name, dbContext);
            return dbContext;
        }
    }
}

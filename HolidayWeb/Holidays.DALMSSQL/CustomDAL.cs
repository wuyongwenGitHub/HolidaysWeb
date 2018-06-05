using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Holidays.DALMSSQL
{
    public class CustomDAL: IDAL.ICustomDAL
    {
        /// <summary>
        /// EF上下文对象
        /// </summary>
        protected DbContext db = new DBContextFactory().GetDbContext();

        public List<T> GetTodayPrice<T>(long ShopId,string date)
        {
            string sql =$"select  HouseInfo.ID,HouseInfo.HouseTitle,case when ShopToDayPriceSet.price  is null then HouseInfo.Price else ShopToDayPriceSet.price end as 'todayprice' from HouseInfo inner join ShopToDayPriceSet on HouseInfo.ID = ShopToDayPriceSet.ShopId and ShopToDayPriceSet.statu = 0 and ShopToDayPriceSet.date = '{date}' and HouseInfo.ID={ShopId}";
            return db.Database.SqlQuery<T>(sql).ToList(); ;
            //throw new NotImplementedException();
        }
    }
}

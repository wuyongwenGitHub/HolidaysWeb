using Holidays.DALMSSQL;
using Holidays.IDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace Holidays.BLL
{
    public class CustomBLL : IBLL.ICustomBLL
    {
        //1.数据层接口 对象 - 等待 被实例化
        protected IDAL.ICustomDAL iCdal;// = new idal.BaseDAL();
        public List<T> GetTodayPrice<T>(int ShopId, string date)
        {
            iCdal =new CustomDAL();
            return iCdal.GetTodayPrice<T>(ShopId,date);
        }
    }
}

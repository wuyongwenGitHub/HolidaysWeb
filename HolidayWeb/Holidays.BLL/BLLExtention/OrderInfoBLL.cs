using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holidays.BLL
{
    public partial class OrderInfoBLL
    {
        public decimal GetPriceTodayTotal()
        {
            return DBSession.IOrderInfoDAL.GetPriceTodayTotal();
        }

        public decimal GetPriceTotal()
        {
            return DBSession.IOrderInfoDAL.GetPriceTotal();
        }

        public decimal GetPlatformRoyaltyTodayTotal()
        {
            return DBSession.IOrderInfoDAL.GetPlatformRoyaltyTodayTotal();
        }

        public decimal GetPlatformRoyaltyTotal()
        {
            return DBSession.IOrderInfoDAL.GetPlatformRoyaltyTotal();
        }
    }
}

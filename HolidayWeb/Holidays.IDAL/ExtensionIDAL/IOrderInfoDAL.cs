using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holidays.IDAL
{
    public partial interface IOrderInfoDAL
    {
        decimal GetPriceTodayTotal();

        decimal GetPriceTotal();

        decimal GetPlatformRoyaltyTodayTotal();

        decimal GetPlatformRoyaltyTotal();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holidays.IBLL
{
    public partial interface IOrderInfoBLL
    {
        decimal GetPriceTodayTotal();

        decimal GetPriceTotal();

        decimal GetPlatformRoyaltyTodayTotal();

        decimal GetPlatformRoyaltyTotal();
    }
}

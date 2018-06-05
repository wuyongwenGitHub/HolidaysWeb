using Holidays.Model.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holidays.DALMSSQL
{
    public partial class OrderInfoDAL
    {
        /// <summary>
        /// 查询平台今日总交易额
        /// add by fruitchan
        /// 2016-12-11 23:28:09
        /// </summary>
        /// <returns>今日总交易额</returns>
        public decimal GetPriceTodayTotal()
        {
            DateTime dtToday = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

            decimal? priceTotalToday = db.Set<OrderInfo>().Where(m => m.CreateTime >= dtToday && (m.State == 1 || m.State == 2)).Sum(m => (decimal?)m.Price);
            return priceTotalToday.HasValue ? priceTotalToday.Value : 0;
        }

        /// <summary>
        /// 查询平台总交易额
        /// add by fruitchan
        /// 2016-12-11 23:28:14
        /// </summary>
        /// <returns>平台总交易额</returns>
        public decimal GetPriceTotal()
        {
            decimal? priceTotal = db.Set<OrderInfo>().Where(m => (m.State == 1 || m.State == 2)).Sum(m => (decimal?)m.Price);
            return priceTotal.HasValue ? priceTotal.Value : 0;
        }

        /// <summary>
        /// 查询平台今日提成额
        /// add by fruitchan
        /// 2016-12-11 23:29:22
        /// </summary>
        /// <returns>平台今日提成额</returns>
        public decimal GetPlatformRoyaltyTodayTotal()
        {
            DateTime dtToday = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            decimal? platformRoyaltyToday = db.Set<OrderInfo>().Where(m => m.CreateTime >= dtToday && (m.State == 1 || m.State == 2)).Sum(m => (decimal?)m.PlatformRoyalty);
            return platformRoyaltyToday.HasValue ? platformRoyaltyToday.Value : 0;
        }

        /// <summary>
        /// 查询平台总提成额
        /// add by fruitchan
        /// 2016-12-11 23:29:43
        /// </summary>
        /// <returns>平台总提成额</returns>
        public decimal GetPlatformRoyaltyTotal()
        {
            DateTime dtToday = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            decimal? platformRoyaltyToday = db.Set<OrderInfo>().Where(m => (m.State == 1 || m.State == 2)).Sum(m => (decimal?)m.PlatformRoyalty);
            return platformRoyaltyToday.HasValue ? platformRoyaltyToday.Value : 0;
        }
    }
}

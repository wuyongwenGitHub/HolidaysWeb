//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Holidays.Model.Entites
{
    using System;
    using System.Collections.Generic;
    
    public partial class HouseInfo
    {
        public long ID { get; set; }
        public long UserInfoID { get; set; }
        public string HouseTitle { get; set; }
        public Nullable<int> ProvinceId { get; set; }
        public Nullable<int> CityId { get; set; }
        public Nullable<int> CountyId { get; set; }
        public int DecorativeStyle { get; set; }
        public int LeaseType { get; set; }
        public int BedroomNum { get; set; }
        public int LivingRoomNum { get; set; }
        public int ToiletNum { get; set; }
        public decimal Price { get; set; }
        public bool IsTrusteeship { get; set; }
        public bool IsSell { get; set; }
        public Nullable<decimal> SellPrice { get; set; }
        public bool IsCooking { get; set; }
        public bool IsPet { get; set; }
        public int StayPersonNum { get; set; }
        public double HouseSize { get; set; }
        public string Facilities { get; set; }
        public string Address { get; set; }
        public string About { get; set; }
        public string Rules { get; set; }
        public string ChargesNotes { get; set; }
        public string BaseInfo { get; set; }
        public bool IsReals { get; set; }
        public System.DateTime CreateTime { get; set; }
        public int State { get; set; }
        public int BuyNum { get; set; }
        public int EvaluateNum { get; set; }
        public int EvaluateAvgScore { get; set; }
        public Nullable<long> ShopId { get; set; }
        public string ShopName { get; set; }
        public Nullable<int> IsEmpty { get; set; }
        public Nullable<int> IsBack { get; set; }
        public Nullable<int> IsCancel { get; set; }
    }
}

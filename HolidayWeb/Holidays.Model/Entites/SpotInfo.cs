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
    
    public partial class SpotInfo
    {
        public long ID { get; set; }
        public string ScenicSpotName { get; set; }
        public string ScenicSpotDesc { get; set; }
        public string ScenicSpotImgs { get; set; }
        public bool IsHomeShow { get; set; }
        public int Sort { get; set; }
        public Nullable<int> ProvinceId { get; set; }
        public Nullable<int> CityId { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<long> LinkSpotId { get; set; }
        public string LinkSpotName { get; set; }
        public string Links { get; set; }
    }
}

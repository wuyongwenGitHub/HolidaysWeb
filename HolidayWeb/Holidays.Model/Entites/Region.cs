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
    
    public partial class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string ShortName { get; set; }
        public Nullable<int> LevelType { get; set; }
        public string CityCode { get; set; }
        public string ZipCode { get; set; }
        public string MergerName { get; set; }
        public Nullable<double> Lng { get; set; }
        public Nullable<double> Lat { get; set; }
        public string Pinyin { get; set; }
    }
}
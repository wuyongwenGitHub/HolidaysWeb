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
    
    public partial class UserFavorite
    {
        public long ID { get; set; }
        public long UserInfoID { get; set; }
        public long HouseInfoID { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
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
    
    public partial class ShopToDayPrice
    {
        public ShopToDayPrice()
        {
            this.statu = 0;
        }
    
        public int Id { get; set; }
        public int ShopId { get; set; }
        public string other { get; set; }
        public decimal price { get; set; }
        public int statu { get; set; }
        public string date { get; set; }
    }
}

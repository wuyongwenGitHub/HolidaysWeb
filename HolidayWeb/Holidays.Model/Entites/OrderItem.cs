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
    
    public partial class OrderItem
    {
        public long ID { get; set; }
        public string OrderNo { get; set; }
        public string OtherNo { get; set; }
        public long OrderId { get; set; }
        public decimal Price { get; set; }
        public decimal BalancePayment { get; set; }
        public int State { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> PayTime { get; set; }
    }
}

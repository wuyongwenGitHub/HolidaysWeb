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
    
    public partial class User
    {
        public User()
        {
            this.IsDeleted = false;
        }
    
        public long Id { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string UserRealName { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string Email { get; set; }
        public System.Guid GUIID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holidays.Model.FormatModel
{
    public class JsApiTicket
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string ticket { get; set; }
        public int expires_in { get; set; }
    }
}

using MISA.ApplicationCore.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Entities
{
    public class ServiceResult
    {
        public Object Data { get; set; }
        public string Message { get; set; }
        public MISACode Code { get; set; }
    }
}

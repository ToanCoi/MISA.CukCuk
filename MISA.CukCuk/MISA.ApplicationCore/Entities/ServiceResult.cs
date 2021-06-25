using MISA.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Entities
{
    public class ServiceResult
    {
        public string devMsg { get; set; }
        public string UserMsg { get; set; }
        public MISACode Code { get; set; }
    }
}

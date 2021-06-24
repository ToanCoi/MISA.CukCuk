using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Entities
{
    public class ServiceResult
    {
        /// <summary>
        /// Object tả chi tiết lỗi
        /// </summary>
        public Object Data { get; set; }

        /// <summary>
        /// Mã lỗi của MISA
        /// </summary>
        public int MISACode { get; set; }
    }
}

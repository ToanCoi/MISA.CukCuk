using MISA.Infrastructure;
using MISA.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore
{
    public class CustomerService
    {
        #region Method

        /// <summary>
        /// Lấytoàn bộ danh sách khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// CreatedBy: NVTOAN 24/06/2021
        public IEnumerable<Customer> GetCustomers()
        {
            var customerDBContext = new CustomerDBContext();
            return customerDBContext.GetCustomers();
        }
        #endregion
    }
}

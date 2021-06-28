using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Interface.Repository
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {

        /// <summary>
        /// Lấy thông tin khách hàng theo mã khách hàng
        /// </summary>
        /// <param name="customerCode">Mã khách hàng</param>
        /// <returns>Object khách hàng</returns>
        /// CreatedBy: NVTOAN 25/06/2021
        Customer GetCustomerByCode(string customerCode);

        /// <summary>
        /// Lấy thông tin khách hàng theo số điện thoại
        /// </summary>
        /// <param name="phoneNumber">Id của khách hàng</param>
        /// <returns>Object khách hàng</returns>
        /// CreatedBy: NVTOAN 25/06/2021
        Customer GetCustomerByPhone(string phoneNumber);

        /// <summary>
        /// Lấy thông tin khách hàng theo Email
        /// </summary>
        /// <param name="email">Id của khách hàng</param>
        /// <returns>Object khách hàng</returns>
        /// CreatedBy: NVTOAN 25/06/2021
        Customer GetCustomerByEmail(string email);
    }
}

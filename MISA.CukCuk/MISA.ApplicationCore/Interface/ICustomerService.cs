using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Interface
{
    public interface ICustomerService
    {
        /// <summary>
        /// Lấy toàn bộ danh sách khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// CreatedBy: NVTOAN 24/06/2021
        IEnumerable<Customer> GetCustomers();

        /// <summary>
        /// Lấy khách hàng bằng Id
        /// </summary>
        /// <param name="customerId">Id của khách hàng</param>
        /// <returns>Một Object khách hàng</returns>
        /// CreatedBy: NVTOAN 25/06/2021
        Customer GetCustomerById(Guid customerId);

        /// <summary>
        /// Thêm mới một khách hàng
        /// </summary>
        /// <param name="customer">Object khách hàng</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        ServiceResult InsertCustomer(Customer customer);

        /// <summary>
        /// Sửa thông tin khách hàng
        /// </summary>
        /// <param name="customerId">Id của khách hàng</param>
        /// <param name="customer">Object khách hàng</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        /// CreatedBy: NVTOAN 25/06/2021
        ServiceResult UpdateCustomer(Guid customerId, Customer customer);


        /// <summary>
        /// Xóa khách hàng theo Id
        /// </summary>
        /// <param name="customerId">Id của khách hàng</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        ServiceResult DeleteCustomer(Guid customerId);
    }
}

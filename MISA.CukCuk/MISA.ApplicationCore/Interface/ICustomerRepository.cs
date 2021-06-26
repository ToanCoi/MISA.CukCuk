using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Interface
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Lấy toàn bộ thông tin khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// CreatedBy: NVTOAN 24/06/2021
        IEnumerable<Customer> GetCustomers();

        /// <summary>
        /// Lấy thông tin khách hàng theo id
        /// </summary>
        /// <param name="customerId">Id của khách hàng</param>
        /// <returns>Object khách hàng</returns>
        /// CreatedBy: NVTOAN 25/06/2021
        Customer GetCustomerById(Guid customerId);

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

        /// <summary>
        /// Thêm mới một khách hàng
        /// </summary>
        /// <param name="customer">Object khách hàng</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        /// CreatedBy: NVTOAN 25/06/2021
        int InsertCustomer(Customer customer);


        /// <summary>
        /// Sửa thông tin một khách hàng
        /// </summary>
        /// <param name="id">Id của khách hàng</param>
        /// <param name="customer">Object khách hàng</param>
        /// <returns>Số dòng ảnh hưởng</returns>
        /// CreatedBy: NVTOAN 25/06/2021
        int UpdateCustomer(Guid customerId, Customer customer);

        /// <summary>
        /// Xóa khách hàng theo Id
        /// </summary>
        /// <param name="customerId">Id của khách hàng</param>
        /// <returns>Số dòng bị ảnh hưởng </returns>
        int DeleteCustomer(Guid customerId);
    }
}

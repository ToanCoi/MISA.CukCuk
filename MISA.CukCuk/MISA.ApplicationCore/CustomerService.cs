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

        /// <summary>
        /// Thêm mới một khách hàng
        /// </summary>
        /// <param name="customer">Object khách hàng/param>
        /// <returns></returns>
        /// CreatedBy: NVTOAN 25/06/2021
        public int InsertCustomer(Customer customer)
        {
            //Validate không để trống
            var customerCode = customer.CustomerCode;
            var phoneNumber = customer.PhoneNumber;
            var email = customer.Email;

            if (string.IsNullOrEmpty(customerCode))
            {
                var msg = new
                {
                    devMsg = new { fieldName = "CustomerCode", msg = "Mã khách hàng không được để trống" },
                    userMsg = "Mã khách hàng không được để trống",
                    Code = 999,

                };

                return BadRequest(msg);
            }

            if (string.IsNullOrEmpty(phoneNumber))
            {
                var msg = new
                {
                    devMsg = new { fieldName = "PhoneNumber", msg = "Mã khách hàng không được để trống" },
                    userMsg = "Mã khách hàng không được để trống",
                    Code = 999
                };

                return BadRequest(msg);
            }

            if (string.IsNullOrEmpty(email))
            {
                var msg = new
                {
                    devMsg = new { fieldName = "Email", msg = "Mã khách hàng không được để trống" },
                    userMsg = "Mã khách hàng không được để trống",
                    Code = 999
                };

                return BadRequest(msg);
            }

            //validate trùng mã
            //mã khách hàng
            var res = dbConnection.Query<Customer>("Proc_GetCustomerByCode", new { CustomerCode = customer.CustomerCode }, commandType: CommandType.StoredProcedure);
            if (res.Count() > 0)
            {
                var msg = new
                {
                    devMsg = new { fieldName = "CustomerCode", msg = "Mã khách hàng đã tồn tại" },
                    userMsg = "Mã khách hàng đã tồn tại",
                    Code = 999
                };

                return BadRequest(msg);
            }
            //số điện thoại
            res = dbConnection.Query<Customer>("Proc_GetCustomerByPhoneNumber", new { PhoneNumber = customer.PhoneNumber }, commandType: CommandType.StoredProcedure);
            if (res.Count() > 0)
            {
                var msg = new
                {
                    devMsg = new { fieldName = "PhoneNumber", msg = "Số điện thoại khách hàng đã tồn tại" },
                    userMsg = "Số điện thoại khách hàng đã tồn tại",
                    Code = 999
                };

                return BadRequest(msg);
            }
            //email
            res = dbConnection.Query<Customer>("Proc_GetCustomerByEmail", new { Email = customer.Email }, commandType: CommandType.StoredProcedure);
            if (res.Count() > 0)
            {
                var msg = new
                {
                    devMsg = new { fieldName = "Email", msg = "Email đã tồn tại" },
                    userMsg = "Email đã tồn tại",
                    Code = 999
                };

                return BadRequest(msg);
            }
        }
        #endregion
    }
}

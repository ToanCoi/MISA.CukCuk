using MISA.ApplicationCore.Entities;
using MISA.Infrastructure;
using MISA.Infrastructure.Models;
using MISA.Entity;
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
        public ServiceResult InsertCustomer(Customer customer)
        {
            var serviceResult = new ServiceResult();
            var customerDBContext = new CustomerDBContext();

            //Validate không để trống
            var customerCode = customer.CustomerCode;
            var phoneNumber = customer.PhoneNumber;
            var email = customer.Email;

            if (string.IsNullOrEmpty(customerCode))
            {
                serviceResult.Code = MISACode.Invalid;
                serviceResult.devMsg = "Mã khách hàng không được để trống";
                serviceResult.UserMsg = "Mã khách hàng không được để trống";

                return serviceResult;
            }

            if (string.IsNullOrEmpty(phoneNumber))
            {
                serviceResult.Code = MISACode.Invalid;
                serviceResult.devMsg = "Số điện thoại không được để trống";
                serviceResult.UserMsg = "Số điện thoại không được để trống";

                return serviceResult;
            }

            if (string.IsNullOrEmpty(email))
            {
                serviceResult.Code = MISACode.Invalid;
                serviceResult.devMsg = "Email không được để trống";
                serviceResult.UserMsg = "Email không được để trống";

                return serviceResult;
            }

            //validate trùng nhau
            //mã khách hàng
            var res = customerDBContext.GetCustomerByCode(customerCode);
            if (res != null)
            {
                serviceResult.Code = MISACode.Invalid;
                serviceResult.devMsg = "Mã khách hàng đã tồn tại";
                serviceResult.UserMsg = "Mã khách hàng đã tồn tại";

                return serviceResult;
            }
            //số điện thoại
            res = customerDBContext.GetCustomerByPhone(phoneNumber);
            if (res != null)
            {
                serviceResult.Code = MISACode.Invalid;
                serviceResult.devMsg = "Số điện thoại khách hàng đã tồn tại";
                serviceResult.UserMsg = "Số điện thoại khách hàng đã tồn tại";

                return serviceResult;
            }
            //email
            res = customerDBContext.GetCustomerByEmail(email);
            if (res != null)
            {
                serviceResult.Code = MISACode.Invalid;
                serviceResult.devMsg = "Email khách hàng đã tồn tại";
                serviceResult.UserMsg = "Email khách hàng đã tồn tại";

                return serviceResult;
            }

            //Nếu tất cả dữ liệu hợp lệ
            int rowAffects = customerDBContext.InsertCustomer(customer);
            if(rowAffects > 0)
            {
                serviceResult.Code = MISACode.Success;

                return serviceResult;
            } 
            else
            {
                
            }
        }
        #endregion
    }
}

using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using MISA.ApplicationCore.Interface;
using MISA.ApplicationCore.Enum;

namespace MISA.ApplicationCore
{
    public class CustomerService : ICustomerService
    {
        #region Declare
        ICustomerRepository _customerRepository;
        #endregion

        #region Constructor
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        #endregion


        #region Method

        public Customer GetCustomerById(Guid customerId)
        {
            return _customerRepository.GetCustomerById(customerId);
        }

        public ServiceResult UpdateCustomer(Guid customerId, Customer customer)
        {
            var serviceResult = new ServiceResult();

            //Validate không để trống
            if (string.IsNullOrEmpty(customer.CustomerCode))
            {
                var msg = new
                {
                    devMsg = "Mã khách hàng không được để trống",
                    userMsg = "Mã khách hàng không được để trống",
                    code = MISACode.Invalid,
                };

                serviceResult.Data = msg;
                serviceResult.Code = MISACode.Invalid;
                serviceResult.Message = "Dữ liệu không hợp lệ";

                return serviceResult;
            }

            //validate trùng nhau
            //mã khách hàng
            var res = _customerRepository.GetCustomerByCode(customer.CustomerCode);
            if (res != null)
            {
                var msg = new
                {
                    devMsg = "Mã khách hàng đã tồn tại",
                    userMsg = "Mã khách hàng đã tồn tại",
                    code = MISACode.Invalid,
                };

                serviceResult.Data = msg;
                serviceResult.Code = MISACode.Invalid;
                serviceResult.Message = "Dữ liệu không hợp lệ";

                return serviceResult;
            }

            //Nếu tất cả dữ liệu hợp lệ
            var rowAffect = _customerRepository.UpdateCustomer(customerId, customer);

            serviceResult.Code = MISACode.Success;
            serviceResult.Message = "Sửa thành công";
            serviceResult.Data = rowAffect;

            return serviceResult;
        }

        public ServiceResult DeleteCustomer(Guid customerId)
        {
            var serviceResult = new ServiceResult();

            var rowAffect = _customerRepository.DeleteCustomer(customerId);

            serviceResult.Code = MISACode.Success;
            serviceResult.Message = "xóa thành công";
            serviceResult.Data = rowAffect;

            return serviceResult;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _customerRepository.GetCustomers();
        }

        public ServiceResult InsertCustomer(Customer customer)
        {
            var serviceResult = new ServiceResult();

            //Validate không để trống
            if (string.IsNullOrEmpty(customer.CustomerCode))
            {
                var msg = new
                {
                    devMsg = "Mã khách hàng không được để trống",
                    userMsg = "Mã khách hàng không được để trống",
                    code = MISACode.Invalid,
                };

                serviceResult.Data = msg;
                serviceResult.Code = MISACode.Invalid;
                serviceResult.Message = "Dữ liệu không hợp lệ";

                return serviceResult;
            }

            //validate trùng nhau
            //mã khách hàng
            var res = _customerRepository.GetCustomerByCode(customer.CustomerCode);
            if (res != null)
            {
                var msg = new
                {
                    devMsg = "Mã khách hàng đã tồn tại",
                    userMsg = "Mã khách hàng đã tồn tại",
                    code = MISACode.Invalid,
                };

                serviceResult.Data = msg;
                serviceResult.Code = MISACode.Invalid;
                serviceResult.Message = "Dữ liệu không hợp lệ";

                return serviceResult;
            }

            //Nếu tất cả dữ liệu hợp lệ
            var rowAffect = _customerRepository.InsertCustomer(customer);

            serviceResult.Code = MISACode.Success;
            serviceResult.Message = "Thêm thành công";
            serviceResult.Data = rowAffect;

            return serviceResult;
        }
        #endregion
    }
}

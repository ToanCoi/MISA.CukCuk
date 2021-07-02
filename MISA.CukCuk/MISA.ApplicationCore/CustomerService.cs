using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using MISA.ApplicationCore.Enum;
using MISA.ApplicationCore.Interface.Service;
using MISA.ApplicationCore.Interface.Repository;
using System.Linq;

namespace MISA.ApplicationCore
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        #region Declare
        ICustomerRepository _customerRepository;
        ICustomerGroupRepository _customerGroupRepository;
        IEnumerable<CustomerGroup> _customerGroups = null;
        #endregion

        #region Constructor
        public CustomerService(ICustomerRepository customerRepository, ICustomerGroupRepository customerGroupRepository) : base(customerRepository)
        {
            _customerRepository = customerRepository;
            _customerGroupRepository = customerGroupRepository;
            _customerGroups = _customerGroupRepository.GetEntities();
        }
        #endregion


        #region Method
        public override ServiceResult InsertEntity(Customer entity)
        {
            return base.InsertEntity(entity);
        }

        public override ServiceResult UpdateEntity(Guid Id, Customer entity)
        {
            return base.UpdateEntity(Id, entity);
        }

        protected override bool CustomValidate(Customer entity, List<string> errorMsg)
        {
            var isValid = true;

            isValid = this.CheckExistCustomerGroup(entity);

            if(!isValid)
            {
                errorMsg.Add("Nhóm khách hàng không có trong hệ thống");
            }

            return isValid;
        }


        private bool CheckExistCustomerGroup(Customer customer)
        {
            var customerGroup = _customerGroups.ToList()
                    .FirstOrDefault(gr => gr.GetType().GetProperty("CustomerGroupName").GetValue(gr)
                    .Equals(customer.GetType().GetProperty("CustomerGroupName").GetValue(customer)));

            //Nếu có group trong hệ thống thì thêm Id vào 
            if(customerGroup != null)
            {
                var customerGroupId = customerGroup.GetType().GetProperty("CustomerGroupId").GetValue(customerGroup);

                customer.GetType().GetProperty("CustomerGroupId").SetValue(customer, customerGroupId);

                return true;
            }

            return false;
        }
        #endregion
    }
}

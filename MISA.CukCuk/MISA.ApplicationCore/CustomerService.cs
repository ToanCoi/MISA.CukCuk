using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using MISA.ApplicationCore.Enum;
using MISA.ApplicationCore.Interface.Service;
using MISA.ApplicationCore.Interface.Repository;

namespace MISA.ApplicationCore
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {

        #region Constructor
        public CustomerService(IBaseRepository<Customer> baseRepository) : base(baseRepository)
        {
        }
        #endregion


        #region Method
        public override int InsertEntity(Customer entity)
        {
            return base.InsertEntity(entity);
        }

        public override int UpdateEntity(Guid Id, Customer entity)
        {
            return base.UpdateEntity(Id, entity);
        }
        #endregion
    }
}

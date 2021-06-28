using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interface.Repository;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MISA.Infrastructure
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {

        #region Constructor
        public CustomerRepository(IConfiguration configuration) : base(configuration)
        {

        }
        #endregion

        #region Method

        public Customer GetCustomerByCode(string customerCode)
        {
            var customer = _dbConnection.QueryFirstOrDefault<Customer>("Proc_GetCustomerByCode", new { CustomerCode = customerCode }, commandType: CommandType.StoredProcedure);

            return customer;
        }

        public Customer GetCustomerByEmail(string email)
        {
            var customer = _dbConnection.QueryFirstOrDefault<Customer>("Proc_GetCustomerByEmail", new { Email = email }, commandType: CommandType.StoredProcedure);

            return customer;
        }

        public Customer GetCustomerById(Guid customerId)
        {
            var customer = _dbConnection.QueryFirstOrDefault<Customer>("Proc_GetCustomerById", new { CustomerId = customerId.ToString() }, commandType: CommandType.StoredProcedure);

            return customer;
        }

        public Customer GetCustomerByPhone(string phoneNumber)
        {
            var customer = _dbConnection.QueryFirstOrDefault<Customer>("Proc_GetCustomerByPhoneNumber", new { PhoneNumber = phoneNumber }, commandType: CommandType.StoredProcedure);

            return customer;
        }
        #endregion
    }
}

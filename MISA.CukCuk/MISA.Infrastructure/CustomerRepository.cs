using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interface;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MISA.Infrastructure
{
    public class CustomerRepository : ICustomerRepository
    {
        #region Declare
        IConfiguration _configuration;
        string _connectionString = string.Empty;
        IDbConnection _dbConnection = null;
        #endregion

        #region Constructor

        public CustomerRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MISACukCukConnectionString");
            _dbConnection = new MySqlConnection(_connectionString);
        }

        #endregion

        #region Method
        public int DeleteCustomer(Guid customerId)
        {
            int rowAffects = _dbConnection.Execute("Proc_DeleteCustomerById", new { CustomerId = customerId.ToString() }, commandType: CommandType.StoredProcedure);

            return rowAffects;
        }

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

        public IEnumerable<Customer> GetCustomers()
        {
            var customers = _dbConnection.Query<Customer>("Proc_GetCustomers", commandType: CommandType.StoredProcedure);

            return customers;
        }

        public int InsertCustomer(Customer customer)
        {
            int rowAffects = _dbConnection.Execute("Proc_InsertCustomer", customer, commandType: CommandType.StoredProcedure);

            return rowAffects;
        }

        public int UpdateCustomer(Guid customerId, Customer customer)
        {
            var dynamicParam = new DynamicParameters();

            foreach(var prop in customer.GetType().GetProperties())
            {
                var propName = prop.Name;
                var propValue = prop.GetValue(customer);
                var propType = prop.PropertyType;

                if(propType == typeof(Guid) || propType == typeof(Guid?))
                {
                    dynamicParam.Add($"@{propName}", propValue, DbType.String);
                } 
                else
                {
                    dynamicParam.Add($"@{propName}", propValue);
                }
            }

            int rowAffects = _dbConnection.Execute("Proc_UpdateCustomer", dynamicParam, commandType: CommandType.StoredProcedure);

            return rowAffects;
        }

        #endregion
    }
}

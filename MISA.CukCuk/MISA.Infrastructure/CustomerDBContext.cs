using Dapper;
using MISA.Infrastructure.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MISA.Infrastructure
{
    public class CustomerDBContext
    {
        #region Method
        /// <summary>
        /// Lấy toàn bộ thông tin khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// CreatedBy: NVTOAN 24/06/2021
        public IEnumerable<Customer> GetCustomers()
        {
            var connectionString = "User Id=dev;" +
                                   "Host=47.241.69.179;" +
                                   "Port=3306;" +
                                   "Password=12345678;" +
                                   "Database=MISACukCuk_Demo;" +
                                   "Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var customers = dbConnection.Query<Customer>("Proc_GetCustomers", commandType: CommandType.StoredProcedure);

            return customers;
        }
        
        /// <summary>
        /// Lấy thông tin khách hàng theo id
        /// </summary>
        /// <param name="id">Id của khách hàng</param>
        /// <returns>Object khách hàng</returns>
        /// CreatedBy: NVTOAN 25/06/2021
        public Customer GetCustomerById(string id)
        {
            var connectionString = "User Id=dev;" +
                                   "Host=47.241.69.179;" +
                                   "Port=3306;" +
                                   "Password=12345678;" +
                                   "Database=MISACukCuk_Demo;" +
                                   "Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var customer = dbConnection.Query<Customer>("Proc_GetCustomerById", new { CustomerId = id }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            return customer;
        }

        /// <summary>
        /// Lấy thông tin khách hàng theo số điện thoại
        /// </summary>
        /// <param name="phoneNumber">Id của khách hàng</param>
        /// <returns>Object khách hàng</returns>
        /// CreatedBy: NVTOAN 25/06/2021
        public Customer GetCustomerByPhone(string phoneNumber)
        {
            var connectionString = "User Id=dev;" +
                                   "Host=47.241.69.179;" +
                                   "Port=3306;" +
                                   "Password=12345678;" +
                                   "Database=MISACukCuk_Demo;" +
                                   "Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var customer = dbConnection.Query<Customer>("Proc_GetCustomerByPhoneNumber", new { PhoneNumber = phoneNumber }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            return customer;
        }

        /// <summary>
        /// Lấy thông tin khách hàng theo Email
        /// </summary>
        /// <param name="email">Id của khách hàng</param>
        /// <returns>Object khách hàng</returns>
        /// CreatedBy: NVTOAN 25/06/2021
        public Customer GetCustomerByEmail(string email)
        {
            var connectionString = "User Id=dev;" +
                                   "Host=47.241.69.179;" +
                                   "Port=3306;" +
                                   "Password=12345678;" +
                                   "Database=MISACukCuk_Demo;" +
                                   "Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var customer = dbConnection.Query<Customer>("Proc_GetCustomerByEmail", new { Email = email }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            return customer;
        }

        /// <summary>
        /// Thêm mới một khách hàng
        /// </summary>
        /// <param name="customer">Object khách hàng</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        /// CreatedBy: NVTOAN 25/06/2021
        public int InsertCustomer(Customer customer)
        {
            var connectionString = "User Id=dev;" +
                                    "Host=47.241.69.179;" +
                                    "Port=3306;" +
                                    "Password=12345678;" +
                                    "Database=MISACukCuk_Demo;" +
                                    "Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            int rowAffects = dbConnection.Execute("Proc_InsertCustomer", customer, commandType: CommandType.StoredProcedure);

            return rowAffects;
        }

        //Sửa thông tin khách hàng

        //Xóa khách hàng theo id

        #endregion
    }
}

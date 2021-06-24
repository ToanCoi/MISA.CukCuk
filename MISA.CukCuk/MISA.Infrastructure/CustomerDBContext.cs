using Dapper;
using MISA.Infrastructure.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
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
        //Lấy thông tin khách hàng theo id

        //Thêm mới khách hàng

        //Sửa thông tin khách hàng

        //Xóa khách hàng theo id

        #endregion
    }
}

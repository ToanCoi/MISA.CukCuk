using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using MySqlConnector;
using MISA.ApplicationCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.CukCuk.Web.Controllers
{
    [Route("api/v1/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        /// <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// CreatedBy: NVTOAN 24/06/2021
        [HttpGet]
        public IActionResult Get()
        {
            var customerService = new CustomerService();
            var customers = customerService.GetCustomers();

            return Ok(customers);
        }

        /// <summary>
        /// Lấy khách hàng theo id
        /// </summary>
        /// <param name="id">id của khách hàng</param>
        /// <returns>Một khách hàng tìm được theo id</returns>
        /// CreateedBy: NVTOAN 24/06/2021
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var connectionString = "User Id=dev;" +
                                    "Host=47.241.69.179;" +
                                    "Port=3306;" +
                                    "Password=12345678;" +
                                    "Database=MISACukCuk_Demo;" +
                                    "Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            var customer = dbConnection.Query<Customer>("Proc_GetCustomerById", new { CustomerId = id }, commandType: CommandType.StoredProcedure);
            return Ok(customer);
        }

        /// <summary>
        /// Thêm một nhân viên mới  
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            var connectionString = "User Id=dev;" +
                                    "Host=47.241.69.179;" +
                                    "Port=3306;" +
                                    "Password=12345678;" +
                                    "Database=MISACukCuk_Demo;" +
                                    "Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);

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
            if(res.Count() > 0)
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
            if(res.Count() > 0)
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
            if(res.Count() > 0)
            {
                var msg = new
                {
                    devMsg = new { fieldName = "Email", msg = "Email đã tồn tại" },
                    userMsg = "Email đã tồn tại",
                    Code = 999
                };

                return BadRequest(msg);
            }


            var rowAffects = dbConnection.Execute("Proc_InsertCustomer", customer, commandType: CommandType.StoredProcedure);
            if (rowAffects > 0)
            {
                return Created("tc", customer);
            }
            else
            {
                return NoContent();
            }
        }

        /// <summary>
        /// Sửa thông tin khách hàng
        /// </summary>
        /// <param name="id">id của khách hàng</param>
        /// <param name="customer">thông tin khách hàng cần sửa</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Customer customer)
        {
            return Ok();
        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var connectionString = "User Id=dev;" +
                                   "Host=47.241.69.179;" +
                                   "Port=3306;" +
                                   "Password=12345678;" +
                                   "Database=MISACukCuk_Demo;" +
                                   "Character Set=utf8";
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            var rowAffects = dbConnection.Query("Proc_DeleteCustomerById", new { CustomerId = id}, commandType: CommandType.StoredProcedure);

            return Delete(id);
        }
    }
}

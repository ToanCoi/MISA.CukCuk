using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using MySqlConnector;
using MISA.ApplicationCore;
using MISA.Entity.Models;
using MISA.Entity;

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
            var customerService = new CustomerService();
            var customer = customerService.GetCustomerById(id);

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
            var customerService = new CustomerService();

            var serviceResult = customerService.InsertCustomer(customer);

            if (serviceResult.Code == MISACode.Invalid)
            {
                return BadRequest(serviceResult);
            }
            else if (serviceResult.Code == MISACode.Success && (int)serviceResult.Data > 0)
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

            var rowAffects = dbConnection.Query("Proc_DeleteCustomerById", new { CustomerId = id }, commandType: CommandType.StoredProcedure);

            return Delete(id);
        }
    }
}

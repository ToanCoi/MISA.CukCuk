using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using MySqlConnector;
using MISA.ApplicationCore;
using MISA.ApplicationCore.Enum;
using MISA.ApplicationCore.Interface;
using MISA.ApplicationCore.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.CukCuk.Web.Controllers
{
    [Route("api/v1/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        #region Declare
        ICustomerService _customerService;
        #endregion

        #region Constructor
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        #endregion
        /// <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// CreatedBy: NVTOAN 24/06/2021
        [HttpGet]
        public IActionResult Get()
        {
            var customers = _customerService.GetCustomers();

            return Ok(customers);
        }

        /// <summary>
        /// Lấy khách hàng theo id
        /// </summary>
        /// <param name="id">id của khách hàng</param>
        /// <returns>Một khách hàng tìm được theo id</returns>
        /// CreateedBy: NVTOAN 24/06/2021
        [HttpGet("{customerId}")]
        public IActionResult Get([FromRoute]Guid customerId)
        {
            var customer = _customerService.GetCustomerById(customerId);

            return Ok(customer);
        }

        /// <summary>
        /// Thêm một nhân viên mới  
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]Customer customer)
        {

            var serviceResult = _customerService.InsertCustomer(customer);

            if (serviceResult.Code == MISACode.Invalid)
            {
                return BadRequest(serviceResult);
            }
            else if (serviceResult.Code == MISACode.Success && (int)serviceResult.Data > 0)
            {
                return Created("success", serviceResult);
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
        [HttpPut("{customerId}")]
        public IActionResult Put([FromRoute]Guid customerid, [FromBody] Customer customer)
        {

            var serviceResult = _customerService.UpdateCustomer(customerid, customer);

            customer.CustomerId = customerid;

            if (serviceResult.Code == MISACode.Invalid)
            {
                return BadRequest(serviceResult);
            }
            else if (serviceResult.Code == MISACode.Success && (int)serviceResult.Data > 0)
            {
                return Ok(serviceResult);
            }
            else
            {
                return NoContent();
            }
        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("{customerId}")]
        public IActionResult Delete(Guid customerId)
        {

            var serviceResult = _customerService.DeleteCustomer(customerId);


            if ((int)serviceResult.Data > 0)
            {
                return Ok(serviceResult);
            }
            else
            {
                return NoContent();
            }
        }
    }
}

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
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interface.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.CukCuk.Web.Controllers
{
    public class CustomersController : BaseController<Customer>
    {
        #region Declare
        ICustomerService _customerSerice;
        #endregion

        #region Constructor
        public CustomersController(ICustomerService customerService) : base(customerService)
        {
            _customerSerice = customerService;
        }
        #endregion
    }
}

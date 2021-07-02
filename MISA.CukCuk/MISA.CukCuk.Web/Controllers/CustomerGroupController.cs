using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.Web.Controllers
{
    public class CustomerGroupController : BaseController<CustomerGroup>
    {
        public CustomerGroupController(ICustomerGroupService customerGroupService) : base(customerGroupService)
        {

        }
    }
}

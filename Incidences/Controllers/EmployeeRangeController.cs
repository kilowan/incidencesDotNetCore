using Incidences.Business;
using Microsoft.AspNetCore.Mvc;
using Incidences.Models.Employee;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Incidences.Controllers
{
    [Route("api/EmployeeType")]
    [ApiController]
    public class EmployeeRangeController : ControllerBase
    {
        private readonly IEmployeeRangeBz employeeRange;
        public EmployeeRangeController(IEmployeeRangeBz employeeRange)
        {
            this.employeeRange = employeeRange;
        }

        [HttpGet]
        [Authorize]
        public IList<TypeRange> Index()
        {
            return this.employeeRange.GetEmployeeTypes();
        }

        [HttpGet("{id}")]
        [Authorize]
        public TypeRange Details(int id)
        {
            return this.employeeRange.SelectRangeById(id);
        }
    }
}

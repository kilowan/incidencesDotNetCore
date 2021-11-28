using Incidences.Business;
using Incidences.Data;
using Microsoft.AspNetCore.Mvc;
using Incidences.Models.Employee;
using System.Collections.Generic;

namespace Incidences.Controllers
{
    [Route("api/EmployeeType")]
    [ApiController]
    public class EmployeeRangeController : ControllerBase
    {
        private readonly IEmployeeRangeBz employeeRange;
        private readonly ISqlBase sql;
        public EmployeeRangeController(IEmployeeRangeBz employeeRange, ISqlBase sql)
        {
            this.employeeRange = employeeRange;
            this.sql = sql;
        }

        [HttpGet]
        public IList<TypeRange> Index()
        {
            return this.employeeRange.GetEmployeeTypes();
        }

        [HttpGet("{id}")]
        public TypeRange Details(int id)
        {
            return this.employeeRange.SelectRangeById(id);
        }
    }
}

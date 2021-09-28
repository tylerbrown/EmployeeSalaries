using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmployeeSalaries.Controllers
{
    public class EmployeeSearchSalaryByDateRangeController : ApiController
    {
        // Function that makes call to Stored Procedure
        public IHttpActionResult getEmployeeRecords(DateTime beginDate, DateTime endDate)
        {
            // Create connection entity
            EmployeeEntities empEntities = new EmployeeEntities();

            // Make Stored Procedure call via entity data model
            var result = empEntities.SearchEmployeeSalaryByDateRange(beginDate, endDate).ToList();

            // Return result
            return Ok(result);
        }
    }
}

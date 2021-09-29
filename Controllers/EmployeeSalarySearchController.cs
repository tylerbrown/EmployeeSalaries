using EmployeeSalaries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmployeeSalaries.Controllers
{
    public class EmployeeSalarySearchController : ApiController
    {
        /* 
         This method will take a date and return the most recent employee information prior to the date
         */
        public IHttpActionResult getEmployeeRecords(DateTime? selectedDate)
        {
            // Create connection entity
            EmployeeEntities empEntities = new EmployeeEntities();

            // Make Stored Procedure call via entity data model
            var result = empEntities.SearchSalaryByDate(selectedDate).ToList();

            // Return result
            return Ok(result);
        }
    }
}

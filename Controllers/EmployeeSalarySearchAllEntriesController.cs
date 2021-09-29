using EmployeeSalaries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmployeeSalaries.Controllers
{
    public class EmployeeSalarySearchAllEntriesController : ApiController
    {
        /* 
         This method will take a date and return ALL entries from the salary table prior to the date 
         */
        public IHttpActionResult getAllEmployeeRecords(DateTime? selectedDate)
        {
            // Create connection entity
            EmployeeEntities empEntities = new EmployeeEntities();

            // Make Stored Procedure call via entity data model
            var result = empEntities.SearchAllEntries(selectedDate).ToList();

            // Return result
            return Ok(result);
        }
    }
}

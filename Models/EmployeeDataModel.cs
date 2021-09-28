using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeSalaries.Models
{
    public class EmployeeDataModel
    {
        public IEnumerable<EmployeeDisplayModel> EmployeeData { get; set; }
        public String testName { get; set; }
        public long totalSalary { get; set; }
    }
}
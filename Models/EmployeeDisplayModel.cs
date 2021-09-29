using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeSalaries.Models
{
    public class EmployeeDisplayModel
    {
        public IEnumerable<EmployeeDataModel> EmployeeData { get; set; }
        public String queryDate { get; set; }
        public int employeeCount { get; set; }
        public long totalSalary { get; set; }
        public double averageSalary { get; set; }
    }
}
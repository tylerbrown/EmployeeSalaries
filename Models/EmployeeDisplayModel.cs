using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeSalaries.Models
{
    public class EmployeeDisplayModel
    {
        public int EmpID { get; set; }
        public String Name { get; set; }
        public long Salary { get; set; }
        public System.DateTime Date { get; set; }
        public int Id { get; set; }
    }
}
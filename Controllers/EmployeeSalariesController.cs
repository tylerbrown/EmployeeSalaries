using EmployeeSalaries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace EmployeeSalaries.Controllers
{
    public class EmployeeSalariesController : Controller
    {
        // Define model as volatile so that it can be modified by multiple threads
        private volatile EmployeeDataModel salaryTable;

        // GET: EmployeeSalaries
        public ActionResult Index(DateTime? selectedDate)
        {
           // if (selectedDate == null)
            {
                //selectedDate = DateTime.Today;
            }



            //IEnumerable<EmployeeDisplayModel> salaryTable = null;
             salaryTable = new EmployeeDataModel();
            salaryTable.testName = "TEST!";
            
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44354/api/");

            // Call inmported function call to Stored Procedure
            var callAPI = client.GetAsync("EmployeeSalarySearch?selectedDate=" + selectedDate);
            callAPI.Wait();

            var returnData = callAPI.Result;

            if (returnData.IsSuccessStatusCode)
            {
                var displayEmployees = returnData.Content.ReadAsAsync<IList<EmployeeDisplayModel>>();
                displayEmployees.Wait();
                salaryTable.EmployeeData = displayEmployees.Result;
            }

            Thread totalSalaryThread = new Thread(new ThreadStart(calculateTotalEmployeeSalary));
            totalSalaryThread.Start();


            // join so that page will wait untill thread are done
            totalSalaryThread.Join();

            return View(salaryTable);
        }

        public void calculateTotalEmployeeSalary()
        {
            salaryTable.testName = "THREAD changed it";

            long totalSalary = 0;

            foreach (var employee in salaryTable.EmployeeData)
            {
                totalSalary += employee.Salary;
            }

            salaryTable.totalSalary = totalSalary;
        }
    }
}
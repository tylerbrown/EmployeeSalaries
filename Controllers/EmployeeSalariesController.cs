using EmployeeSalaries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web.Mvc;

namespace EmployeeSalaries.Controllers
{
    public class EmployeeSalariesController : Controller
    {
        // Define model as volatile so that it can be modified by multiple threads
        private volatile EmployeeDisplayModel employeeDataDisplay;

        /* GET: EmployeeSalaries
         * This method takes a date and will create a data model that contains 
         *  a list of employees with their names, salaries, and salary date as well
         *  as an employee count, combine average salary, and total salary for the given date
         */
        public ActionResult Index(DateTime? selectedDate)
        {
            // Create the model object that will hold all the employee data
            employeeDataDisplay = new EmployeeDisplayModel();

            // Check if no date is selected
            if (selectedDate == null)
            {
                // Default to todays date
                selectedDate = DateTime.Today;
            }

            // Update the data model with the selected date
            employeeDataDisplay.queryDate = selectedDate.Value.ToString("yyyy-MM-dd");

            // Create an http client to make api calls
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44354/api/");

            // Make api call to get latest salaries prior to selected date
            var callAPI = client.GetAsync("EmployeeSalarySearch?selectedDate=" + selectedDate);
            callAPI.Wait();

            // Get the data from the api call
            var returnData = callAPI.Result;

            // Check for success
            if (returnData.IsSuccessStatusCode)
            {
                // Read in the employee data
                var employeeData = returnData.Content.ReadAsAsync<IList<EmployeeDataModel>>();
                employeeData.Wait();
                var employeeDataResult = employeeData.Result;

                // Store the employee data of the top 1000 salaries
                employeeDataDisplay.EmployeeData = employeeDataResult.Take(1000);

                // Store the employee count
                employeeDataDisplay.employeeCount = employeeDataResult.Count;

                // Create total and average threads
                Thread averageSalaryThread = new Thread(() => calculateCombinedAverageSalaries(employeeDataResult));
                Thread totalSalaryThread = new Thread(() => calculateSalariesTotal(employeeDataResult));

                // Start the threads
                averageSalaryThread.Start();
                totalSalaryThread.Start();

                // Wait for threads to complete
                averageSalaryThread.Join();
                totalSalaryThread.Join();
            }

            return View(employeeDataDisplay);
        }

        /* 
         This method will take a list of employees and calculate their average salaries
         */
        public void calculateCombinedAverageSalaries(IEnumerable<EmployeeDataModel> allEmployeeSalaries)
        {
            // Define variable
            long totalSalary = 0;
            long averageSalary = 0;

            // Check if there are entries
            if (allEmployeeSalaries.Count() > 0)
            {
                // Loop through all employee salaries
                foreach (var employee in allEmployeeSalaries)
                {
                    // Add employee salary to the total salary
                    totalSalary += employee.Salary;
                }

                // Calculate average salary
                averageSalary = totalSalary / allEmployeeSalaries.Count();
            }

            // Store average salary
            employeeDataDisplay.averageSalary = averageSalary;
        }

        /* 
         This method will take a list of employees and calculate their total salaries
         */
        public void calculateSalariesTotal(IEnumerable<EmployeeDataModel> allEmployeeSalaries)
        {
            // Define variable
            long totalSalary = 0;

            // Loop through all employee salaries
            foreach (var employee in allEmployeeSalaries)
            {
                // Add employee salary to the total salary
                totalSalary += employee.Salary;
            }

            // Store total salary
            employeeDataDisplay.totalSalary = totalSalary;
        }
    }
}
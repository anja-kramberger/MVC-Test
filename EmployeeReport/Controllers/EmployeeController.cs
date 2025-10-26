using EmployeeReport.Models;
using EmployeeReport.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;

namespace EmployeeReport.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;
        private const string ApiKey = "vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";
        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        public async Task<IActionResult> Index()
        {

            ViewBag.EmployeeTotals = await GetEmployeeTotals();
            return View();
        }

        private async Task<List<EmployeeTotal>> GetEmployeeTotals()
        {
            var entries = await _employeeService.GetEmployeesAsync(ApiKey);

            var totals = entries
                .Where(t => t.DeletedOn == null)
                .GroupBy(g => string.IsNullOrEmpty(g.EmployeeName) ? "Unassigned" : g.EmployeeName)
                .Select(s => new EmployeeTotal
                {
                    Name = s.Key,
                    TotalHours = s.Sum(e => (e.EndTimeUtc - e.StarTimeUtc).TotalHours)
                })
                .OrderByDescending(x => x.TotalHours)
                .ToList();

            return totals;
        }
        

    }
}

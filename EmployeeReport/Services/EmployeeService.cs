using EmployeeReport.Models;
using System.Text.Json;

namespace EmployeeReport.Services
{
    public class EmployeeService
    {
        private readonly HttpClient _httpClient;

        public EmployeeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Employee>> GetEmployeesAsync(string apiKey)
        {
            var url = $"https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code={apiKey}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to retrieve data from API");
            }

            var json = await response.Content.ReadAsStringAsync();
            var employees = JsonSerializer.Deserialize<List<Employee>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return employees;
        }
    }
}

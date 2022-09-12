using System.Collections.Generic;
using System.Threading.Tasks;
using Employee.Modal.Dto;
using Employee.Modal.Entities;

namespace Employee.Services.Interface
{
    public interface IEmployeeService
    {
        //List<Employee> GetAllEmployee(string name);
        Task<IEnumerable<ResponseViewModel>> GetAllEmployee(string username, string name);
        Task<ResponseViewModel> GetEmployee(int id);
        Task<bool> Create(EmployeeEntity employee);
        Task<bool> Update(int id, EmployeeUpdateRequest request);
        Task<bool> Delete(int id);
        Task<List<string>> ListUsername();
    }
}

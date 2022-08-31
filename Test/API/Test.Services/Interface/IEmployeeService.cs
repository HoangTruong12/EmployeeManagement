using System.Collections.Generic;
using System.Threading.Tasks;
using Test.Modal.Dto;
using Test.Modal.Entities;

namespace Test.Services.Interface
{
    public interface IEmployeeService
    {
        //List<Employee> GetAllEmployee(string name);
        Task<IEnumerable<ResponseViewModel>> GetAllEmployee(string username, string name);
        Task<ResponseViewModel> GetEmployee(int id);
        Task<bool> Create(Employee employee);
        Task<bool> Update(int id, Employee employee);
        Task<bool> Delete(int id);
        Task<List<string>> ListUsername();
    }
}

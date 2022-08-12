using System.Collections.Generic;
using System.Threading.Tasks;
using Test.Modal.Entities;

namespace Test.Services.Interface
{
    public interface IEmployeeService
    {
        //List<Employee> GetAllEmployee(string name);
        Task<IEnumerable<Employee>> GetAllEmployee(string id, string name);
        Task<Employee> GetEmployee(int id);
        Task<bool> Create(Employee employee);
        Task<bool> Update(int id, Employee employee);
        Task<bool> Delete(int id);
        bool CheckExistsUsernameEmployee(string username);
        bool CheckExistsEmailEmployee(string email);
        bool CheckExistsPhoneNumberEmployee(string phoneNumber);
    }
}

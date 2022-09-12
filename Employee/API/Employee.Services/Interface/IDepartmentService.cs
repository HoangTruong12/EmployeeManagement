using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Employee.Modal.Entities;

namespace Employee.Services.Interface
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAllDepartment();
        Task<Department> GetDepartment(int id);
        Task<List<Department>> ListDepartment();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Data.Repository;
using Test.Data.UnitOfWork;
using Test.Modal.Entities;
using Test.Services.Interface;

namespace Test.Services.Implement
{
    public class RegisterService : BaseService, IRegisterService
    {
        private readonly IRepository<Employee> _empRepo;

        public RegisterService(IUnitOfWork unitOfWork, IRepository<Employee> empRepo) : base(unitOfWork)
        {
            _empRepo = empRepo;
        }

        public async Task<bool> Register(Employee employee)
        {
            try
            {
                var result = new Employee
                {
                    Username = employee.Username,
                    //Password = employee.Password,
                    Password = BCrypt.Net.BCrypt.HashPassword(employee.Password),
                    Name = employee.Name,
                    Birthday = employee.Birthday,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber,
                    DepartmentId = employee.DepartmentId,  
                };

                UnitOfWork.BeginTransaction();
                await _empRepo.Add(result);
                UnitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                UnitOfWork.Rollback();
                throw new Exception(ex.Message);
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Data.Repository;
using Employee.Data.UnitOfWork;
using Employee.Modal.Entities;
using Employee.Services.Interface;

namespace Employee.Services.Implement
{
    public class RegisterService : BaseService, IRegisterService
    {
        private readonly IRepository<EmployeeEntity> _empRepo;

        public RegisterService(IUnitOfWork unitOfWork, IRepository<EmployeeEntity> empRepo) : base(unitOfWork)
        {
            _empRepo = empRepo;
        }

        public async Task<bool> Register(EmployeeEntity employee)
        {
            try
            {
                var result = new EmployeeEntity
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

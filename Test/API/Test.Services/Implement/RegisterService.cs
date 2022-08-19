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

        public bool CheckExistsEmailEmployee(string email)
        {
            try
            {
                var checkExistEmail = _empRepo.Get(x => x.Email == email);

                return (checkExistEmail != null && checkExistEmail.Count() > 0);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool CheckExistsPhoneNumberEmployee(string phoneNumber)
        {
            try
            {
                var checkExistPhoneNumber = _empRepo.Get(x => x.PhoneNumber == phoneNumber);

                return (checkExistPhoneNumber != null && checkExistPhoneNumber.Count() > 0);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool CheckExistsUsernameEmployee(string username)
        {
            try
            {
                var checkExistUsername = _empRepo.Get(x => x.Username == username);

                return (checkExistUsername != null && checkExistUsername.Count() > 0);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

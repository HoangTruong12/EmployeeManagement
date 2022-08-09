using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Data.Repository;
using Test.Data.UnitOfWork;
using Test.Modal.Entities;
using Test.Services.Interface;

namespace Test.Services.Implement
{
    public class EmployeeService : BaseService, IEmployeeService
    {
        private readonly IRepository<Employee> _empRepo;

        public EmployeeService(IUnitOfWork unitOfWork, IRepository<Employee> empRepo) : base(unitOfWork)
        {
            _empRepo = empRepo;
        }

        public List<Employee> GetAllEmployee()
        {
            var listEmployees = _empRepo.GetAll();

            return listEmployees.Select(x => new Employee
            {
                Id = x.Id,
                Username = x.Username,
                Password = x.Password,
                Name = x.Name,
                Birthday = x.Birthday,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber
            }).ToList();
        }

        public async Task<Employee> GetEmployee(int id)
        {
            return await _empRepo.Get(id);
        }

        public async Task<bool> Create(Employee employee)
        {
            try
            {
                //CheckExistsEmployee(employee);

                var result = new Employee
                {
                    Username = employee.Username,
                    //Password = employee.Password,
                    Password = BCrypt.Net.BCrypt.HashPassword(employee.Password),
                    Name = employee.Name,
                    Birthday = employee.Birthday,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber
                };

                UnitOfWork.BeginTransaction();
                await _empRepo.Add(result);
                UnitOfWork.Commit();
                return true;
            }
            catch(Exception ex)
            {
                UnitOfWork.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Update(int id, Employee employee)
        {
            try
            {
                var existingEmployee = _empRepo.GetAll().FirstOrDefault(x => x.Id == id);

                if (existingEmployee == null)
                    await _empRepo.Add(employee);

                //existingEmployee.Id = id;
                existingEmployee.Username = employee.Username;
                existingEmployee.Password = BCrypt.Net.BCrypt.HashPassword(employee.Password);
                existingEmployee.Name = employee.Name;
                existingEmployee.Birthday = employee.Birthday;
                existingEmployee.Email = employee.Email;
                existingEmployee.PhoneNumber = employee.PhoneNumber;

                UnitOfWork.BeginTransaction();
                await _empRepo.Update(existingEmployee);
                UnitOfWork.Commit();

                return true;
            }
            catch
            {
                UnitOfWork.Rollback();
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                await _empRepo.Delete(id);

                return true;
            }
            catch
            {
                UnitOfWork.Rollback();
                return false;
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

                //if (checkExist != null && checkExist.Count() > 0)
                //{
                //    return true;
                //}

                //return false;
                return (checkExistUsername != null && checkExistUsername.Count() > 0);
                //return _empRepo.GetAll().Any(x => x.Username == employee.Username);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

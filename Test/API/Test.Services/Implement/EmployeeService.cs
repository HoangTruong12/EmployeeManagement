﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Data.Repository;
using Test.Data.UnitOfWork;
using Test.Modal.Dto;
using Test.Modal.Entities;
using Test.Services.Interface;

namespace Test.Services.Implement
{
    public class EmployeeService : BaseService, IEmployeeService
    {
        private readonly IRepository<Employee> _empRepo;
        private readonly IRepository<Department> _depRepo;

        public EmployeeService(IUnitOfWork unitOfWork, IRepository<Employee> empRepo, IRepository<Department> depRepo) : base(unitOfWork)
        {
            _empRepo = empRepo;
            _depRepo = depRepo;
        }

        //public async Task<Employee> GetAllEmployee()
        //{
        //    var listEmployees = _empRepo.GetAll();

        //    return result = listEmployees.Select(x => new Employee
        //    {
        //        Id = x.Id,
        //        Username = x.Username,
        //        Password = x.Password,
        //        Name = x.Name,
        //        Birthday = x.Birthday,
        //        Email = x.Email,
        //        PhoneNumber = x.PhoneNumber
        //    }).ToList();

        //}

        public async Task<IEnumerable<ResponseViewModel>> GetAllEmployee(string employeeId, string name)
        {
            try
            {
                var employees = _empRepo.GetAll().ToList();
                var departments = _depRepo.GetAll().ToList();

                var query = (from e in employees // outer sequence
                             join d in departments //inner sequence 
                             on e.DepartmentId equals d.Id // key selector 
                             select new ResponseViewModel
                             { // result selector 
                                 Id = e.Id,
                                 Username = e.Username,
                                 Password = e.Password,
                                 Name = e.Name,
                                 Birthday = e.Birthday,
                                 Email = e.Email,
                                 PhoneNumber = e.PhoneNumber,
                                 DepartmentId = d.Id,
                                 DepartmentName = d.DepartmentName
                             }).ToList();

                if (!string.IsNullOrEmpty(employeeId) || !string.IsNullOrEmpty(name))
                {
                    query = query.Where(x => (x.Id.ToString() == employeeId || employeeId == null) && (x.Name.Contains(name) || name == null)).ToList();
                }

                return query;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseViewModel> GetEmployee(int id)
        {
            var employees = _empRepo.GetAll().ToList();
            var departments = _depRepo.GetAll().ToList();

            var query = (from e in employees // outer sequence
                         join d in departments //inner sequence 
                         on e.DepartmentId equals d.Id // key selector 
                         select new ResponseViewModel
                         { // result selector 
                             Id = e.Id,
                             Username = e.Username,
                             Password = e.Password,
                             Name = e.Name,
                             Birthday = e.Birthday,
                             Email = e.Email,
                             PhoneNumber = e.PhoneNumber,
                             DepartmentId = d.Id,
                             DepartmentName = d.DepartmentName
                         }).ToList();

            var employee = await _empRepo.Get(id);
            if(employee == null)
            {
                return null;
            }

            var result = query.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId);
            if (result == null)
            { 
                return null;
            }
            return result;
        }

        public async Task<bool> Create(Employee employee)
        {
            try
            {
                //CheckExistsEmployee(employee);

                var result = new Employee
                {
                    Username = employee.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(employee.Password),
                    Name = employee.Name,
                    Birthday = employee.Birthday,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber,
                    DepartmentId = employee.DepartmentId
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
                existingEmployee.DepartmentId = employee.DepartmentId;

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

        public async Task<List<string>> ListUsername()
        {
            try
            {
                var list = _empRepo.GetAll().Select(u => u.Username).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}

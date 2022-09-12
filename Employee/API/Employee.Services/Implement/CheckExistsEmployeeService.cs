using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Employee.Data.Repository;
using Employee.Data.UnitOfWork;
using Employee.Modal.Entities;
using Employee.Services.Interface;

namespace Employee.Services.Implement
{
    public class CheckExistsEmployeeService : BaseService, ICheckExistsEmployeeService
    {
        private readonly IRepository<EmployeeEntity> _empRepo;

        public CheckExistsEmployeeService(IUnitOfWork unitOfWork, IRepository<EmployeeEntity> empRepo) : base(unitOfWork)
        {
            _empRepo = empRepo;
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

        public bool CheckExistPhoneNumberWhenUpdate(int id, string phoneNumber)
        {
            try
            {
                bool result = false;
                var checkPhone = _empRepo.GetAll().FirstOrDefault(x => x.PhoneNumber == phoneNumber);
                if(checkPhone == null)
                {
                    return result;
                }
                return result = checkPhone.Id == id ? false : true;          
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckExistEmailWhenUpdate(int id, string email)
        {
            try
            {
                bool rs = false;
                var checkEmail = _empRepo.GetAll().FirstOrDefault(x => x.Email == email);
                if (checkEmail == null)
                {
                    return rs;
                }
                return rs = checkEmail.Id == id ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

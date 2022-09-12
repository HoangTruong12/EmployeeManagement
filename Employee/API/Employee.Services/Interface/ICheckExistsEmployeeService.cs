using System;
using System.Collections.Generic;
using System.Text;

namespace Employee.Services.Interface
{
    public interface ICheckExistsEmployeeService
    {
        bool CheckExistsUsernameEmployee(string username);
        bool CheckExistsEmailEmployee(string email);
        bool CheckExistsPhoneNumberEmployee(string phoneNumber);
        bool CheckExistPhoneNumberWhenUpdate(int id, string phoneNumber);
        bool CheckExistEmailWhenUpdate(int id, string email);
        bool CheckDepartmentWhenUpdate(int id);
    }
}

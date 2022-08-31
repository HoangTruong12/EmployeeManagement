using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Services.Interface
{
    public interface ICheckExistsEmployeeService
    {
        bool CheckExistsUsernameEmployee(string username);
        bool CheckExistsEmailEmployee(string email);
        bool CheckExistsPhoneNumberEmployee(string phoneNumber);
    }
}

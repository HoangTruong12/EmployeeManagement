using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Employee.Modal.Dto;

namespace Employee.Services.Interface
{
    public interface ILoginService
    {
        Task<string> Login(LoginDto login);

        LoginDto Authenticate(LoginDto login);
    }
}

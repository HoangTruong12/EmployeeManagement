using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Modal.Dto;

namespace Test.Services.Interface
{
    public interface ILoginService
    {
        Task<string> Login(LoginDto login);

        LoginDto Authenticate(LoginDto login);
    }
}

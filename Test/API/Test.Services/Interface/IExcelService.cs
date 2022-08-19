using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Modal.Dto;
using Test.Modal.Entities;

namespace Test.Services.Interface
{
    public interface IExcelService
    {
        string Download();
        Task<bool> Import(IFormFile file);
        bool CheckIfExcelFile(IFormFile file);
    }
}

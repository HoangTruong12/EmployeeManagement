﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Employee.Modal.Dto;
using Employee.Modal.Entities;

namespace Employee.Services.Interface
{
    public interface IExcelService
    {
        string Download();
        Task<bool> Import(IFormFile file);
        bool CheckIfExcelFile(IFormFile file);
    }
}

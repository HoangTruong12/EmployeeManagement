using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Employee.Modal.Entities;
using Employee.Services.Interface;

namespace Employee.WebAPI.Controllers
{
    [Authorize]
    public class ExcelController : BaseController
    {
        private readonly IExcelService _excelService;
        public ExcelController(IExcelService excelService)
        {
            _excelService = excelService;
        }

        [HttpGet("download")]
        public string Download()
        {
            var content = _excelService.Download();
            return content;
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            bool checkIfExcelFile = _excelService.CheckIfExcelFile(file);
            if (checkIfExcelFile)
            {
                //var import = await _excelService.Import(file);
                //if (!import)
                //{
                //    return BadRequest("Username already exist");
                //}
                await _excelService.Import(file);
                return Ok("Uploaded success");
            }
            else
            {
                return BadRequest("Invalid file extension");
            }
        }
    }
}


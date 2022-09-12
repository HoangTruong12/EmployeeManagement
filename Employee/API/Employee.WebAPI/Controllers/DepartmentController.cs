using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee.Services.Interface;

namespace Employee.WebAPI.Controllers
{
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentService _depService;
        public DepartmentController(IDepartmentService depService)
        {
            _depService = depService;
        }

        [HttpGet("GetDepartments")]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                var deps = await _depService.GetAllDepartment();
                return Ok(deps);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // GET: DepartmentController/Details/5
        [HttpGet("GetDepartment/{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var item = await _depService.GetDepartment(id);
            if (item == null)
                return NotFound($"Department with Id: {id} was not found");

            return Ok(item);
        }

        [HttpGet("getListDepartment")]
        public async Task<IActionResult> ListUsername()
        {
            var list = await _depService.ListDepartment();
            if (list != null)
            {
                return Ok(list);
            }
            return NotFound();
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee.Modal.Dto;
using Employee.Modal.Entities;
using Employee.Services.Interface;

namespace Employee.WebAPI.Controllers
{
    [Authorize]
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService _empService;
        private readonly ICheckExistsEmployeeService _checkExistsService;
        public EmployeeController(IEmployeeService empService, ICheckExistsEmployeeService checkExistsService)
        {
            _empService = empService;
            _checkExistsService = checkExistsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees(string username, string name)
        {
            try
            {
                var employees = await _empService.GetAllEmployee(username, name);

                return Ok(employees);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("getId/{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var item = await _empService.GetEmployee(id);
            if (item == null)
                return NotFound($"Employee with Id: {id} was not found");

            return Ok(item);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(EmployeeEntity employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var checkUsername = _checkExistsService.CheckExistsUsernameEmployee(employee.Username);
                    if (checkUsername)
                    {
                        return BadRequest("Username already exist");
                    }

                    var checkEmail = _checkExistsService.CheckExistsEmailEmployee(employee.Email);
                    if (checkEmail)
                    {
                        return BadRequest("Email already exist");
                    }

                    var checkPhoneNumber = _checkExistsService.CheckExistsPhoneNumberEmployee(employee.PhoneNumber);
                    if (checkPhoneNumber)
                    {
                        return BadRequest("Phone number already exist");
                    }

                    await _empService.Create(employee);
                    return CreatedAtAction("GetEmployee", new { Id = employee.Id }, employee);
                }
                return new JsonResult("Create Employee Error") { StatusCode = 500 };
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, EmployeeUpdateRequest request)
        {

            var item = await _empService.GetEmployee(id); 
            if(item == null)
            {
                return NotFound($"Employee with Id: {id} was not found");
            }

            var checkPhoneNumber = _checkExistsService.CheckExistPhoneNumberWhenUpdate(id, request.PhoneNumber);
            if (checkPhoneNumber)
            {
                return BadRequest("Phone number already exist");
            }

            var checkEmail = _checkExistsService.CheckExistEmailWhenUpdate(id, request.Email);
            if (checkEmail)
            {
                return BadRequest("Email already exist");
            }

            var checkDepartmentId = _checkExistsService.CheckDepartmentWhenUpdate(request.DepartmentId);
            if (checkDepartmentId)
            {
                return BadRequest("DepartmentId was not found");
            }

            if (item != null)
            {
                await _empService.Update(id, request);
                return Ok(request);
            }

            return NotFound($"Employee with Id: {id} was not found");
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var item = await _empService.GetEmployee(id);
                if (item == null)
                {
                    return NotFound($"Employee with Id: {id} was not found");
                }

                await _empService.Delete(id);

                return Ok($"Deleted employee with Id: {id} success");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet("getListUsername")]
        public async Task<IActionResult> ListUsername()
        {
            var list = await _empService.ListUsername();
            if (list != null)
            {
                return Ok(list);
            }
            return NotFound();
        }

    }
}

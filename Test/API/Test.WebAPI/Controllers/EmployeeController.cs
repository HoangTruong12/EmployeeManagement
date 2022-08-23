using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Test.Modal.Entities;
using Test.Services.Interface;

namespace Test.WebAPI.Controllers
{
    [Authorize]
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService _empService;
        public EmployeeController(IEmployeeService empService)
        {
            _empService = empService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees(string employeeId, string name)
        {
            try
            {
                var employees = await _empService.GetAllEmployee(employeeId, name);

                return Ok(employees);
            }
            catch(Exception ex)
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
        public async Task<IActionResult> Create(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var checkUsername = _empService.CheckExistsUsernameEmployee(employee.Username);     
                    if (checkUsername)
                    {
                        return BadRequest("Username already exist");
                    }

                    var checkEmail = _empService.CheckExistsEmailEmployee(employee.Email);
                    if (checkEmail)
                    {
                        return BadRequest("Email already exist");
                    }

                    var checkPhoneNumber = _empService.CheckExistsPhoneNumberEmployee(employee.PhoneNumber);
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
        public async Task<IActionResult> Update(int id, Employee employee)
        {
            var item = await _empService.GetEmployee(id);

            if(item != null)
            {
                await _empService.Update(id, employee);
                return Ok(item);
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
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        //[HttpGet("{search}")]
        //public async Task<ActionResult<Employee>> Search(string name)
        //{
        //    try
        //    {
        //        var result = await _empService.Search(name);
        //        if (result.Any())
        //        {
        //            return Ok(result);
        //        }

        //        return NotFound();
        //    }
        //    catch(Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

    }
}

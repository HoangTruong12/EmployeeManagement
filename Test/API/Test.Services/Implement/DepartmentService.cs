using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Data.Repository;
using Test.Data.UnitOfWork;
using Test.Modal.Dto;
using Test.Modal.Entities;
using Test.Services.Interface;

namespace Test.Services.Implement
{
    public class DepartmentService : BaseService, IDepartmentService
    {
        private readonly IRepository<Department> _depRepo;

        public DepartmentService(IUnitOfWork unitOfWork, IRepository<Department> depRepo) : base(unitOfWork)
        {
            _depRepo = depRepo;
        }

        public async Task<IEnumerable<Department>> GetAllDepartment()
        {
            var departments = _depRepo.GetAll();
           var result = departments.Select(x => new Department
            {
               Id = x.Id,
               DepartmentName = x.DepartmentName
            }).ToList();

            return result;
        }

        public async Task<Department> GetDepartment(int id)
        {
            return await _depRepo.Get(id);
        }

        public async Task<List<Department>> ListDepartment()
        {
            try
            {
                var list = _depRepo.GetAll().ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}

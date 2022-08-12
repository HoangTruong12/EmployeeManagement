using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Test.Data.Repository;
using Test.Data.UnitOfWork;
using Test.Modal.Entities;
using Test.Services.Interface;

namespace Test.Services.Implement
{
    public class ExcelService : BaseService, IExcelService
    {
        private readonly IRepository<Employee> _empRepo;
        private readonly IConfiguration _configuration;

        public ExcelService(IUnitOfWork unitOfWork, IRepository<Employee> empRepo, IConfiguration configuration)
            : base(unitOfWork)
        {
            _configuration = configuration;
            _empRepo = empRepo;
        }

        public string Download()
        {
            using (var stream = new MemoryStream())
            {
                List<Employee> employees = _empRepo.GetAll().ToList();
                var dataTable = CommonMethods.ConvertListToDataTable(employees);
                dataTable.Columns.Remove("Id");

                byte[] fileContents = null;

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {

                    ExcelWorksheet worksheet = pck.Workbook.Worksheets.Add("Employees");

                    worksheet.Cells["A1"].Value = "Username";
                    worksheet.Cells["A1"].Style.Font.Bold = true;
                    worksheet.Cells["A1"].Style.Font.Size = 16;
                    worksheet.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    worksheet.Cells["B1"].Value = "Password";
                    worksheet.Cells["B1"].Style.Font.Bold = true;
                    worksheet.Cells["B1"].Style.Font.Size = 16;
                    worksheet.Cells["B1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["B1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    worksheet.Cells["C1"].Value = "Name";
                    worksheet.Cells["C1"].Style.Font.Bold = true;
                    worksheet.Cells["C1"].Style.Font.Size = 16;
                    worksheet.Cells["C1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["C1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    worksheet.Cells["D1"].Value = "Birthday";
                    worksheet.Cells["D1"].Style.Font.Bold = true;
                    worksheet.Cells["D1"].Style.Font.Size = 16;
                    worksheet.Cells["D1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["D1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    worksheet.Cells["E1"].Value = "Email";
                    worksheet.Cells["E1"].Style.Font.Bold = true;
                    worksheet.Cells["E1"].Style.Font.Size = 16;
                    worksheet.Cells["E1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["E1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    worksheet.Cells["F1"].Value = "PhoneNumber";
                    worksheet.Cells["F1"].Style.Font.Bold = true;
                    worksheet.Cells["F1"].Style.Font.Size = 16;
                    worksheet.Cells["F1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    pck.Save();
                    fileContents = pck.GetAsByteArray();
                }
                return Convert.ToBase64String(fileContents);
            }
        }

        public async Task<bool> Upload(IFormFile file)
        {

            bool isSaveSuccess = false;
            string fileName;

            try
            {
                DataTable dt = new DataTable();
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.

                var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files");

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    var employee = new Employee
                    {
                        Username = file.ContentType,
                        Password = file.ContentType
                    };

                    await file.CopyToAsync(stream);
                    //dt = ConvertFileToDataTable(stream);

                    UnitOfWork.BeginTransaction();

                   // _empRepo.AddRange();
                    await _empRepo.Add(employee);
                    UnitOfWork.Commit();
                }
                isSaveSuccess = true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return isSaveSuccess;
        }

        //public DataTable ConvertFileToDataTable(MemoryStream stream)
        //{
        //    return Encoding.UTF8.(stream.ToArray());
        //}

        public bool CheckIfExcelFile(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension == ".xlsx" || extension == ".xls");
        }
    }
}

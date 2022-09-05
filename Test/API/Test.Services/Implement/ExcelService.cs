using ClosedXML.Excel;
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
using Test.Modal;
using Test.Modal.Entities;
using Test.Services.Interface;

namespace Test.Services.Implement
{
    public class ExcelService : BaseService, IExcelService
    {
        private readonly IRepository<Employee> _empRepo;

        public ExcelService(IUnitOfWork unitOfWork, IRepository<Employee> empRepo)
            : base(unitOfWork)
        {
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

                    worksheet.Cells["G1"].Value = "DepartmentId";
                    worksheet.Cells["G1"].Style.Font.Bold = true;
                    worksheet.Cells["G1"].Style.Font.Size = 16;
                    worksheet.Cells["G1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["G1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    pck.Save();
                    fileContents = pck.GetAsByteArray();
                }
                return Convert.ToBase64String(fileContents);
            }
        }

        public async Task<bool> Import(IFormFile file)
        {
            bool isSaveSuccess = false;
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                var fileName = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files", fileName);
                using (FileStream fs = File.Create(filePath))
                {
                    file.CopyTo(fs);
                }
                int rowNo = 1;
                XLWorkbook workbook = XLWorkbook.OpenFromTemplate(filePath);
                var sheets = workbook.Worksheets.First();
                var rows = sheets.Rows().ToList();
                foreach (var row in rows)
                {
                    if (rowNo != 1)
                    {
                        var username = row.Cell(1).Value.ToString();

                        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(username))
                        {
                            break;
                        }

                        var passBcrypt = BCrypt.Net.BCrypt.HashPassword(row.Cell(2).Value.ToString());

                        Employee employee;
                        employee = _empRepo.GetAll().Where(x => x.Username == username).FirstOrDefault();

                        if (employee == null)
                        {
                            employee = new Employee();
                        }

                        if (username == employee.Username)
                        {
                            return false;
                        }

                        employee.Username = username;
                        employee.Password = passBcrypt;
                        employee.Name = row.Cell(3).Value.ToString();
                        employee.Birthday = row.Cell(4).Value.ToString();
                        employee.Email = row.Cell(5).Value.ToString();
                        employee.PhoneNumber = row.Cell(6).Value.ToString();
                        employee.DepartmentId = int.Parse(row.Cell(7).Value.ToString());

                        UnitOfWork.BeginTransaction();
                        await _empRepo.Add(employee);
                        UnitOfWork.Commit();

                    }
                    else
                    {
                        rowNo++;
                    }
                }
                isSaveSuccess = true;
                return isSaveSuccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckIfExcelFile(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension == ".xlsx" || extension == ".xls");
        }
    }
}

﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Employee.Services.Implement;
using Employee.Services.Interface;

namespace Employee.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterService(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<IExcelService, ExcelService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ICheckExistsEmployeeService, CheckExistsEmployeeService>();
            
            return services;
        }
    }
}

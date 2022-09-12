using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Employee.Data.Context;
using Employee.Data.Repository;
using Employee.Data.UnitOfWork;

namespace Employee.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterData(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContextPool<TestDbContext>((serviceProvider, optionsBuilder) =>
            //{
            //    optionsBuilder.UseSqlServer(configuration.GetConnectionString("Test"));
            //    optionsBuilder.UseInternalServiceProvider(serviceProvider);
            //    optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            //});

            services.AddDbContext<EmployeeDbContext>(options =>
              options.UseSqlServer(
                  configuration.GetConnectionString("ConnectionSql")));


            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            return services;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Test.Data.Context;
using Test.Data.Repository;
using Test.Data.UnitOfWork;

namespace Test.Data
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

            services.AddDbContext<TestDbContext>(options =>
              options.UseSqlServer(
                  configuration.GetConnectionString("Test")));


            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            return services;
        }
    }
}

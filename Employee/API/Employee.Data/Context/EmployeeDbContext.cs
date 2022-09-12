using Microsoft.EntityFrameworkCore;

namespace Employee.Data.Context
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {
        }

        public DbSet<Modal.Entities.EmployeeEntity> Employees { get; set; }
        public DbSet<Modal.Entities.Department> Departments { get; set; }
        public DbSet<Modal.Entities.Notification> Notifications { get; set; }
    }
}

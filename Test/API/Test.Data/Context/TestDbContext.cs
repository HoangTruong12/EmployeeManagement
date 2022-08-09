using Microsoft.EntityFrameworkCore;

namespace Test.Data.Context
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public DbSet<Modal.Entities.Employee> Employees { get; set; }
    }
}

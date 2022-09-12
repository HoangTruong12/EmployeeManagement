using Employee.Data.Context;

namespace Employee.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        EmployeeDbContext Context { get; }

        void BeginTransaction();

        void Commit();

        void Rollback();
    }
}

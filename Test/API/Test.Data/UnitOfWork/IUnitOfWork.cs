using Test.Data.Context;

namespace Test.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        TestDbContext Context { get; }

        void BeginTransaction();

        void Commit();

        void Rollback();
    }
}

using Microsoft.EntityFrameworkCore.Storage;
using System;
using Test.Data.Context;

namespace Test.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction _transaction;

        public UnitOfWork(TestDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public TestDbContext Context { get; }

        public void BeginTransaction()
        {
            _transaction = Context.Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _transaction?.Commit();
            }
            catch
            {
                _transaction?.Rollback();

                throw;
            }
            finally
            {
                _transaction?.Dispose();
            }
        }

        public void Rollback()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                _transaction?.Dispose();
            }
        }

        public void Dispose()
        {
            Context?.Dispose();
            _transaction?.Dispose();
        }
    }
}

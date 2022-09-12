using Microsoft.EntityFrameworkCore.Storage;
using System;
using Employee.Data.Context;

namespace Employee.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction _transaction;

        public UnitOfWork(EmployeeDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public EmployeeDbContext Context { get; }

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

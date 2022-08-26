using Test.Data.UnitOfWork;

namespace Test.Services
{
    public abstract class BaseService
    {
        protected  readonly IUnitOfWork UnitOfWork;

        protected  BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}

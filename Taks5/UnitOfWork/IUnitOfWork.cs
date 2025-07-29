using Taks5.Repositories;

namespace Taks5.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        void Save();
    }
}

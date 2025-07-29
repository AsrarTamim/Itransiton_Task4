using Taks5.Repositories;

namespace Taks5.UnitOfWork
{
    public class UnitOfWork:IUnitOfWork
    {
        private  readonly AppDbContext _context;
        public IUserRepository User { get; }

        public UnitOfWork(AppDbContext context, IUserRepository user)
        {
            _context = context;
            User = user;
        }   

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

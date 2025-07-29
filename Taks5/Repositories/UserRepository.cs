using Taks5.Entities;

namespace Taks5.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public void add(UserAccount User)
        {
            _context.Users.Add(User);
        }

        public void Update(UserAccount User)
        {
            _context.Users.Update(User);
        }
        public UserAccount GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public List<UserAccount> GetAll()
        {
            return _context.Users.ToList();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public void Delete(UserAccount User)
        {
            _context.Users.Remove(User);
        }

        
    }
}

using Taks5.Entities;

namespace Taks5.Repositories
{
    public interface IUserRepository
    {
        void add(UserAccount User);
        void Update(UserAccount User);
        void Delete(UserAccount User);
        UserAccount GetByEmail(string email);
        public List<UserAccount> GetAll();
    }
}

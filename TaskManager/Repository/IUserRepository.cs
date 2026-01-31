using TaskManager.Entities;

namespace TaskManager.Repository
{
    public interface IUserRepository
    {
        public List<User> GetAll();

        public User Get(int id);

        public int? CreateUser(User u);

        public User? GetByLogin(string login);

        public bool SetRefreshToken(string refreshToken, int id);

        public bool Delete(int id);

    }
}

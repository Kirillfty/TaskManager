using Microsoft.EntityFrameworkCore;
using TaskManager.Entities;

namespace TaskManager.Repository
{
    public class UserRepository : IUserRepository
    {
        public ApplicationDB _context;

        public UserRepository(ApplicationDB context)
        {
            _context = context;
        }

        public int? CreateUser(User u)
        {
            _context.Users.Add(u);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }
            return u.Id;
        }

        public bool Delete(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user is not null)
            {
                _context.Remove(user);
                _context.SaveChanges();
                return true;
                
            }
            return false;
        }

        public User Get(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User? GetByLogin(string login)
        {
            return _context.Users.FirstOrDefault(u => u.Login == login);
        }

        public bool SetRefreshToken(string refreshToken, int id)
        {
            User? r = _context.Users.FirstOrDefault(u => u.Id == id);

            if (r == null)
                return false;

            r.RefreshToken = refreshToken;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}

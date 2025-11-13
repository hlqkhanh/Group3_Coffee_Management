using CoffeeManagement.DAL.DAO;
using CoffeeManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeManagement.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserDao userDao;

        public UserRepository(IUserDao userDao)
        {
            this.userDao = userDao ?? throw new ArgumentNullException(nameof(userDao));
        }
        public User? Authenticate(string email, string password)
        {
            return userDao.Authenticate(email, password);
        }

        public User Create(User user)
        {
            return userDao.Create(user);
        }

        public bool Delete(int id)
        {
            return userDao.Delete(id);
        }

        public IEnumerable<User> GetAll()
        {
            return userDao.GetAll();
        }

        public User? GetById(int id)
        {
            return userDao.GetById(id);
        }

        public User? GetByEmail(string email)
        {
            return userDao.GetByEmail(email);
        }

        public User? GetByUsername(string username)
        {
            return userDao.GetByUsername(username);
        }

        public void Update(User user)
        {
            userDao.Update(user);
        }
        public List<User> GetByRole(int roleId)
        {
            return userDao.GetByRole(roleId);
        }
    }
}
using CoffeeManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeManagement.DAL.DAO
{
    public interface IUserDao
    {
        User Create(User user);
        bool Delete(int id);
        void Update(User user);
        User? GetById(int id);
        User? GetByUsername(string username);
        User? GetByEmail(string email);
        List<User> GetByRole(int roleId);
        User? Authenticate(string email, string password);
        IEnumerable<User> GetAll();
    }
}
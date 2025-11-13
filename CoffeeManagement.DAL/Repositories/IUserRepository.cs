using CoffeeManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeManagement.DAL.Repositories
{
    public interface IUserRepository
    {
        User Create(User user);
        bool Delete(int id);
        void Update(User user);
        User? GetById(int id);
        User? GetByUsername(string username);
        User? GetByEmail(string email);
        User? Authenticate(string email, string password);
        List<User> GetByRole(int roleId);
        IEnumerable<User> GetAll();
    }
}
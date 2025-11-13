using CoffeeManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeManagement.BLL.Services
{
    public interface IUserService
    {
        User Create(User user);
        bool Delete(int id);
        void Update(User user);
        User? GetById(int id);
        User? GetByUsername(string username);
        User? Authenticate(string email, string password);
        IEnumerable<User> GetAll();
        List<User> GetByRole(int roleId);
    }
}
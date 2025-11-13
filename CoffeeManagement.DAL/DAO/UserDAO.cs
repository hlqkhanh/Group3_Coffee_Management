using CoffeeManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeManagement.DAL.DAO
{
    public class UserDao : IUserDao
    {
        private readonly CoffeeManagementDbContext _context;

        public UserDao(CoffeeManagementDbContext db)
        {
            _context = db ?? throw new ArgumentNullException(nameof(db));
        }

        public User Create(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public bool Delete(int id)
        {
            var e = _context.Users.Find(id);
            if (e == null) return false;
            _context.Users.Remove(e);
            _context.SaveChanges();
            return true;
        }

        public void Update(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            var existing = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existing == null) throw new InvalidOperationException("User not found");

            existing.Username = user.Username;
            existing.Email = user.Email;
            existing.Password = user.Password;
            existing.FirstName = user.FirstName;
            existing.LastName = user.LastName;
            existing.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
        }

        public User? GetById(int id) => _context.Users.Find(id);

        public User? GetByEmail(string email) =>
            string.IsNullOrWhiteSpace(email) ? null : _context.Users.SingleOrDefault(u => u.Email == email);

        public User? GetByUsername(string username) =>
            string.IsNullOrWhiteSpace(username) ? null : _context.Users.SingleOrDefault(u => u.Username == username);

        public User? Authenticate(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) return null;
            return _context.Users.SingleOrDefault(u => u.Email == email && u.Password == password);
        }

        public IEnumerable<User> GetAll() => _context.Users.Include("Role").ToList();

        public List<User> GetByRole(int roleId)
        {
            return _context.Users.Where(u => u.RoleId == roleId).ToList();
        }
    }
}
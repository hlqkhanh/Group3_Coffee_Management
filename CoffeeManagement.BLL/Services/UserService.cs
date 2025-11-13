using CoffeeManagement.DAL.Models;
using CoffeeManagement.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeManagement.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public User? Authenticate(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            email = email.Trim();

            // Plain-text password per your design
            return _userRepository.Authenticate(email, password);
        }

        public User Create(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(user.Username)) throw new ArgumentException("Username is required.");
            if (string.IsNullOrWhiteSpace(user.Password)) throw new ArgumentException("Password is required.");
            if (string.IsNullOrWhiteSpace(user.Email)) throw new ArgumentException("Email is required.");

            user.Username = user.Username.Trim();
            user.Email = user.Email.Trim();

            // Check username uniqueness
            var byUsername = _userRepository.GetByUsername(user.Username);
            if (byUsername != null)
                throw new InvalidOperationException("Username already exists.");

            // Check email uniqueness if repository supports it
            var byEmail = _userRepository.GetByEmail(user.Email);
            if (byEmail != null)
                throw new InvalidOperationException("Email already in use.");

            // Set audit fields
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = null;

            // Default role (optional) if not set
            // if not set by caller, assume Customer
            // if ((int)user.Role < 0) user.Role = Role.Customer;

            return _userRepository.Create(user);
        }

        public bool Delete(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid id.", nameof(id));
            return _userRepository.Delete(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll() ?? Enumerable.Empty<User>();
        }

        public User? GetById(int id)
        {
            if (id <= 0) return null;
            return _userRepository.GetById(id);
        }

        public User? GetByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return null;
            return _userRepository.GetByUsername(username.Trim());
        }

        public User? GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            return _userRepository.GetByEmail(email.Trim());
        }

        public void Update(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (user.Id <= 0) throw new ArgumentException("User id is required for update.");

            var existing = _userRepository.GetById(user.Id) ?? throw new InvalidOperationException("User not found.");

            // validate unique username if changed
            var newUsername = string.IsNullOrWhiteSpace(user.Username) ? existing.Username : user.Username.Trim();
            if (!string.Equals(newUsername, existing.Username, StringComparison.OrdinalIgnoreCase))
            {
                var u = _userRepository.GetByUsername(newUsername);
                if (u != null && u.Id != existing.Id)
                    throw new InvalidOperationException("Username already exists.");
            }

            // validate unique email if changed
            var newEmail = string.IsNullOrWhiteSpace(user.Email) ? existing.Email : user.Email.Trim();
            var emailOwner = _userRepository.GetByEmail(newEmail);
            if (emailOwner != null && emailOwner.Id != existing.Id)
                throw new InvalidOperationException("Email already in use.");

            // update scalar fields
            existing.Username = newUsername;
            existing.Email = newEmail;
            existing.Password = user.Password; // plain-text per your design
            existing.FirstName = user.FirstName;
            existing.LastName = user.LastName;
            existing.UpdatedAt = DateTime.UtcNow;

            _userRepository.Update(existing);
        }
        public List<User> GetByRole(int roleId)
        {
            if (roleId < 0) throw new ArgumentException("Invalid role id.", nameof(roleId));
            return _userRepository.GetByRole(roleId);
        }
    }
}
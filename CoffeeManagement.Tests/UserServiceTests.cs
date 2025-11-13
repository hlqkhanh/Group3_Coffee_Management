using CoffeeManagement.BLL.Services;
using CoffeeManagement.DAL.Models;
using CoffeeManagement.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoffeeManagement.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        // ============================================
        // 1️⃣ TEST: Authenticate()
        // ============================================
        [Test]
        public void Authenticate_ValidCredentials_ReturnsUser()
        {
            var email = "test@example.com";
            var password = "123456";
            var user = new User { Id = 1, Email = email, Password = password };

            _userRepositoryMock.Setup(r => r.Authenticate(email, password)).Returns(user);

            var result = _userService.Authenticate(email, password);

            Assert.IsNotNull(result);
            Assert.AreEqual(email, result.Email);
        }

        [Test]
        public void Authenticate_InvalidCredentials_ReturnsNull()
        {
            _userRepositoryMock.Setup(r => r.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                               .Returns((User)null);

            var result = _userService.Authenticate("wrong@example.com", "wrongpass");

            Assert.IsNull(result);
        }

        // ============================================
        // 2️⃣ TEST: Create()
        // ============================================
        [Test]
        public void Create_ValidUser_ReturnsCreatedUser()
        {
            var newUser = new User
            {
                Username = "john",
                Email = "john@example.com",
                Password = "123"
            };

            _userRepositoryMock.Setup(r => r.GetByUsername("john")).Returns((User)null);
            _userRepositoryMock.Setup(r => r.GetByEmail("john@example.com")).Returns((User)null);
            _userRepositoryMock.Setup(r => r.Create(It.IsAny<User>())).Returns((User u) => u);

            var result = _userService.Create(newUser);

            Assert.IsNotNull(result);
            Assert.AreEqual("john", result.Username);
            _userRepositoryMock.Verify(r => r.Create(It.IsAny<User>()), Times.Once);
        }

        [Test]
        public void Create_ExistingUsername_ThrowsException()
        {
            var newUser = new User { Username = "john", Email = "john@example.com", Password = "123" };

            _userRepositoryMock.Setup(r => r.GetByUsername("john")).Returns(new User());

            Assert.Throws<InvalidOperationException>(() => _userService.Create(newUser));
        }

        [Test]
        public void Create_NullUser_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _userService.Create(null));
        }

        // ============================================
        // 3️⃣ TEST: Delete()
        // ============================================
        [Test]
        public void Delete_ValidId_ReturnsTrue()
        {
            _userRepositoryMock.Setup(r => r.Delete(1)).Returns(true);

            var result = _userService.Delete(1);

            Assert.IsTrue(result);
            _userRepositoryMock.Verify(r => r.Delete(1), Times.Once);
        }

        [Test]
        public void Delete_InvalidId_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _userService.Delete(0));
        }

        // ============================================
        // 4️⃣ TEST: GetAll()
        // ============================================
        [Test]
        public void GetAll_ReturnsUsersList()
        {
            var list = new List<User> { new User { Id = 1, Username = "A" } };
            _userRepositoryMock.Setup(r => r.GetAll()).Returns(list);

            var result = _userService.GetAll();

            Assert.AreEqual(1, result.Count());
        }

        // ============================================
        // 5️⃣ TEST: GetById()
        // ============================================
        [Test]
        public void GetById_Valid_ReturnsUser()
        {
            var user = new User { Id = 2 };
            _userRepositoryMock.Setup(r => r.GetById(2)).Returns(user);

            var result = _userService.GetById(2);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
        }

        // ============================================
        // 6️⃣ TEST: Update()
        // ============================================
        [Test]
        public void Update_ValidUser_CallsRepositoryUpdate()
        {
            var existing = new User { Id = 1, Username = "john", Email = "john@example.com" };
            var update = new User { Id = 1, Username = "johnny", Email = "johnny@example.com", Password = "123" };

            _userRepositoryMock.Setup(r => r.GetById(1)).Returns(existing);
            _userRepositoryMock.Setup(r => r.GetByUsername("johnny")).Returns((User)null);
            _userRepositoryMock.Setup(r => r.GetByEmail("johnny@example.com")).Returns((User)null);

            _userService.Update(update);

            _userRepositoryMock.Verify(r => r.Update(It.Is<User>(u =>
                u.Username == "johnny" && u.Email == "johnny@example.com")), Times.Once);
        }

        [Test]
        public void Update_UserNotFound_ThrowsException()
        {
            var update = new User { Id = 99, Username = "notfound" };
            _userRepositoryMock.Setup(r => r.GetById(99)).Returns((User)null);

            Assert.Throws<InvalidOperationException>(() => _userService.Update(update));
        }

        // ============================================
        // 7️⃣ TEST: GetByRole()
        // ============================================
        [Test]
        public void GetByRole_ValidRole_ReturnsUsers()
        {
            var roleList = new List<User> { new User { Id = 1, Username = "staff" } };
            _userRepositoryMock.Setup(r => r.GetByRole(1)).Returns(roleList);

            var result = _userService.GetByRole(1);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("staff", result[0].Username);
        }

        [Test]
        public void GetByRole_InvalidRole_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _userService.GetByRole(-1));
        }
    }
}
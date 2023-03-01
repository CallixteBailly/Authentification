using Auth.Application.Interface;
using Auth.Domain.Entities;
using Auth.Infrastructure.Context;
using Auth.Infrastructure.Persistance.Interceptros;
using Auth.Infrastructure.Services;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Auth.Infrastructure.Persistance.Tests
{
    public class UserRepositoryTests
    {
        private readonly AuthDbContext _authDbContext;
        private readonly IUserRepository _userRepository;
        private readonly DateTimeService _dateTime = new();
        private readonly Fixture _fixture = new();

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AuthDbContext>()
                .UseInMemoryDatabase(databaseName: "AuthDatabase")
                .Options;

            var interceptor = new AuditableEntitySaveChangesInterceptor(_dateTime);
            _authDbContext = new AuthDbContext(options, interceptor);
            _userRepository = new UserRepository(_authDbContext);
        }

        [Fact]
        public void AddUser_ShouldAddUserToDatabase()
        {
            // Arrange
            var user = _fixture.Create<User>();


            // Act
            _userRepository.Add(user);
            _authDbContext.SaveChanges();

            // Assert
            var savedUser = _authDbContext?.Users?.SingleOrDefault(u => u.Id == user.Id);
            Assert.NotNull(savedUser);
            Assert.Equal(user, savedUser, new UserEqualityComparer());
        }

        [Fact]
        public void GetUserByEmail_ShouldReturnUserWithMatchingEmail()
        {
            // Arrange
            var user = _fixture.Create<User>();
            _authDbContext?.Users?.Add(user);
            _authDbContext?.SaveChanges();

            // Act
            var result = _userRepository.GetUserByEmail(user.Email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user, result, new UserEqualityComparer());
        }

        [Fact]
        public void CreatePaswwordHash_ShouldGenerateValidPasswordHashAndSalt()
        {
            // Arrange
            var password = "testpassword";

            // Act
            _userRepository.CreatePaswwordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            // Assert
            Assert.NotNull(passwordHash);
            Assert.NotNull(passwordSalt);
            Assert.NotEmpty(passwordHash);
            Assert.NotEmpty(passwordSalt);
        }

        [Fact]
        public void VerifyPasswordHash_ShouldVerifyValidPasswordHash()
        {
            // Arrange
            var password = "testpassword";
            _userRepository.CreatePaswwordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            // Act
            var result = _userRepository.VerifyPasswordHash(password, passwordHash, passwordSalt);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AddUser_ShouldCallSaveChanges()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var mockDbContext = new Mock<AuthDbContext>();
            var userRepository = new UserRepository(mockDbContext.Object);

            // Act
            userRepository.Add(user);

            // Assert
            mockDbContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        public class UserEqualityComparer : IEqualityComparer<User>
        {
            public bool Equals(User? x, User? y)
            {
                return x == null || y == null
                    ? false
                    : x.Id == y.Id &&
                       x.FirstName == y.FirstName &&
                       x.LastName == y.LastName &&
                       x.Email == y.Email &&
                       x.PasswordHash.SequenceEqual(y.PasswordHash) &&
                       x.PasswordSalt.SequenceEqual(y.PasswordSalt);
            }

            public int GetHashCode(User obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }
}

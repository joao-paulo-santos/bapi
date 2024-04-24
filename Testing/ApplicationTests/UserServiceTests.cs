using Moq;
using Core.Entities;
using Core.Interfaces; 
using Application.Services;

namespace Testing.ApplicationTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _userService = new UserService(_mockUnitOfWork.Object);
        }

        #region helpers
        private User GetDefaultUser()
        {
            return new User
            {
                Password = "password",
                Role = Role.Admin,
                Username = "username"
            };
        }
        #endregion

        #region User Registration Tests

        [Fact]
        public async Task RegisterUserAsync_ValidUser_ReturnsCreatedUser()
        {
            // Arrange
            var username = "testuser";
            var password = "password";
            var newUser = new User
            {
                Username = username,
                Password = "hashedPassword",
                CreatedDate = DateTime.UtcNow,
                Role = Role.User
            };
            _mockUnitOfWork.Setup(uow => uow.UserRepository.AddUserAsync(It.IsAny<User>()))
                .ReturnsAsync(newUser);

            // Act
            var createdUser = await _userService.RegisterUserAsync(username, password);

            // Assert
            Assert.NotNull(createdUser);
            Assert.Equal(username, createdUser.Username);
            Assert.Equal(Role.User, createdUser.Role);
            _mockUnitOfWork.Verify(uow => uow.UserRepository.AddUserAsync(It.IsAny<User>()), Times.Once);
        }

        //field validations are already ensured in Data Annotations

        #endregion

        #region Password Verification Tests

        [Fact]
        public void VerifyPassword_CorrectPassword_ReturnsTrue()
        {
            // Arrange
            var user = new User { Username ="testName" ,Password = "5f4dcc3b5aa765d61d8327deb882cf99", Role = Role.User};
            var password = "password"; // Matches the hashed password

            // Act
            var isValid = _userService.VerifyPassword(user, password);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void VerifyPassword_IncorrectPassword_ReturnsFalse()
        {
            // Arrange
            var user = new User { Username = "testName", Password = "5f4dcc3b5aa765d61d8327deb882cf98", Role = Role.User };
            var password = "password"; // Doesn't match the hashed password

            // Act
            var isValid = _userService.VerifyPassword(user, password);

            // Assert
            Assert.False(isValid);
        }

        #endregion

        #region Get by Username Tests
        [Fact]
        public async Task GetUserByUsernameAsync_ExistingUser_ReturnsUser()
        {
            // Arrange
            var username = "testName";
            var mockUser = new User { Username = "testName", Password = "5f4dcc3b5aa765d61d8327deb882cf99", Role = Role.User };
            _mockUnitOfWork.Setup(uow => uow.UserRepository.GetUserByUsernameAsync(username))
                .ReturnsAsync(mockUser);

            // Act
            var user = await _userService.GetUserByUsernameAsync(username);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(username, user.Username);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_NonexistentUser_ReturnsNull()
        {
            // Arrange
            var username = "nonexistentuser";
            _mockUnitOfWork.Setup(uow => uow.UserRepository.GetUserByUsernameAsync(username))
                .ReturnsAsync((User?)null);

            // Act
            User? user = await _userService.GetUserByUsernameAsync(username);

            // Assert
            Assert.Null(user);
        }
        #endregion

        #region Get Paged Users Tests

        [Fact]
        public async Task GetPagedListOfUsersAsync_ValidInput_ReturnsPagedUsers()
        {
            // Arrange
            var pageIndex = 1;
            var pageSize = 10;
            var mockUsers = new List<User>() { GetDefaultUser(), GetDefaultUser() };
            _mockUnitOfWork.Setup(uow => uow.UserRepository.GetPagedListOfUsersAsync(pageIndex, pageSize))
                .ReturnsAsync(mockUsers);

            // Act
            var users = await _userService.GetPagedListOfUsersAsync(pageIndex, pageSize);

            // Assert
            Assert.NotNull(users);
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public async Task GetPagedListOfUsersAsync_PageSizeGreaterThanMax_ReturnsMaxPageSize()
        {
            // Arrange
            var pageIndex = 0;
            var pageSize = 20; // Greater than maxPageSize (15)
            var expectedSize = 15;
            var mockUsers = new List<User>();
            for(int i = 0; i < expectedSize; i++) {
                mockUsers.Add(GetDefaultUser());
            }
            _mockUnitOfWork.Setup(uow => uow.UserRepository.GetPagedListOfUsersAsync(pageIndex, expectedSize))
                .ReturnsAsync(mockUsers);

            // Act
            var users = await _userService.GetPagedListOfUsersAsync(pageIndex, pageSize);

            // Assert
            Assert.NotNull(users);
            Assert.Equal(expectedSize, users.Count);
        }

        [Fact]
        public async Task GetPagedListOfUsersAsync_ZeroPageSize_ReturnsDefaultPageSize()
        {
            // Arrange
            var pageIndex = 0;
            var pageSize = 0;
            var expectedSize = 5; // Default page size assumed to be 5
            var mockUsers = new List<User>();
            for (int i = 0; i < expectedSize; i++)
            {
                mockUsers.Add(GetDefaultUser());
            }
            _mockUnitOfWork.Setup(uow => uow.UserRepository.GetPagedListOfUsersAsync(pageIndex, expectedSize))
                .ReturnsAsync(mockUsers);

            // Act
            var users = await _userService.GetPagedListOfUsersAsync(pageIndex, pageSize);

            // Assert
            Assert.NotNull(users);
            Assert.Equal(expectedSize, users.Count); 
        }

        #endregion

        #region Get List Tests
        [Fact]
        public async Task GetListOfUsersAsync_ReturnsUsersFromRepository()
        {
            // Arrange
            var mockUsers = new List<User>() { GetDefaultUser(), GetDefaultUser() };
            _mockUnitOfWork.Setup(uow => uow.UserRepository.GetListOfUsersAsync())
                .ReturnsAsync(mockUsers);

            // Act
            var users = await _userService.GetListOfUsersAsync();

            // Assert
            Assert.NotNull(users);
            Assert.Equal(2, users.Count);
        }
        #endregion

        #region Delete User Tests
        [Fact]
        public async Task DeleteUserAsync_ValidUser_DeletesUserAndSavesChanges()
        {
            // Arrange
            var user = new User { Id = 1, Username = "testuser", Password = "pw", Role= Role.Manager };
            _mockUnitOfWork.Setup(uow => uow.UserRepository.DeleteUserAsync(user))
                .Returns(Task.FromResult(true)); // Assuming DeleteUserAsync returns true

            // Act
            var isDeleted = await _userService.DeleteUserAsync(user);

            // Assert
            Assert.True(isDeleted);
        }

        [Fact]
        public async Task DeleteUserAsync_InvalidUser_DeletesUserAndSavesChanges()
        {
            // Arrange
            var user = new User { Id = 1, Username = "testuser", Password = "pw", Role = Role.Manager };
            _mockUnitOfWork.Setup(uow => uow.UserRepository.DeleteUserAsync(user))
                .Returns(Task.FromResult(false)); // Assuming DeleteUserAsync returns true

            // Act
            var isDeleted = await _userService.DeleteUserAsync(user);

            // Assert
            Assert.False(isDeleted);
        }


        #endregion

        #region Change User Role Tests
        [Fact]
        public async Task ChangeUserRoleAsync_ValidUserAndRole_UpdatesUserAndSavesChanges()
        {
            // Arrange
            var user = new User { Id = 1, Username = "testuser", Password = "pw", Role = Role.User };
            var newRole = Role.Admin;
            var expectedUpdatedUser = new User { Id = 1, Username = "testuser", Password = "pw", Role = Role.Admin };
            _mockUnitOfWork.Setup(uow => uow.UserRepository.UpdateUserAsync(It.IsAny<User>()))
                .Returns(Task.FromResult<User?>(expectedUpdatedUser)); // Assuming UpdateUserAsync saves changes

            // Act
            var updatedUser = await _userService.ChangeUserRoleAsync(user, newRole);

            // Assert
            Assert.NotNull(updatedUser);
            Assert.Equal(newRole, updatedUser.Role);
        }
        #endregion
    }
}

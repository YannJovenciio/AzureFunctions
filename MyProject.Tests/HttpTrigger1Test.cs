using Company.Function;
using AzureFunctions.UserService;
using AzureFunctions.UserInterface;
using Microsoft.Extensions.Logging;
using System.Net;
using AzureFunctions.Users;
using AzureFunctions.UserDTOS;
using Moq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AzureFunctions.DbContexts;
using Microsoft.EntityFrameworkCore;
using AzureFunctions;

namespace NewProject.Tests
{
    public class HttpTrigger1Test
    {


        [Fact]
        public async Task ShouldReturnAllUsersAsync()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb2")
            .Options;

            var context = new UserDbContext(options);

            UserService userService = new UserService(context);

            var users = new List<User>
            {
                new User { id = 1, username = "joao", email = "joao@example.com", password = "123456" },
                new User { id = 2, username = "jeferson", email = "jeferson@example.com", password = "123456" }
            };

            // Act
            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            var result = await userService.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, u => u.username == "joao" && u.email == "joao@example.com");
            Assert.Contains(result, u => u.username == "jeferson" && u.email == "jeferson@example.com");


        }

        [Fact]
        public async Task GetUserById_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

            var context = new UserDbContext(options);

            UserService userService = new UserService(context);

            var existingUser = new User
            {
                id = 6,
                username = "jeferson",
                email = "jeferson@example.com",
                password = "123456"
            };

            //Act
            await context.Users.AddAsync(existingUser);
            await context.SaveChangesAsync();

            //Assert
            var result = await userService.GetUserByIdAsync(6);
            Assert.NotNull(result);
            Assert.Equal("jeferson", result.username);
            Assert.Equal("jeferson@example.com", result.email);

        }

        [Fact]
        public async Task GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = new UserDbContext(options);
            UserService userService = new UserService(context);

            var ExistingUser = new User
            {
                id = 4,
                username = "joao",
                email = "joao@example.com",
                password = "123456"
            };

            //Act
            context.Users.Add(ExistingUser);
            await context.SaveChangesAsync();
            var result = await userService.GetUserByIdAsync(4);

            //Assert
            await Assert.ThrowsAsync<UserNotFoundException>(async () =>
            {
                await userService.GetUserByIdAsync(5);
            });
        }

        [Fact]
        public async Task AddUser_ShouldReturnOkResult_WithAddedUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb1")
                .Options;

            var context = new UserDbContext(options);
            UserService userService = new UserService(context);

            var user = new User
            {
                id = 1,
                username = "Joao",
                email = "Joao@example.com",
                password = "123456"
            };

            //Act
            await userService.AddUserAsync(user);
            var result = await userService.GetUserByIdAsync(1);

            //Assert

            Assert.NotNull(result);
            Assert.Equal("Joao", result.username);
            Assert.Equal("Joao@example.com", result.email);

        }
        [Fact]
        public async Task UpdateUserTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = new UserDbContext(options);
            UserService userService = new UserService(context);

            UserDTO user = new UserDTO();
            user.username = "Beltrano";
            user.email = "Beltrano@example.com";

            // Act
            User existingUser = new() { id = 1, username = "joao", email = "joao@example.com", password = "123456" };
            context.Users.Add(existingUser);
            await context.SaveChangesAsync();

            UserDTO userDTO = new() { id = 1, username = "Beltrano", email = "Beltrano@example.com" };
            await userService.UpdateUserAsync(userDTO, 1);
            User updatedUser = await context.Users.FindAsync(1);
            // Assert

            Assert.NotNull(updatedUser);
            Assert.Equal("Beltrano", updatedUser.username);
            Assert.Equal("Beltrano@example.com", updatedUser.email);
        }
    }
}

using EmployeeManagement.Controllers;
using EmployeeManagement.Data;
using EmployeeManagement.Entities;
using EmployeeManagement.Helpers;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeManagement.Tests
{
    public class UsersControllerUnitTests
    {

        #region Test Methods
        [Fact]
        public async Task AuthenticateMethodAuthenticatesValidUsers()
        {
            // Arrange
            var mockRepo = new Mock<IRepository<User>>();
            mockRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(GetTestUsers());
            var mockSettings = new Mock<AppSettings>();
            mockSettings.Object.Secret = "My long random Secret";
            var controller = new UsersController(mockRepo.Object, mockSettings.Object);

            // Act
            var result = await controller.Authenticate(new AuthenticateRequest() { Username = "test", Password = "test" });

            // Assert
            var okObjResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<AuthenticateResponse>(okObjResult.Value);
            Assert.NotEmpty(response.Token);
            Assert.NotEqual(0, response.Id);
            Assert.NotEmpty(response.LastName);
            Assert.NotEmpty(response.FirstName);
        }
        #endregion


        #region Mock helper methods
        private List<User> GetTestUsers()
        {
            var users = new List<User>();
            users.Add(new User()
            {
                Id = 1,
                FirstName = "First", 
                LastName = "Last", 
                Password = "test", 
                Username = "test"
            });
            users.Add(new User()
            {
                Id = 2,
                FirstName = "First2",
                LastName = "Last2",
                Password = "Pass2",
                Username = "Username2"
            });
            return users;
        }
        #endregion 
    }
}

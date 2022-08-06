using EmployeeManagement.Controllers;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeManagement.Tests
{
    public class EmployeeControllerUnitTests
    {

        #region Test Methods
        [Fact]
        public async Task GetEmployeeMethodReturnsAllEmployees()
        {
            // Arrange
            var mockRepo = new Mock<IRepository<Employee>>();
            mockRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(GetTestEmployees());
            var controller = new EmployeesController( mockRepo.Object);

            // Act
            var result = await controller.GetEmployee();

            // Assert
            var viewResult = Assert.IsType<ActionResult<IEnumerable<Employee>>>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Employee>>(
                viewResult.Value);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task GetEmployeeMethodReturnsSpecificEmployee()
        {
            // Arrange
            var mockRepo = new Mock<IRepository<Employee>>();
            mockRepo.Setup(repo => repo.Get(2))
                .ReturnsAsync(GetSpecificEmployee(2));
            var controller = new EmployeesController(mockRepo.Object);

            // Act
            var result = await controller.GetEmployee(2);

            // Assert
            var viewResult = Assert.IsType<ActionResult<Employee>>(result);
            var model = Assert.IsAssignableFrom<Employee>(
                viewResult.Value);
            Assert.Equal(2, model.Id);
        }

        [Fact]
        public async Task PostEmployeeMethodAddsEmployee()
        {
            // Arrange
            var mockRepo = new Mock<IRepository<Employee>>();
            var newEmployee = new Employee()
            {
                Address = "Add3",
                Cityname = "City3",
                Company = "Comp3",
                Designation = "Des3",
                Gender = "G3",
                Id = 3,
                Name = "Name3"
            };
            mockRepo.Setup(repo => repo.Insert(newEmployee));
            mockRepo.Setup(repo => repo.Exists(newEmployee.Id)).ReturnsAsync(Exists(newEmployee.Id));
         
            var controller = new EmployeesController(mockRepo.Object);

            // Act
            var result = await controller.PostEmployee(newEmployee);

            // Assert
            var viewResult = Assert.IsType<ActionResult<Employee>>(result);
            var createdAtActionResult = Assert.IsAssignableFrom<CreatedAtActionResult>(
                viewResult.Result);
            var model = Assert.IsAssignableFrom<Employee>(
             createdAtActionResult.Value);
            Assert.Equal(3, model.Id);
        }

        [Fact]
        public async Task PutEmployeeMethodUpdatesEmployee()
        {
            // Arrange
            var mockRepo = new Mock<IRepository<Employee>>();
            var employeeToUpdate = new Employee()
            {
                Address = "Add3",
                Cityname = "City3",
                Company = "Comp3",
                Designation = "Des3",
                Gender = "G3",
                Id = 3,
                Name = "Name3"
            };
            mockRepo.Setup(repo => repo.Insert(employeeToUpdate));
            employeeToUpdate.Address = "Edited";
            mockRepo.Setup(repo => repo.Update(employeeToUpdate));
            mockRepo.Setup(repo => repo.Get(employeeToUpdate.Id)).ReturnsAsync(GetSpecificEmployee(employeeToUpdate.Id));

            var controller = new EmployeesController(mockRepo.Object);

            // Act
            var result = await controller.PutEmployee(employeeToUpdate.Id, employeeToUpdate);

            // Assert
            Assert.IsAssignableFrom<NoContentResult>(
                result);
        }

        [Fact]
        public async Task DeleteEmployeeMethodRemovesEmployee()
        {
            // Arrange
            var mockRepo = new Mock<IRepository<Employee>>();
            var employeeToDelete = new Employee()
            {
                Address = "Add3",
                Cityname = "City3",
                Company = "Comp3",
                Designation = "Des3",
                Gender = "G3",
                Id = 3,
                Name = "Name3"
            };
            mockRepo.Setup(repo => repo.Insert(employeeToDelete));
            mockRepo.Setup(repo => repo.Delete(employeeToDelete));
            mockRepo.Setup(repo => repo.Get(employeeToDelete.Id)).ReturnsAsync(GetSpecificEmployee(employeeToDelete.Id));

            var controller = new EmployeesController(mockRepo.Object);

            // Act
            var result = await controller.DeleteEmployee(employeeToDelete.Id);

            // Assert
            Assert.IsAssignableFrom<NoContentResult>(
                result);
        }

        #endregion 


        #region Mock helper methods
        private Employee GetSpecificEmployee(int id)
        {
            return new Employee()
            {
                Id = id,
                Name = "Test One",
                Address = "Addddd",
                Cityname = "Cityyy",
                Company = "Company",
                Designation = "Des",
                Gender = "G"
            };
        }

        private List<Employee> GetTestEmployees()
        {
            var sessions = new List<Employee>();
            sessions.Add(new Employee()
            {
                Id = 1,
                Name = "Test One",
                Address = "Addddd",
                Cityname = "Cityyy",
                Company = "Company",
                Designation = "Des",
                Gender = "G"
            });
            sessions.Add(new Employee()
            {
                Id = 2,
                Name = "Test Two",
                Address = "Addddd",
                Cityname = "Cityyy",
                Company = "Company",
                Designation = "Des",
                Gender = "G"
            });
            return sessions;
        }

        private bool Exists(int id)
        {
            return true; 
        }

        #endregion 
    }
}

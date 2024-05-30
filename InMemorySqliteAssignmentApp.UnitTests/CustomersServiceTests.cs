using FluentAssertions;
using FluentValidation;
using InMemorySqliteAssignmentApp.Controllers;
using InMemorySqliteAssignmentApp.Domain.Entities;
using InMemorySqliteAssignmentApp.Services_BusinessLayer_.Abstract;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;


namespace InMemorySqliteAssignmentApp.UnitTests
{
    public class CustomersServiceTests
    {
        private readonly Mock<ICustomerService> _service;
        private readonly Mock<ILogger<Customer>> _logger;
        private readonly Mock<IValidator<Customer>> _validator;
        private readonly CustomersController _controller;

        public CustomersServiceTests()
        {
            _logger = new Mock<ILogger<Customer>>();
            _validator = new Mock<IValidator<Customer>>();
            _service = new Mock<ICustomerService>();
            _controller = new CustomersController(_validator.Object, _service.Object, _logger.Object);
        }

        [Fact]
        public async void SaveCustomer_Returns_OKResutl()
        {
            // Arrange
            var customers = GetCustomers();

            var validator = new InlineValidator<Customer>();
            validator.RuleFor(x => x.DateOfBirth).NotEmpty();
            validator.RuleFor(x => x.FullName).NotEmpty();

            var controller = new CustomersController(validator, _service.Object, _logger.Object);

            Customer customer = new Customer()
            {
                Id = new Guid(),
                FullName = "Vijay Kumar",
                DateOfBirth = new DateOnly(1978, 06, 22),
                CustomerAvtar = string.Empty
            };

            _service.Setup(service => service.SaveCustomer(It.IsAny<Customer>())).Returns(customer);

            //Act
            var response = (OkObjectResult) await controller.SaveCustomer(customer);

            //Assert
            response.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetCustomerById_Returns_Customer()
        {
            //Arrange
            Guid id = new Guid("62cf479c-9537-4029-9944-db71fd6be65e");
            var customerList = GetCustomers();

            var customer = customerList.Where(x => x.Id == id).FirstOrDefault();

            _service.Setup(x => x.GetCustomerById(It.IsAny<Guid>())).Returns(customer);

            //Act
            var response = _controller.GetCustomerById(id);

            //Assert
            response.Should().BeOfType<OkObjectResult>();
            Assert.Equal(customerList.Where(x => x.Id == id).FirstOrDefault().Id, customer.As<Customer>().Id);
        }

        [Fact]
        public void GetCustomerByAge_Return_CustomerList()
        {
            // Arrange
            var customerList = GetCustomers();
            int age = 47;

            var customers = customerList.Where(x => DateTime.Now.Year - x.DateOfBirth.Year == age).ToList();

            _service.Setup(x => x.GetCustomerByAge(It.IsAny<int>())).Returns(customers);

            // Act
            var response = _controller.GetCustomerByAge(age);


            //Assert
            Assert.NotNull(response);
            Assert.Equal(customerList.Where(x => DateTime.Now.Year - x.DateOfBirth.Year == age), customers);
        }

        [Fact]
        public void PatchCustomerDetails_Return_OkResult() 
        {
            // Arrange
            var customersList = GetCustomers();
            Guid id = new Guid("6bdc1ad2-5175-4e7f-89dd-9d07c976ba3d");

            JsonPatchDocument<Customer> patchDoc = new JsonPatchDocument<Customer>();
            patchDoc.Replace(x => x.FullName, "TestName");


            var customer = customersList.Where(x => x.Id == id).FirstOrDefault();

            _service.Setup(service => service.PatchCustomer(It.IsAny<Customer>())).Returns(customer);

            // Act
            var response = _controller.PatchCustomerDetails(id, patchDoc);

            // Assert
            Assert.IsType<OkObjectResult>(response);
            Assert.True((customersList.Where(x => x.Id == id).FirstOrDefault()).FullName.Equals("TestName"));
        }


        public IEnumerable<Customer> GetCustomers() 
        {
            return new List<Customer> 
            { 
                new Customer() 
                {  
                    Id = new Guid("6ca62410-ad0e-4449-b0fd-b8b4ab86b526"), 
                    FullName="John Mercy", 
                    DateOfBirth= new DateOnly(1978, 06, 22),
                    CustomerAvtar = string.Empty
                },
                new Customer()
                {
                    Id = new Guid("62cf479c-9537-4029-9944-db71fd6be65e"),
                    FullName="Vijay Kumar",
                    DateOfBirth= new DateOnly(1980, 03, 12),
                    CustomerAvtar = string.Empty
                },
                new Customer()
                {
                    Id = new Guid("6bdc1ad2-5175-4e7f-89dd-9d07c976ba3d"),
                    FullName="Vinod Sharma",
                    DateOfBirth= new DateOnly(1982, 11, 14),
                    CustomerAvtar = string.Empty
                },
                new Customer()
                {
                    Id = new Guid("14eeb310-8b0f-44de-9313-41b88d43df84"),
                    FullName="Rajkumar Verma",
                    DateOfBirth= new DateOnly(1986, 06, 01),
                    CustomerAvtar = string.Empty
                },
                new Customer()
                {
                    Id = new Guid("56808d86-55d1-489c-9596-98f03c0903f2"),
                    FullName="Suresh Kumar",
                    DateOfBirth= new DateOnly(1990, 10, 26),
                    CustomerAvtar = string.Empty
                },
            };
        }

        public class JsonPatchDoc
        {
            public int OperationType { get; set; } = 0;
            public string Path { get; set; } = "string";
            public string Op { get; set; } = "string";
            public string From { get; set; } = "string";
            public string Value { get; set; } = "string";
        }
    }
}
using InMemorySqliteAssignmentApp.Domain.Entities;
using InMemorySqliteAssignmentApp.Infrastructure_Data_;
using InMemorySqliteAssignmentApp.Services_BusinessLayer_.Abstract;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace InMemorySqliteAssignmentApp.Services_BusinessLayer_.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _appDbContext;

        private readonly ILogger<CustomerService> _logger;

        public CustomerService(AppDbContext appDbContext, ILogger<CustomerService> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public Customer SaveCustomer(Customer model)
        {
            Customer customer = new Customer();
            
            customer.Id = Guid.NewGuid();
            customer.FullName = model.FullName; 
            customer.DateOfBirth = model.DateOfBirth;
            customer.CustomerAvtar = model.CustomerAvtar;

            _appDbContext.Add(customer);
            _appDbContext.SaveChanges();

            return customer;
        }

        public Customer GetCustomerById(Guid id) => _appDbContext.Customers.Where(x => x.Id.Equals(id)).FirstOrDefault();

        public IEnumerable<Customer> GetCustomerByAge(int age) =>_appDbContext.Customers.Where(x => (DateTime.Now.Year - x.DateOfBirth.Year) == age).ToList();

        public Customer PatchCustomer(Customer model)
        {
            _appDbContext.Update(model);
            _appDbContext.SaveChanges();

            return model;
        }
     }
}

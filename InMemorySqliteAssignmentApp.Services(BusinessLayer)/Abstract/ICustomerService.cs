using InMemorySqliteAssignmentApp.Domain.Entities;

namespace InMemorySqliteAssignmentApp.Services_BusinessLayer_.Abstract
{
    public interface ICustomerService
    {
        public Customer SaveCustomer(Customer model);
        public Customer GetCustomerById(Guid id);
        public IEnumerable<Customer> GetCustomerByAge(int age);
        public Customer PatchCustomer(Customer model);
    }
}

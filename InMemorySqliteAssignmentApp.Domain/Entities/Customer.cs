using InMemorySqliteAssignmentApp.Domain.Abstract;
using Swashbuckle.AspNetCore.Annotations;

namespace InMemorySqliteAssignmentApp.Domain.Entities
{
    public class Customer : BaseEntity<Guid>
    {
        
        public Customer() 
        {
            Id = new Guid();
        }

        public string FullName { get; set; } = String.Empty;
        public DateOnly DateOfBirth { get; set; }

        // Adding new field for Customer Avtar
        [SwaggerSchema(ReadOnly = true)]
        public string CustomerAvtar { get; set; }= String.Empty;
    }
}

using FluentValidation;
using InMemorySqliteAssignmentApp.Domain.Entities;

namespace InMemorySqliteAssignmentApp.Domain.Helpers
{
    public class CustomerValidator: AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            //RuleFor(x=>x.Id).NotEmpty()
            //    .WithErrorCode("name_required")
            //    .WithMessage("Id cannot be empty");

            RuleFor(x=>x.FullName).NotEmpty()
                .WithErrorCode("name_required")
                .WithMessage("Name cannot be empty");

            RuleFor(x => x.DateOfBirth).NotEmpty()
                .WithErrorCode("date_required")
                .WithMessage("Date cannot be empty");
        }
    }
}

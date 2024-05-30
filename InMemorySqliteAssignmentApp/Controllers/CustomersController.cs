using FluentValidation;
using InMemorySqliteAssignmentApp.Domain.Entities;
using InMemorySqliteAssignmentApp.Services_BusinessLayer_.Abstract;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace InMemorySqliteAssignmentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IValidator<Customer> _validator;
        private readonly ICustomerService _customerService;
        private readonly ILogger<Customer> _logger;

        public CustomersController(IValidator<Customer> validator,ICustomerService customerService, ILogger<Customer> logger)
        {
            _validator = validator;
            _customerService = customerService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SaveCustomer(Customer model) 
        {
            if (model == null)
                return BadRequest("Customer Detail not Found !");

            var validation = _validator.Validate(model);
            if(!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage));
                return BadRequest(errors);
            }

            try
            {
                // Getting user Avtar
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("https://ui-avatars.com/");

                    using (HttpResponseMessage response = await httpClient.GetAsync($"api/?name={String.Join("+",model.FullName.Split(" "))}&format=svg"))
                    {
                        var apiResponse = response.Content.ReadAsStringAsync().Result;
                        response.EnsureSuccessStatusCode();
                        _logger.LogInformation(apiResponse);
                        model.CustomerAvtar = apiResponse;
                    }
                }
                
                var result = _customerService.SaveCustomer(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Guid")]
        public IActionResult GetCustomerById(Guid id) 
        {
            if (id == null)
                return BadRequest("Id is null !");
            
            var customer = _customerService.GetCustomerById(id);

            if (customer == null)
                return NotFound("Customer Not Found !");

            return Ok(customer);
        }

        [HttpPost]
        [Route("int")]
        public IActionResult GetCustomerByAge([FromQuery] int age) 
        {
            if (age == 0)
                return BadRequest("Data not provided !");

            var customerslist = _customerService.GetCustomerByAge(age);

            if (customerslist == null)
                return NotFound("Customers Not Found !");

            return Ok(customerslist);
        }

        [HttpPatch]
        [Route("Guid")]
        public IActionResult PatchCustomerDetails([FromQuery] Guid id,[FromBody] JsonPatchDocument<Customer> patchDoc) 
        {
            if (patchDoc == null)
                return BadRequest();

            try
            {
                var customer = _customerService.GetCustomerById(id);
                var original = customer;

                if (customer == null)
                    return NotFound();

                patchDoc.ApplyTo(customer, ModelState);

                if (!TryValidateModel(customer))
                    return BadRequest(ModelState);

                _customerService.PatchCustomer(customer);

                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);    
                throw;
            }
            
        }
    }
}

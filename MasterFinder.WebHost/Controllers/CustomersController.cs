using Microsoft.AspNetCore.Mvc;
using MasterFinder.Domain.Entities;
using MasterFinder.Domain.Interfaces;
using MasterFinder.ValueObjects;
using System.Threading;

namespace MasterFinder.WebHost.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerRepository.GetAllAsync(CancellationToken.None);
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id, CancellationToken.None);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
        {
            var customer = new Customer(
                new Username(request.Username),
                new PhoneNumber(request.Phone)
            );
            await _customerRepository.AddAsync(customer, CancellationToken.None);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        [HttpGet("{id}/orders")]
        public async Task<IActionResult> GetOrders(Guid id)
        {
            var orders = await _customerRepository.GetCustomerOrdersAsync(id, CancellationToken.None);
            return Ok(orders);
        }

        [HttpGet("by-phone/{phone}")]
        public async Task<IActionResult> GetByPhone(string phone)
        {
            var customer = await _customerRepository.GetByPhoneAsync(phone, CancellationToken.None);
            if (customer == null) return NotFound();
            return Ok(customer);
        }
    }

    public class CreateCustomerRequest
    {
        public string Username { get; set; }
        public string Phone { get; set; }
    }
}






using Microsoft.AspNetCore.Mvc;
using MasterFinder.Domain.Entities;
using MasterFinder.Domain.Interfaces;
using MasterFinder.ValueObjects;
using System.Threading;

namespace MasterFinder.WebHost.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IExecutorRepository _executorRepository;

        public OrdersController(
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            IExecutorRepository executorRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _executorRepository = executorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderRepository.GetAllAsync(CancellationToken.None);
            return Ok(orders);
        }

        [HttpGet("open")]
        public async Task<IActionResult> GetOpenOrders()
        {
            var orders = await _orderRepository.GetOpenOrdersAsync(CancellationToken.None);
            return Ok(orders);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(Guid customerId)
        {
            var orders = await _orderRepository.GetByCustomerIdAsync(customerId, CancellationToken.None);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id, CancellationToken.None);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId, CancellationToken.None);
            if (customer == null) return BadRequest("Заказчик не найден");

            var order = customer.CreateOrder(
                new OrderTitle(request.Title),
                new OrderDescription(request.Description)
            );
            await _orderRepository.AddAsync(order, CancellationToken.None);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpPost("{id}/respond")]
        public async Task<IActionResult> Respond(Guid id, [FromBody] RespondRequest request)
        {
            var order = await _orderRepository.GetByIdAsync(id, CancellationToken.None);
            if (order == null) return NotFound("Заказ не найден");

            var executor = await _executorRepository.GetByIdAsync(request.ExecutorId, CancellationToken.None);
            if (executor == null) return BadRequest("Исполнитель не найден");

            var comment = request.Comment != null ? ResponseComment.Create(request.Comment) : null;
            var response = executor.RespondToOrder(order, comment);

            await _orderRepository.UpdateAsync(order, CancellationToken.None);
            return Ok(response);
        }

        [HttpPost("{id}/withdraw-response/{responseId}")]
        public async Task<IActionResult> WithdrawResponse(Guid id, Guid responseId, [FromQuery] Guid executorId)
        {
            var order = await _orderRepository.GetByIdAsync(id, CancellationToken.None);
            if (order == null) return NotFound("Заказ не найден");

            var response = order.Responses.FirstOrDefault(r => r.Id == responseId);
            if (response == null) return NotFound("Отклик не найден");

            var executor = await _executorRepository.GetByIdAsync(executorId, CancellationToken.None);
            if (executor == null) return BadRequest("Исполнитель не найден");

            response.Withdraw(executor);
            await _orderRepository.UpdateAsync(order, CancellationToken.None);
            return Ok(response);
        }

        [HttpPost("{id}/accept/{executorId}")]
        public async Task<IActionResult> AcceptResponse(Guid id, Guid executorId, [FromQuery] Guid customerId)
        {
            var order = await _orderRepository.GetByIdAsync(id, CancellationToken.None);
            if (order == null) return NotFound("Заказ не найден");

            var customer = await _customerRepository.GetByIdAsync(customerId, CancellationToken.None);
            if (customer == null) return BadRequest("Заказчик не найден");

            var response = order.Responses.FirstOrDefault(r => r.Executor.Id == executorId);
            if (response == null) return NotFound("Отклик не найден");

            order.AcceptResponse(response, customer);
            await _orderRepository.UpdateAsync(order, CancellationToken.None);
            return Ok(order);
        }

        [HttpPost("{id}/start")]
        public async Task<IActionResult> StartExecution(Guid id, [FromQuery] Guid executorId)
        {
            var order = await _orderRepository.GetByIdAsync(id, CancellationToken.None);
            if (order == null) return NotFound("Заказ не найден");

            var executor = await _executorRepository.GetByIdAsync(executorId, CancellationToken.None);
            if (executor == null) return BadRequest("Исполнитель не найден");

            order.StartExecution(executor);
            await _orderRepository.UpdateAsync(order, CancellationToken.None);
            return Ok(order);
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> Complete(Guid id, [FromQuery] Guid executorId)
        {
            var order = await _orderRepository.GetByIdAsync(id, CancellationToken.None);
            if (order == null) return NotFound("Заказ не найден");

            var executor = await _executorRepository.GetByIdAsync(executorId, CancellationToken.None);
            if (executor == null) return BadRequest("Исполнитель не найден");

            order.Complete(executor);
            await _orderRepository.UpdateAsync(order, CancellationToken.None);
            return Ok(order);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelOrderRequest request)
        {
            var order = await _orderRepository.GetByIdAsync(id, CancellationToken.None);
            if (order == null) return NotFound("Заказ не найден");

            var executor = await _executorRepository.GetByIdAsync(request.ExecutorId, CancellationToken.None);
            if (executor == null) return BadRequest("Исполнитель не найден");

            var reason = request.Reason != null ? CancelReason.Create(request.Reason) : null;
            order.Cancel(executor, reason);
            await _orderRepository.UpdateAsync(order, CancellationToken.None);
            return Ok(order);
        }

        [HttpGet("{id}/responses")]
        public async Task<IActionResult> GetResponses(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id, CancellationToken.None);
            if (order == null) return NotFound();
            return Ok(order.Responses);
        }
    }

    public class CreateOrderRequest
    {
        public Guid CustomerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class RespondRequest
    {
        public Guid ExecutorId { get; set; }
        public string Comment { get; set; }
    }

    public class CancelOrderRequest
    {
        public Guid ExecutorId { get; set; }
        public string Reason { get; set; }
    }
}





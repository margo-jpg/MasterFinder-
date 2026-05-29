using Microsoft.AspNetCore.Mvc;
using MasterFinder.Domain.Entities;
using MasterFinder.Domain.Interfaces;
using MasterFinder.ValueObjects;
using System.Threading;

namespace MasterFinder.WebHost.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExecutorsController : ControllerBase
    {
        private readonly IExecutorRepository _executorRepository;

        public ExecutorsController(IExecutorRepository executorRepository)
        {
            _executorRepository = executorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var executors = await _executorRepository.GetAllAsync(CancellationToken.None);
            return Ok(executors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var executor = await _executorRepository.GetByIdAsync(id, CancellationToken.None);
            if (executor == null) return NotFound();
            return Ok(executor);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExecutorRequest request)
        {
            var executor = new Executor(
                new Username(request.Username),
                new PhoneNumber(request.Phone),
                new Specialization(request.Specialization)
            );
            await _executorRepository.AddAsync(executor, CancellationToken.None);
            return CreatedAtAction(nameof(GetById), new { id = executor.Id }, executor);
        }

        [HttpGet("by-specialization")]
        public async Task<IActionResult> GetBySpecialization([FromQuery] string specialization)
        {
            var executors = await _executorRepository.GetBySpecializationAsync(specialization, CancellationToken.None);
            return Ok(executors);
        }

        [HttpGet("{id}/responses")]
        public async Task<IActionResult> GetResponses(Guid id)
        {
            var responses = await _executorRepository.GetExecutorResponsesAsync(id, CancellationToken.None);
            return Ok(responses);
        }
    }

    public class CreateExecutorRequest
    {
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Specialization { get; set; }
    }
}





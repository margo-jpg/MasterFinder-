using Microsoft.EntityFrameworkCore;
using MasterFinder.Domain.Entities;
using MasterFinder.Domain.Interfaces;

namespace MasterFinder.Infrastructure.Repositories
{
    public class EfExecutorRepository : EfRepository<Executor, Guid>, IExecutorRepository
    {
        public EfExecutorRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Executor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await _dbSet
                .Include("_responses")
                .Include("_executions")
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        public async Task<IEnumerable<Executor>> GetBySpecializationAsync(string specialization, CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(e => e.Specialization.Value.Contains(specialization))
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<Response>> GetExecutorResponsesAsync(Guid executorId, CancellationToken cancellationToken = default)
            => await _context.Set<Response>()
                .Where(r => r.Executor.Id == executorId)
                .Include(r => r.Order)
                .ToListAsync(cancellationToken);
    }
}

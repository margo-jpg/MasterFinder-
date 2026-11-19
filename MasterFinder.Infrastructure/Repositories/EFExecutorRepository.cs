using MasterFinder.Domain.Entities;
using MasterFinder.Domain.Interfaces;
using MasterFinder.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace MasterFinder.Infrastructure.Repositories
{
    public class EfExecutorRepository : EfRepository<Executor, Guid>, IExecutorRepository
    {
        public EfExecutorRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Executor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(e => e.Responses)
                .Include(e => e.Executions)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Executor>> GetBySpecializationAsync(string specialization, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(e => e.Specialization.Value.Contains(specialization))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Response>> GetExecutorResponsesAsync(Guid executorId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Response>()
                .Where(r => r.Executor.Id == executorId)
                .Include(r => r.Order)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Execution>> GetExecutorExecutionsAsync(Guid executorId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Execution>()
                .Where(e => e.Executor.Id == executorId)
                .Include(e => e.Order)
                .ToListAsync(cancellationToken);
        }
    }
}



using Microsoft.EntityFrameworkCore;
using MasterFinder.Domain.Entities;
using MasterFinder.Domain.Enums;
using MasterFinder.Domain.Interfaces;

namespace MasterFinder.Infrastructure.Repositories
{
    public class EfOrderRepository : EfRepository<Order, Guid>, IOrderRepository
    {
        public EfOrderRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await _dbSet
                .Include(o => o.Customer)
                .Include("_responses")
                .ThenInclude(r => r.Executor)
                .Include(o => o.Execution)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        public async Task<IEnumerable<Order>> GetOpenOrdersAsync(CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(o => o.Status == OrderStatus.Open)
                .OrderBy(o => o.CreatedAt)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(o => o.Customer.Id == customerId)
                .ToListAsync(cancellationToken);
    }
}
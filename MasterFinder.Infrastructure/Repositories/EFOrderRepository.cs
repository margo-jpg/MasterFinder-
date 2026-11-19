using MasterFinder.Domain.Entities;
using MasterFinder.Domain.Enums;
using MasterFinder.Domain.Interfaces;
using MasterFinder.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace MasterFinder.Infrastructure.Repositories
{
    public class EfOrderRepository : EfRepository<Order, Guid>, IOrderRepository
    {
        public EfOrderRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(o => o.Customer)
                .Include(o => o.Responses)
                    .ThenInclude(r => r.Executor)
                .Include(o => o.Execution)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetOpenOrdersAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(o => o.Status == OrderStatus.Open)
                .OrderBy(o => o.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(o => o.Customer.Id == customerId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Response>> GetOrderResponsesAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Response>()
                .Where(r => r.Order.Id == orderId)
                .Include(r => r.Executor)
                .ToListAsync(cancellationToken);
        }
    }
}



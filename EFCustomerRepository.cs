using Microsoft.EntityFrameworkCore;
using MasterFinder.Domain.Entities;
using MasterFinder.Domain.Interfaces;

namespace MasterFinder.Infrastructure.Repositories
{
    public class EfCustomerRepository : EfRepository<Customer, Guid>, ICustomerRepository
    {
        public EfCustomerRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await _dbSet
                .Include("_orders")
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public async Task<Customer?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default)
            => await _dbSet
                .FirstOrDefaultAsync(c => c.Phone.Value == phone, cancellationToken);

        public async Task<IEnumerable<Order>> GetCustomerOrdersAsync(Guid customerId, CancellationToken cancellationToken = default)
            => await _context.Set<Order>()
                .Where(o => o.Customer.Id == customerId)
                .ToListAsync(cancellationToken);
    }
}

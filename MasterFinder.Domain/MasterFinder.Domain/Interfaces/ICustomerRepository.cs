using MasterFinder.Domain.Entities;
using MasterFinder.Domain.Interfaces;

namespace MasterFinder.Domain.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer, Guid>
    {
        Task<Customer?> GetByPhoneAsync(string phone, CancellationToken cancellationToken);
        Task<IEnumerable<Order>> GetCustomerOrdersAsync(Guid customerId, CancellationToken cancellationToken);
    }
}
using MasterFinder.Domain.Entities;
using MasterFinder.Domain.Interfaces;

namespace MasterFinder.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order, Guid>
    {
        Task<IEnumerable<Order>> GetOpenOrdersAsync(CancellationToken cancellationToken);       //какие заказы доступны
        Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);    //посмотреть свои заказы
    }
}
using MasterFinder.Domain.Entities;
using MasterFinder.Domain.Interfaces;

namespace MasterFinder.Domain.Interfaces
{
    public interface IExecutorRepository : IRepository<Executor, Guid>
    {
        Task<IEnumerable<Executor>> GetBySpecializationAsync(string specialization, CancellationToken cancellationToken);
        Task<IEnumerable<Response>> GetExecutorResponsesAsync(Guid executorId, CancellationToken cancellationToken);
    }
}
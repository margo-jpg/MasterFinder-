using MasterFinder.Domain.Repositories.Abstractions.Base;

namespace MasterFinder.Domain.Repositories.Abstractions.Base
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        // Так как имя пользователя уникальное
        Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);
    }
}



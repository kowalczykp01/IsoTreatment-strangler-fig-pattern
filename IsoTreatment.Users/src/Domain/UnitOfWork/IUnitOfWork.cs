using Domain.Repositories;

namespace Domain.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    public IUserRepository UserRepository { get; }
    public Task SaveChangesAsync();
}

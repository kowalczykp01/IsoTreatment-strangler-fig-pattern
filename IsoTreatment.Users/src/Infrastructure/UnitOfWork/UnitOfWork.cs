using Domain.Repositories;
using Domain.UnitOfWork;
using Infrastructure.Repositories;

namespace Infrastructure.UnitOfWork;

public sealed class UnitOfWork(UserDbContext context) : IUnitOfWork
{
    private readonly Lazy<IUserRepository> _userRepository = new(() => new UserRepository(context));

    public IUserRepository UserRepository => _userRepository.Value;
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Dispose(bool disposing)
    {
        if (disposing)
        {
            context.Dispose();
        }
    }

    public Task SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }
}

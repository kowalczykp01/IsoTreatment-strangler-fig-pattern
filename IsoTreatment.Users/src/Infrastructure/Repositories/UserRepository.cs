using System;
using Domain;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class UserRepository(UserDbContext dbContext) : IUserRepository
{
    public async Task CreateAsync(User user)
    {
        await dbContext.Users.AddAsync(user);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await dbContext.Users.FindAsync(id);
        if (user is not null)
        {
            dbContext.Users.Remove(user);
        }
    }

    public async Task<User?> GetByIdAsync(int id) => await dbContext.Users.FindAsync(id);

    public async Task<User?> GetByEmailAsync(string email) =>
        await dbContext.Users.SingleOrDefaultAsync(x => x.Email == email);

    public async Task UpdateAsync(User user) => dbContext.Users.Update(user);
}

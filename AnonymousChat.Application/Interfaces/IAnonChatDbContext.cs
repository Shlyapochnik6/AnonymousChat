using AnonymousChat.Domain;
using Microsoft.EntityFrameworkCore;

namespace AnonymousChat.Application.Interfaces;

public interface IAnonChatDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Message> Messages { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
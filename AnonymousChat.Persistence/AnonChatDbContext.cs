using AnonymousChat.Application.Interfaces;
using AnonymousChat.Domain;
using Microsoft.EntityFrameworkCore;

namespace AnonymousChat.Persistence;

public class AnonChatDbContext : DbContext, IAnonChatDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    
    public AnonChatDbContext(DbContextOptions<AnonChatDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}
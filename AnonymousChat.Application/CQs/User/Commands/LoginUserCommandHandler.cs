using System.Security.Claims;
using AnonymousChat.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AnonymousChat.Application.CQs.User.Commands;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ClaimsIdentity>
{
    private readonly IAnonChatDbContext _dbContext;

    public LoginUserCommandHandler(IAnonChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ClaimsIdentity> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Name == request.Name, cancellationToken);
        if (user == null)
        {
            user = new Domain.User()
            {
                Name = request.Name
            };
            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name)
        };
        var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
    }
}
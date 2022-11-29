using AnonymousChat.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AnonymousChat.Application.CQs.User.Queries;

public class GetListUsersQueryHandler : IRequestHandler<GetListUsersQuery, UsersVm>
{
    private readonly IAnonChatDbContext _dbContext;

    public GetListUsersQueryHandler(IAnonChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<UsersVm> Handle(GetListUsersQuery request, CancellationToken cancellationToken)
    {
        var names = await _dbContext.Users
            .Select(u => u.Name).ToListAsync(cancellationToken);
        return new UsersVm() { Names = names };
    }
}
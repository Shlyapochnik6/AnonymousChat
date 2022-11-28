using AnonymousChat.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AnonymousChat.Application.CQs.Message.Queries;

public class GetListMessagesQueryHandler : IRequestHandler<GetListMessagesQuery, MessagesVm>
{
    private readonly IAnonChatDbContext _dbContext;

    public GetListMessagesQueryHandler(IAnonChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<MessagesVm> Handle(GetListMessagesQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Name == request.Name,
                cancellationToken: cancellationToken);
        if (user is null)
            return new MessagesVm()
            {
                Messages = new List<Domain.Message>()
            };
        var messages = _dbContext.Messages
            .Where(m => m.User.Id == user.Id)
            .OrderByDescending(m => m.DateSend);
        return new MessagesVm() { Messages = messages };
    }
}
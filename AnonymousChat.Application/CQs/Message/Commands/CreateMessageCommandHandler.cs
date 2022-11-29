using AnonymousChat.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AnonymousChat.Application.CQs.Message.Commands;

public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, MessageDto>
{
    private readonly IAnonChatDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateMessageCommandHandler(IAnonChatDbContext dbContext,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<MessageDto> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var recipient = await _dbContext.Users
            .FirstOrDefaultAsync(r => r.Name == request.Recipient, cancellationToken);
        if (recipient == null)
            throw new NullReferenceException($"The entered recipient doesn't exist");
        var message = new Domain.Message()
        {
            Author = request.Author,
            Recipient = request.Recipient,
            Header = request.Header,
            Body = request.Body,
            DateSend = DateTimeOffset.Now.DateTime.AddHours(3),
            User = recipient
        };
        await _dbContext.Messages.AddAsync(message, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<MessageDto>(message);
    }
}
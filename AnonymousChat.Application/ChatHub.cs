using System.Security.Claims;
using AnonymousChat.Application.CQs.Message.Commands;
using AnonymousChat.Application.Interfaces;
using AnonymousChat.Domain;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AnonymousChat.Application;

public class ChatHub : Hub
{
    private readonly IMediator _mediator;
    private readonly IAnonChatDbContext _dbContext;
    
    public ChatHub(IMediator mediator, IAnonChatDbContext dbContext)
    {
        _mediator = mediator;
        _dbContext = dbContext;
    }
    
    public async Task Send(string body, 
        string recipient, string header)
    {
        var command = new CreateMessageCommand
        {
            Author = Context.User?.Identity?.Name!,
            Body = body,
            Recipient = recipient,
            Header = header
        };
        var message = await _mediator.Send(command);
        var isUserExist = await _dbContext.Users
            .AnyAsync(u => u.Name == recipient);
        if (!isUserExist)
            throw new NullReferenceException($"User named {recipient} does not exist");
        await Clients.User(recipient).SendAsync("Receive", message);
    }
}   
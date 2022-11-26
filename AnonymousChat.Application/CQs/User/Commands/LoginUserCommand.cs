using System.Security.Claims;
using MediatR;

namespace AnonymousChat.Application.CQs.User.Commands;

public class LoginUserCommand : IRequest<ClaimsIdentity>
{
    public string Name { get; set; }
    public string ConnectionId { get; set; }
}
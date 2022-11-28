using MediatR;

namespace AnonymousChat.Application.CQs.Message.Queries;

public class GetListMessagesQuery : IRequest<MessagesVm>
{
    public string? Name { get; set; }
}
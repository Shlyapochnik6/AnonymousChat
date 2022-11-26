using MediatR;

namespace AnonymousChat.Application.CQs.Message.Commands;

public class CreateMessageCommand : IRequest<MessageDto>
{
    public string Author { get; set; }
    
    public string Recipient { get; set; }
    
    public string Header { get; set; }
    
    public string Body { get; set; }
}
namespace AnonymousChat.Application.CQs.Message.Queries;

public class MessagesVm
{
    public IEnumerable<Domain.Message> Messages { get; set; }
}
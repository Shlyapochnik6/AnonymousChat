using AnonymousChat.Application.Common.Mappings;
using AutoMapper;

namespace AnonymousChat.Application.CQs.Message.Commands;

public class MessageDto : IMapWith<Domain.Message>
{
    public string Author { get; set; }
    
    public string Recipient { get; set; }
    
    public string Header { get; set; }
    
    public string Body { get; set; }
    
    public DateTime DateSend { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Message, MessageDto>()
            .ForMember(c => c.Author,
                o => o.MapFrom(c => c.Author))
            .ForMember(c => c.Recipient,
                o => o.MapFrom(c => c.Recipient))
            .ForMember(c => c.Header,
                o => o.MapFrom(c => c.Header))
            .ForMember(c => c.Body,
                o => o.MapFrom(c => c.Body))
            .ForMember(c => c.DateSend,
                o => o.MapFrom(c => c.DateSend));
    }
}
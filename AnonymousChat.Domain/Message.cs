namespace AnonymousChat.Domain;

public class Message
{
    public long Id { get; set; }
    
    public string Recipient { get; set; }
    
    public string Header { get; set; }
    
    public string Body { get; set; }
    
    public DateTime DateSend { get; set; }
    
    public User? User { get; set; }
}
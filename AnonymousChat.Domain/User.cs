namespace AnonymousChat.Domain;

public class User
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public List<Message> Messages { get; set; }
}
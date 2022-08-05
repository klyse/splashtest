namespace web.Models;

public class RunDto
{
    public Guid Id { get; set; }
    public State State { get; set; }
    public DateTime RunDateTime { get; set; }
}
namespace web.Models;

public class Run
{
    public Guid Id { get; set; }
    public State State { get; set; }

    public Guid TestId { get; set; }
    public Test Test { get; set; }
}
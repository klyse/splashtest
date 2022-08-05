namespace web.Models;

public class Run
{
    public Guid Id { get; set; }
    public State State { get; set; }
    public DateTime RunDateTime { get; set; }
    public Guid TestId { get; set; }
    public Test Test { get; set; }

    public RunDto Project() => new RunDto
    {
        Id = Id,
        State = State,
        RunDateTime = RunDateTime
    };
}
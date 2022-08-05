namespace web.Models;

public enum State
{
    Pending,
    Running,
    Failed,
    Succeeded
}

public class Test
{
    public Guid Id { get; set; }
    public State State { get; set; }
    public string Result { get; set; }
    public string TestCode { get; set; }
}
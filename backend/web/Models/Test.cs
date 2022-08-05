namespace web.Models;

public class Test
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<TestStep>? TestSteps { get; set; }

    public ICollection<Run> Runs { get; }

    public Test()
    {
        TestSteps = new HashSet<TestStep>();
        Runs = new HashSet<Run>();
    }
}
namespace web.Models;

public class Test
{
    public Test()
    {
        TestSteps = new HashSet<TestStep>();
        Runs = new HashSet<Run>();
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedDateTime { get; set; }
    public ICollection<TestStep>? TestSteps { get; set; }

    public ICollection<Run> Runs { get; }

    public TestDto Project()
    {
        return new()
        {
            Id = Id,
            Name = Name,
            TestCode = TestSteps,
            CreatedDateTime = CreatedDateTime,
            Runs = Runs.OrderByDescending(c => c.RunDateTime).Select(c => c.Project()).ToList()
        };
    }
}
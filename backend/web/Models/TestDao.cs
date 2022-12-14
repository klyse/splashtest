namespace web.Models;

public class TestDao
{
    public string Name { get; set; }
    public ICollection<TestStep>? TestSteps { get; set; }

    public Test Project()
    {
        return new()
        {
            Name = Name,
            TestSteps = TestSteps
        };
    }
}
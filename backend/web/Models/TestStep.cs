namespace web.Models;

public class TestStep
{
    public TestStepTypes Type { get; set; }
    public string Value { get; set; } = null!;
    public string? Find { get; set; }
}
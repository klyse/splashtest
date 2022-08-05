namespace web.Models;

public class TestDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<TestStep>? TestCode { get; set; }
}
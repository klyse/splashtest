namespace web.Models;

public class TestDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public ICollection<TestStep>? TestCode { get; set; }
    public ICollection<RunDto> Runs { get; set; }
}
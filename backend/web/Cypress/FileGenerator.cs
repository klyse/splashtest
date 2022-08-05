using System.Text;
using web.Models;

namespace web.Cypress;

public class FileGenerator
{
    public string GetFileContent(ICollection<TestStep> testSteps)
    {
        var fileContent = new StringBuilder();
        fileContent.Append(@"
describe('empty spec', () => {
  it('passes', () => {
");

        foreach (var testStep in testSteps)
        {
            switch (testStep.Type)
            {
                case TestStepTypes.NavigateTo:
                    fileContent.Append(@$"
    cy.visit('{testStep.Value}')
");
                    break;
            }
        }

        fileContent.Append(@"
  })
})
");
        return fileContent.ToString();
    }
}
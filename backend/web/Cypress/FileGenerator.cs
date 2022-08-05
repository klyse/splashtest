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
                case TestStepTypes.Visit:
                    fileContent.Append(@$"
    cy.visit('{testStep.Value}')
");
                    break;
                case TestStepTypes.Click:
                    fileContent.Append(@$"
    cy.contains('{testStep.Value}').click()
");
                    break;
                case TestStepTypes.Contains:
                    fileContent.Append(@$"
    cy.contains('{testStep.Value}')
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
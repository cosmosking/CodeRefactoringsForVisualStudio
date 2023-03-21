using ExtractTryAndFinallyBody;
using Microsoft.CodeAnalysis.CodeRefactorings;
using NUnit.Framework;

namespace CodeRefactoringsForVisualStudio.Tests.ExtractTryAndFinallyBodyTests
{
    public class ExtractTryAndFinallyBodyCSharpCodeTests : BaseCodeRefactoringTestFixture
    {
        protected override CodeRefactoringProvider CreateProvider()
        {
            return new ExtractTryAndFinallyBodyCSharpCodeRefactoringProvider();
        }

        [TestCase("ShouldExtractTryBody")]
        [TestCase("ShouldExtractTryAndFinallyBody")]
        public void Should(string caseName)
        {
            TestCodeRefactoring("ExtractTryAndFinallyBody.Data", caseName);
        }
    }

}
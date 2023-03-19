using InvertAssignmentDirection;
using Microsoft.CodeAnalysis.CodeRefactorings;
using NUnit.Framework;

namespace CodeRefactoringsForVisualStudio.Tests.InvertAssignmentDirectionTests
{
    public class InvertAssignmentDirectionCSharpCodeTests : BaseCodeRefactoringTestFixture
    {
        protected override CodeRefactoringProvider CreateProvider()
        {
            return new InvertAssignmentDirectionCSharpCodeRefactoringProvider();
        }


        [TestCase("ShouldInvertSelectedSingleAssignment")]
        [TestCase("ShouldInvertSelectedTwoAssignments")]
        [TestCase("ShouldPreserveLeftTrivia")]
        [TestCase("ShouldPreserveRightTrivia")]
        [TestCase("ShouldInvertAssignmentWithMemberAccessOnBothSides")]
        [TestCase("ShouldInvertAssignmentWithMemberAccessOnOneSide")]
        [TestCase("ShouldInvertAssignmentWithElementAccessOnBothSides")]
        public void Should(string caseName)
        {
            TestCodeRefactoring("InvertAssignmentDirection.Data", caseName);
        }
    }

    public class InvertAssignmentDirectionVisualBasicCodeTests : VBCodeRefactoringTestFixture
    {
        protected override CodeRefactoringProvider CreateProvider()
        {
            return new InvertAssignmentDirectionVBCodeRefactoringProvider();
        }


        [TestCase("ShouldInvertSelectedSingleAssignment")]
        [TestCase("ShouldInvertSelectedTwoAssignments")]
        [TestCase("ShouldPreserveLeftTrivia")]
        [TestCase("ShouldPreserveRightTrivia")]
        [TestCase("ShouldInvertAssignmentWithMemberAccessOnBothSides")]
        [TestCase("ShouldInvertAssignmentWithMemberAccessOnOneSide")]
        [TestCase("ShouldInvertAssignmentWithElementAccessOnBothSides")]
        public void Should(string caseName)
        {
            TestCodeRefactoring("InvertAssignmentDirection.Data", caseName);
        }
    }
}
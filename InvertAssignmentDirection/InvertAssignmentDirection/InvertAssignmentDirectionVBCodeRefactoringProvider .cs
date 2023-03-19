using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace InvertAssignmentDirection
{
    [ExportCodeRefactoringProvider(LanguageNames.VisualBasic, Name = nameof(InvertAssignmentDirectionVBCodeRefactoringProvider)), Shared]
    public class InvertAssignmentDirectionVBCodeRefactoringProvider : CodeRefactoringProvider
    {
        public sealed override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
        {

            SyntaxNode rootNode = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var selectedAssignmentExpressions = rootNode.ExtractSelectedNodesOfType<AssignmentStatementSyntax>(context.Span).Where(x => IsHandledExpressionSyntax(x.Left) && IsHandledExpressionSyntax(x.Right));

            if (selectedAssignmentExpressions.Any())
            {
                var action = CodeAction.Create("Invert assignment", cancellationToken => InvertAssignments(context.Document, selectedAssignmentExpressions, cancellationToken));
                context.RegisterRefactoring(action);
            }
        }


        private async Task<Document> InvertAssignments(Document document, IEnumerable<AssignmentStatementSyntax> assignmentExpressions, CancellationToken cancellationToken)
        {
            SyntaxNode rootNode = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            rootNode = rootNode.ReplaceNodes(assignmentExpressions, InvertAssignmentExpression);
            return document.WithSyntaxRoot(rootNode);
        }

        private SyntaxNode InvertAssignmentExpression(AssignmentStatementSyntax originalNode, AssignmentStatementSyntax _)
        {
            ExpressionSyntax left = originalNode.Right.WithoutTrivia().WithTriviaFrom(originalNode.Left);
            ExpressionSyntax right = originalNode.Left.WithoutTrivia().WithTriviaFrom(originalNode.Right);
            SyntaxNode invertedAssigment = originalNode.WithLeft(left).WithRight(right);

            return invertedAssigment;
        }

        private bool IsHandledExpressionSyntax(VisualBasicSyntaxNode node)
        {
            bool result = false;

            switch (node)
            {
                case IdentifierNameSyntax _:
                case MemberAccessExpressionSyntax _:
                case InvocationExpressionSyntax _:
                    result = true;
                    break;
            }

            return result;
        }
    }
}

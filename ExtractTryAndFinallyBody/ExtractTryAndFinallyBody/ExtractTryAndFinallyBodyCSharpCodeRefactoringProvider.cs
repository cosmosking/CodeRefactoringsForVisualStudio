using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace ExtractTryAndFinallyBody
{
    [ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = nameof(ExtractTryAndFinallyBodyCSharpCodeRefactoringProvider)), Shared]
    public class ExtractTryAndFinallyBodyCSharpCodeRefactoringProvider : CodeRefactoringProvider
    {
        public sealed override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
        {
            SyntaxNode rootNode = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var selectedTryExpressions = rootNode.ExtractSelectedNodesOfType<TryStatementSyntax>(context.Span);

            if (selectedTryExpressions.Any())
            {
                var action = CodeAction.Create("Extract Try Body", cancellationToken => ExtractTryBody(context.Document, selectedTryExpressions, cancellationToken));
                context.RegisterRefactoring(action);
            }
        }


        private async Task<Document> ExtractTryBody(Document document, IEnumerable<TryStatementSyntax> assignmentExpressions, CancellationToken cancellationToken)
        {
            SyntaxNode rootNode = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            rootNode = rootNode.ReplaceNodes(assignmentExpressions, ExtractTryBodyExpression);
            return document.WithSyntaxRoot(rootNode);
        }

        private SyntaxNode ExtractTryBodyExpression(TryStatementSyntax originalNode, TryStatementSyntax _)
        {
            SyntaxNode body = originalNode.Block.WithoutTrivia();
            SyntaxNode final = originalNode.Finally.WithoutTrivia();
            return body;
        }

        private bool IsHandledExpressionSyntax(CSharpSyntaxNode node)
        {
            bool result = false;

            switch (node)
            {
                case IdentifierNameSyntax _:
                case MemberAccessExpressionSyntax _:
                case ElementAccessExpressionSyntax _:
                    result = true;
                    break;
            }

            return result;
        }
    }
}

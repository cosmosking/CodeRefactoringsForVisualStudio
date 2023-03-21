using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Formatting;

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

        private async Task<Document> ExtractTryBody(Document document, IEnumerable<TryStatementSyntax> tryExpressions, CancellationToken cancellationToken)
        {
            SyntaxNode rootNode = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            rootNode = rootNode.ReplaceNodes(tryExpressions, ExtractTryBodyExpression);
            
            return document.WithSyntaxRoot(rootNode);
        }

        private SyntaxNode ExtractTryBodyExpression(TryStatementSyntax originalNode, TryStatementSyntax _)
        {
            var oldTryBody = originalNode.Block.Statements;
            
            var newTryBody = SyntaxFactory.Block(oldTryBody);
            var finalyBody = originalNode.Finally?.Block;
            if(finalyBody != null)
            {
                foreach (var statement in finalyBody.Statements)
                {
                    newTryBody = newTryBody.AddStatements(statement);
                }
            }
            //newTryBody = newTryBody.ReplaceToken(newTryBody.OpenBraceToken, SyntaxFactory.Token(SyntaxKind.None))
            //                       .ReplaceToken(newTryBody.CloseBraceToken, SyntaxFactory.Token(SyntaxKind.None));
            return newTryBody.WithTriviaFrom(originalNode);
        }

    }
}

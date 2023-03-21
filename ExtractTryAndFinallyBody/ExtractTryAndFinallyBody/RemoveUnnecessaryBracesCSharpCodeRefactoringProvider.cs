using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExtractTryAndFinallyBody
{
    [ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = nameof(RemoveUnnecessaryBracesCSharpCodeRefactoringProvider)), Shared]
    public class RemoveUnnecessaryBracesCSharpCodeRefactoringProvider : CodeRefactoringProvider
    {
        public sealed override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
        {
            SyntaxNode rootNode = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var selectedMethodDeclaration = rootNode.ExtractSelectedNodesOfType<MethodDeclarationSyntax>(context.Span);

            if (selectedMethodDeclaration.Any())
            {
                var action = CodeAction.Create("Remove Unnecessary Braces", cancellationToken => RemoveUnnecessaryBraces(context.Document, selectedMethodDeclaration, cancellationToken));
                context.RegisterRefactoring(action);
            }
        }

        private async Task<Microsoft.CodeAnalysis.Document> RemoveUnnecessaryBraces(Microsoft.CodeAnalysis.Document document, IEnumerable<MethodDeclarationSyntax> methodDeclarations, CancellationToken cancellationToken)
        {
            SyntaxNode rootNode = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            rootNode = rootNode.ReplaceNodes(methodDeclarations, ReplaceMethodDeclaration);

            return document.WithSyntaxRoot(rootNode);
        }

        private SyntaxNode ReplaceMethodDeclaration(MethodDeclarationSyntax originalNode, MethodDeclarationSyntax _)
        {

            var oldMethodBody = originalNode.Body;

            var newMethodBody = SyntaxFactory.Block();

            if (oldMethodBody.ChildNodes() != null)
            {
                foreach (var state in oldMethodBody.Statements)
                {
                    if (state is BlockSyntax)
                    {
                        var block = state as BlockSyntax;
                        var leading = block.GetLeadingTrivia();
                        var trailing = block.GetTrailingTrivia();
                        int i = 0;
                        int count = block.Statements.Count;

                        foreach (var subState in block.Statements)
                        {
                            var newSubState = subState;
                            if (i == 0)
                            {
                                newSubState = newSubState.WithLeadingTrivia(leading);
                            }
                            if (i == count - 1)
                            {
                                newSubState = newSubState.WithTrailingTrivia(trailing);
                            }
                            newMethodBody = newMethodBody.AddStatements(newSubState);

                            i++;
                        }
                    }
                    else
                    {
                        newMethodBody = newMethodBody.AddStatements(state);
                    }
                }
            }

            var newMethod = originalNode.WithBody(newMethodBody.WithTriviaFrom(oldMethodBody));
            return newMethod;
        }

    }
}

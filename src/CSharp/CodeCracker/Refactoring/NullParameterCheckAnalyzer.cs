using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace CodeCracker.CSharp.Refactoring
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NullParameterCheckAnalyzer : DiagnosticAnalyzer
    {
        internal const string Title = "You should check for null parameters.";
        internal const string MessageFormat = "Check for null parameters to avoid NullReferenceException.";
        internal const string Category = SupportedCategories.Refactoring;

        internal static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId.NullParameterCheck.ToDiagnosticId(),
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            helpLinkUri: HelpLink.ForDiagnostic(DiagnosticId.NullParameterCheck));

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context) =>
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.MethodDeclaration);

        private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            if (context.IsGenerated()) return;
            var localDeclaration = (MethodDeclarationSyntax)context.Node;
            if (!localDeclaration.ParameterList.Parameters.Any()) return;

            var variableDeclaration = localDeclaration.ChildNodes()
                .OfType<ParameterListSyntax>()
                .FirstOrDefault();

            var semanticModel = context.SemanticModel;
            foreach (var parameter in localDeclaration.ParameterList.Parameters)
            {
                var variableType = semanticModel.GetTypeInfo(parameter).ConvertedType;
                if (!variableType.IsReferenceType) continue;

                var diagnostic = Diagnostic.Create(Rule, parameter.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
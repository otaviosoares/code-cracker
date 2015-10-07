using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;

namespace CodeCracker.CSharp.Refactoring
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NullParameterCheckCodeFixProvider)), Shared]
    public class NullParameterCheckCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(DiagnosticId.NullParameterCheck.ToDiagnosticId());

        public override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
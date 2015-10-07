using CodeCracker.CSharp.Refactoring;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Xunit;

namespace CodeCracker.Test.CSharp.Refactoring
{
    public class NullParameterCheckTests : CodeFixVerifier<NullParameterCheckAnalyzer, NullParameterCheckCodeFixProvider>
    {
        [Fact]
        public async Task IgnoresWhenMethodHasNoParameters()
        {
            const string test = @"void main()
{}";
            await VerifyCSharpHasNoDiagnosticsAsync(test.WrapInCSharpMethod());
        }

        [Fact]
        public async Task IgnoresWhenParameterHasValueType()
        {
            const string test = @"void main(int i)
{}";
            await VerifyCSharpHasNoDiagnosticsAsync(test.WrapInCSharpMethod());
        }

        [Fact]
        public async Task ReferenceTypeParameterProducesDiagnostic()
        {
            const string source = @"void main(string s)
{}";

            var expected = new DiagnosticResult
            {
                Id = DiagnosticId.NullParameterCheck.ToDiagnosticId(),
                Message = "Check for null parameters to avoid NullReferenceException.",
                Severity = DiagnosticSeverity.Hidden,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 2, 11) }
            };
            await VerifyCSharpDiagnosticAsync(source, expected);
        }
    }
}
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
            const string source = @"void main(string i)
{}";

            var expected = new DiagnosticResult
            {
                Id = DiagnosticId.StringRepresentation_RegularString.ToDiagnosticId(),
                Message = "Change to regular string",
                Severity = DiagnosticSeverity.Hidden,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 6, 17) }
            };
            await VerifyCSharpDiagnosticAsync(source, expected);
        }
    }
}
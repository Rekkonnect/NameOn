using Gu.Roslyn.Asserts;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoseLynn.Analyzers;
using RoseLynn.Testing;

namespace NameOn.Test
{
    public abstract class BaseNAMEDiagnosticTests<TAnalyzer> : BaseNAMEDiagnosticTests
        where TAnalyzer : DiagnosticAnalyzer, new()
    {
        protected sealed override DiagnosticAnalyzer GetNewDiagnosticAnalyzerInstance() => new TAnalyzer();
    }

    public abstract class BaseNAMEDiagnosticTests : BaseDiagnosticTests
    {
        protected ExpectedDiagnostic ExpectedDiagnostic => ExpectedDiagnostic.Create(TestedDiagnosticRule);
        protected sealed override DiagnosticDescriptorStorageBase DiagnosticDescriptorStorage => NAMEDiagnosticDescriptorStorage.Instance;

        protected override UsingsProviderBase GetNewUsingsProviderInstance()
        {
            return NAMEUsingsProvider.Instance;
        }

        protected override void ValidateCode(string testCode)
        {
            RoslynAssert.Valid(GetNewDiagnosticAnalyzerInstance(), testCode);
        }
        protected override void AssertDiagnostics(string testCode)
        {
            RoslynAssert.Diagnostics(GetNewDiagnosticAnalyzerInstance(), ExpectedDiagnostic, testCode);
        }

        [TestMethod]
        public void EmptyCode()
        {
            ValidateCode(@"");
        }
        [TestMethod]
        public void EmptyCodeWithUsings()
        {
            ValidateCode(NAMEUsingsProvider.DefaultUsings);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NameOn.Test.NameOfUsage
{
    [TestClass]
    public sealed class NAME0001_Tests : NameOfUsageAnalyzerDiagnosticTests
    {
        [TestMethod]
        public void PropertyNameOf()
        {
            var testCode =
@"
class Program
{
    [ForceNameOf]
    string Property { get; set; }

    void Run()
    {
        Property = ↓""nothing"";
    }
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
        [TestMethod]
        public void FieldNameOf()
        {
            var testCode =
@"
class Program
{
    [ForceNameOf]
    string Field;

    void Run()
    {
        Field = ↓""nothing"";
    }
}
";

            AssertDiagnosticsWithUsings(testCode);
        }

        [TestMethod]
        public void FunctionArgumentNameOf()
        {
            var testCode =
@"
class Program
{
    void Run()
    {
        Function(nameof(Run));
        Function(↓""Run"");
    }

    void Function([ForceNameOf] string name)
    {
    }
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}

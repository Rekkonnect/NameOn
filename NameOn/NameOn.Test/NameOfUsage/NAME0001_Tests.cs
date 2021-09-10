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

        [TestMethod]
        public void DelegateInvocationArgumentNameOf()
        {
            var testCode =
@"
class Program
{
    void Run(Function f)
    {
        f(nameof(Run));
        f(↓""Run"");
    }

    delegate void Function([ForceNameOf] string name);
}
";

            AssertDiagnosticsWithUsings(testCode);
        }

        [TestMethod]
        public void ConstructorAssignmentNameOf()
        {
            var testCode =
@"
class C
{
    [ForceNameOf]
    string Value { get; set; }

    public C(string value)
    {
        Value = ↓value;
    }
    public C([ForceNameOf] string value, bool dummy)
    {
        Value = value;
    }
}
";

            AssertDiagnosticsWithUsings(testCode);
        }

        [TestMethod]
        public void ConstructorInvocationNameOf()
        {
            var testCode =
@"
class Program
{
    void Function()
    {
        new C(↓""value"");
        new C(nameof(C));
    }
}

class C
{
    public C([ForceNameOf] string value)
    {
    }
}
";

            AssertDiagnosticsWithUsings(testCode);
        }

        [TestMethod]
        public void FunctionReturnNameOf()
        {
            var testCode =
@"
class Program
{
    [return: ForceNameOf]
    string Function()
    {
        return ↓""value"";
    }
    [return: ForceNameOf]
    string AnotherFunction()
    {
        return nameof(AnotherFunction);
    }
}
";

            AssertDiagnosticsWithUsings(testCode);
        }

        [TestMethod]
        public void DelegateReturnNameOf()
        {
            var testCode =
@"
class Program
{
    void Function()
    {
        NameGetter getter = ↓GetRandomString;
        NameGetter anotherGetter = GetName;

        var name = anotherGetter();
        AcceptName(name);
    }
    
    void AcceptName([ForceNameOf] string name) { }

    string GetRandomString()
    {
        return ""random"";
    }
    [return: ForceNameOf]
    string GetName()
    {
        return nameof(NameGetter);
    }

    [return: ForceNameOf]
    delegate string NameGetter();
}
";

            AssertDiagnosticsWithUsings(testCode);
        }
    }
}

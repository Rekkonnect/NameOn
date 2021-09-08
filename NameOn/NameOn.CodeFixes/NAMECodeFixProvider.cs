using RoseLynn.CodeFixes;
using System.Resources;

namespace NameOn.CodeFixes
{
    public abstract class NAMECodeFixProvider : MultipleDiagnosticCodeFixProvider
    {
        protected sealed override ResourceManager ResourceManager => CodeFixResources.ResourceManager;
    }
}

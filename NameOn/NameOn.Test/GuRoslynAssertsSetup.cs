using Gu.Roslyn.Asserts;
using NameOn.Test;

// Properly includes all the required assemblies for the compiled code in the tests
[assembly: TransitiveMetadataReferences
(
    // All assemblies that this assembly depends on
    typeof(BaseNAMEDiagnosticTests)
)]
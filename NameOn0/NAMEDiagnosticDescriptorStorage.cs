using Microsoft.CodeAnalysis;
using RoseLynn.Analyzers;
using System.Resources;

namespace NameOn
{
    internal sealed class NAMEDiagnosticDescriptorStorage : DiagnosticDescriptorStorageBase
    {
        public static readonly NAMEDiagnosticDescriptorStorage Instance = new();

        protected override string BaseRuleDocsURI => "https://github.com/AlFasGD/NameOn/blob/master/docs/rules";
        protected override string DiagnosticIDPrefix => "NAME";
        protected override ResourceManager ResourceManager => Resources.ResourceManager;

        #region Category Constants
        public const string APIRestrictionsCategory = "API Restrictions";
        public const string BrevityCategory = "Brevity";
        public const string DesignCategory = "Design";
        public const string InformationCategory = "Information";
        public const string ValidityCategory = "Validity";
        #endregion

        #region Rules
        [DiagnosticSupported(typeof(NameOfUsageAnalyzer))]
        public readonly DiagnosticDescriptor
            NAME0001_Rule,
            NAME0002_Rule;

        private NAMEDiagnosticDescriptorStorage()
        {
            NAME0001_Rule = CreateDiagnosticDescriptor(0001, APIRestrictionsCategory, DiagnosticSeverity.Error);
            NAME0002_Rule = CreateDiagnosticDescriptor(0002, ValidityCategory, DiagnosticSeverity.Error);
        }
        #endregion
    }
}

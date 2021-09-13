using RoseLynn;
using System;

namespace NameOn.Core
{
    [AttributeUsage(AttributeTargeting.NameOfTargets, Inherited = true, AllowMultiple = false)]
    public sealed class ProhibitNameOfAttribute : NameOfRestrictionAttributeBase
    {
        public override NameOfRestrictions Restrictions => NameOfRestrictions.Prohibit;

        public ProhibitNameOfAttribute()
            : base() { }
        public ProhibitNameOfAttribute(IdentifiableSymbolKind affectedSymbolKinds)
            : base(affectedSymbolKinds) { }
    }
}

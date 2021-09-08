using System;

namespace NameOn.Core
{
    public static class AttributeTargeting
    {
        public const AttributeTargets NameOfTargets = AttributeTargets.Parameter
                                                    | AttributeTargets.Field
                                                    | AttributeTargets.Property
                                                    | AttributeTargets.ReturnValue;
    }
}

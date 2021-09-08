using System;

namespace NameOn.Core
{
    // Changes to this should also be reflected in NameOfRestrictionAssociation.RestrictionDictionary
    [Flags]
    public enum NamedSymbolKind : uint
    {
        None = 0,

        Namespace = 1,

        // Types
        Class = 1 << 1,
        Struct = 1 << 2,
        Interface = 1 << 3,
        Delegate = 1 << 4,
        Enum = 1 << 5,
        Record = 1 << 6,

        RecordClass = Record | Class,
        RecordStruct = Record | Struct,
        AnyRecord = RecordClass | RecordStruct,

        // Parameters
        Parameter = 1 << 10,
        GenericParameter = 1 << 11,

        // Members
        Field = 1 << 16,
        Property = 1 << 17,
        Event = 1 << 18,
        Method = 1 << 19,

        Alias = 1U << 31,

        // Reserve all flags that might exist in the future 
        All = ~None,
    }
}

﻿using RoseLynn.Testing;

namespace NameOn.Test
{
    public sealed class NAMEUsingsProvider : UsingsProviderBase
    {
        public static readonly NAMEUsingsProvider Instance = new();

        public const string DefaultUsings =
@"
using NameOn.Core;
using System;
using System.Collections;
using System.Collections.Generic;
";

        public override string DefaultNecessaryUsings => DefaultUsings;

        private NAMEUsingsProvider() { }
    }
}

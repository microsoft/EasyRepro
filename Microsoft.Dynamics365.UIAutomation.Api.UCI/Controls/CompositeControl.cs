// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class CompositeControl : FieldReference
    {
        public string Id { get; set; }
        public List<Field> Fields { get; set; }
    }
}

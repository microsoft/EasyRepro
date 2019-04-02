// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class CompositeControl
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Field> Fields { get; set; }
    }
}

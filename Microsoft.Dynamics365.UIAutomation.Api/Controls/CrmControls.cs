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

    /// <summary>
    /// Represents an individual field on a Dynamics 365 Customer Experience web form.
    /// </summary>
    public class Field
    {
        /// <summary>
        /// Gets or sets the identifier of the field.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the field.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }
    }

    /// <summary>
    /// Represents an Option Set in Dynamics 365.
    /// </summary>
    public class OptionSet
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class MultiValueOptionSet
    {
        public string Name { get; set; }
        public string[] Values { get; set; }
    }

    public class Lookup
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int Index { get; set; }
    }

}

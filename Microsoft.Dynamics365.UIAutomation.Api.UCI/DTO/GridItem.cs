// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class GridItem
    {
        public GridItem()
        {
            this._attributes = new Dictionary<string, object>();
        }

        private Dictionary<string, object> _attributes;

        public Guid Id { get; set; }
        public string EntityName { get; set; }
        public Uri Url { get; set; }

        public object this[string attributeName]
        {
            get
            {
                if (this._attributes.ContainsKey(attributeName))
                    return this._attributes[attributeName];

                return null;
            }
            set
            {
                this._attributes[attributeName] = value;
            }
        }

        public T GetAttribute<T>(string attributeName)
        {
            return (T)this[attributeName];
        }
    }
}
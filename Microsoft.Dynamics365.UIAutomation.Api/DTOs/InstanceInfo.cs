// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class InstanceInfo
    {
        public Guid OrganizationId { get; set; }
        public Uri OrganizationUri { get; set; }
        public string UniqueName { get; set; }
        public string FriendlyName { get; set; }
        public string Purpose { get; set; }
        public string InstanceType { get; set; }
        public int? OrganizationServiceHealth { get; set; }
        public int? EffectiveUsedStorageMB { get; set; }
        public bool? UpdateAvailable { get; set; }
        public string Version { get; set; }

        public bool OpenWith(Browser browser)
        {
            browser.GoToXrmUri(this.OrganizationUri);

            return true;
        }
    }
}
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class Office365XrmInstancePicker
        : XrmPage
    {
        public Office365XrmInstancePicker(InteractiveBrowser browser)
            : base(browser)
        {
        }

        private static BrowserCommandOptions InstancePickerRetryOptions => new BrowserCommandOptions(
	        Constants.DefaultTraceSource,
	        "xRM Instance Picker",
	        5,
	        1000,
	        null,
            false, 
            typeof(StaleElementReferenceException));

	    public BrowserCommandResult<List<InstanceInfo>> GetInstances()
        {
            return Execute(InstancePickerRetryOptions, driver =>
            {
	            driver.Navigate().GoToUrl("https://port.crm.dynamics.com/G/Instances/InstancePicker.aspx?Redirect=True");
                driver.WaitForPageToLoad();

                var instances = driver.GetJsonArray("Mscrm.InstancePicker._grid3._gridData3.GridRows");

                var list = instances
	                .Where(i => i["EventData"]["OrganizationId"].ToObject<Guid>() != Guid.Empty)
	                .Select(i => new InstanceInfo
	                {
		                OrganizationId = i["EventData"]["OrganizationId"].ToObject<Guid>(),
		                OrganizationUri = i["EventData"]["OrganizationUrl"] != null ? new Uri(i["EventData"]["OrganizationUrl"].ToString()) : null,
		                UniqueName = i["EventData"]["OrganizationUniqueName"].ToString(),
		                FriendlyName = i["EventData"]["OrganizationFriendlyName"].ToString(),
		                EffectiveUsedStorageMB = i["EventData"]["EffectiveUsedStorageMB"].ToObject<int>(),
		                InstanceType = i["EventData"]["InstanceType"].ToString(),
		                OrganizationServiceHealth = i["EventData"]["OrganizationServiceHealth"].ToObject<int>(),
		                Purpose = i["EventData"]["Purpose"].ToString(),
		                UpdateAvailable = i["EventData"]["UpdateNowAvailable"].ToObject<bool>(),
		                Version =
			                $"{i["EventData"]["Version"]["Major"].ToString()}.{i["EventData"]["Version"]["Minor"].ToString()}.{i["EventData"]["Version"]["Build"].ToString()}.{i["EventData"]["Version"]["Revision"].ToString()}"
	                }).ToList();

                return list;
            });
        }
        public Browser OpenXrmInstance(InstanceInfo instance)
        {
            var browser = new Browser(Browser.Driver);

            instance.OpenWith(browser);

            return browser;
        }
    }
}
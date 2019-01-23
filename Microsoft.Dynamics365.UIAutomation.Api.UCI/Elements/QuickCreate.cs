// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class QuickCreate : Element
    {
        private readonly WebClient _client;

        public QuickCreate(WebClient client) : base()
        {
            _client = client;
        }

        public void TakeWindowScreenShot(string path, ScreenshotImageFormat fileFormat)
        {

            _client.TakeWindowScreenShot(path, fileFormat);
        }
        public void SetValue(string field, string value)
        {
            _client.SetValue(field, value);
        }

        public bool CheckQuickCreateFormVisible()
        {
            bool quickCreateExists = false;
            quickCreateExists = _client.CheckQuickCreateFormVisible();
            return quickCreateExists;
        }

        public void SetValue(LookupItem control, int index = 0)
        {
            _client.SetValueQuickCreate(control, index);
        }

        public void SetValue(OptionSet optionSet)
        {
            _client.SetValue(optionSet);
        }

        public void SetValue(BooleanItem optionSet)
        {
            _client.SetValue(optionSet);
        }

        public void SetValue(string field, DateTime date, string format = "MM dd yyyy")
        {
            _client.SetValue(field, date, format);
        }

        public void SetValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            _client.SetValue(option, removeExistingValues);
        }

        public void Save()
        {
            _client.SaveQuickCreate();
        }

        public bool ValidateFieldMandatory(string field)
        {
            return _client.ValidateFieldMandatory(field);
        }

        public bool ValidateFieldLocked(string field)
        {
            return _client.ValidateFieldLocked(field);
        }
    }
}
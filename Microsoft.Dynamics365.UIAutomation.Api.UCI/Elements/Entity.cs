// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class Entity : Element
    {
        private readonly WebClient _client;

        public Entity(WebClient client) : base()
        {
            _client = client;
        }

        public void OpenEntity(string entityname, Guid id)
        {
            _client.OpenEntity(entityname, id);
        }

        public void SetValue(string field, string value)
        {
            _client.SetValue(field, value);
        }

        public void SetValue(LookupItem control)
        {
            _client.SetValue(control);
        }

        public void SetValue(OptionSet optionSet)
        {
            _client.SetValue(optionSet);
        }

        public void SetValue(string field, DateTime date, string format ="MM dd yyyy")
        {
            _client.SetValue(field, date, format);
        }

        public void SetValue(MultiValueOptionSet option, bool removeExistingValues = false)
        {
            _client.SetValue(option, removeExistingValues);
        }

        public string GetValue(LookupItem control)
        {
            return _client.GetValue(control);
        }

        public string GetValue(string field)
        {
            return _client.GetValue(field);
        }

        public string GetValue(OptionSet optionSet)
        {
            return _client.GetValue(optionSet);
        }

        public void GetValue(MultiValueOptionSet option)
        {
            _client.GetValue(option);
        }

        public void Save()
        {
            _client.Save();
            _client.HandleSaveDialog();
        }
        public void Delete()
        {
            _client.Delete();
        }
        public void Assign(string userOrTeam)
        {
            _client.Assign(userOrTeam);
        }
        public void SwitchProcess(string processToSwitchTo)
        {
            _client.SwitchProcess(processToSwitchTo);
        }

        public void CloseOpportunity(bool closeAsWon)
        {
            _client.CloseOpportunity(closeAsWon);
        }

        public void OpenRecordSetNavigator(int index = 0)
        {
            _client.OpenRecordSetNavigator(index);
        }

        public void CloseRecordSetNavigator()
        {
            _client.CloseRecordSetNavigator();
        }
    }   
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;

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

        public void SetValue(LookupItem control, int index = 0)
        {
            _client.SetValue(control, index);
        }

        public void SetValue(OptionSet optionSet)
        {
            _client.SetValue(optionSet);
        }

        public void SetValue(BooleanItem optionSet)
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

        public void OpenRecordSetNavigator(int index = 0)
        {
            _client.OpenRecordSetNavigator(index);
        }

        public void CloseRecordSetNavigator()
        {
            _client.CloseRecordSetNavigator();
        }

        /// <summary>
        /// Opens any tab on the web page.
        /// </summary>
        /// <param name="tabName">The name of the tab based on the References class</param>
        public void SelectTab(string tabName, string subTabName = "")
        {
            _client.SelectTab(tabName, subTabName);
        }

        public Guid GetObjectId()
        {
            return _client.GetObjectId();
        }

        public List<GridItem> GetSubGridItems(string subgridName)
        {
            return _client.GetSubGridItems(subgridName);
        }

        public void OpenSubGridRecord(string subgridName, int index = 0)
        {
            _client.OpenSubGridRecord(subgridName, index);
        }

        public int GetSubGridItemsCount(string subgridName)
        {
            return _client.GetSubGridItemsCount(subgridName);
        }

        public void SelectLookup(LookupItem control)
        {
            _client.SelectLookup(control);
        }

        public string GetHeaderValue(LookupItem control)
        {
            return _client.GetHeaderValue(control);
        }

        public string GetHeaderValue(OptionSet control)
        {
            return _client.GetHeaderValue(control);
        }

        public string GetHeaderValue(string control)
        {
            return _client.GetHeaderValue(control);
        }

        public MultiValueOptionSet GetHeaderValue(MultiValueOptionSet control)
        {
            return _client.GetHeaderValue(control);
        }

        public void SetHeaderValue(string field, string value)
        {
            _client.SetHeaderValue(field, value);
        }

        public void SetHeaderValue(LookupItem control)
        {
            _client.SetHeaderValue(control);
        }

        public void SetHeaderValue(MultiValueOptionSet control)
        {
            _client.SetHeaderValue(control);
        }

        public void SetHeaderValue(OptionSet control)
        {
            _client.SetHeaderValue(control);
        }
    }   
}

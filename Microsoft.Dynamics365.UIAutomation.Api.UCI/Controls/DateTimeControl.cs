// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    /// <summary>
    /// Represents a DateTime field in Dynamics 365.
    /// </summary>
    public class DateTimeControl
    {
        public DateTimeControl(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public DateTime? Value { get; set; }

        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }

        private string _dateAsString;
        public string DateAsString
        {
            get => _dateAsString ?? (_dateAsString = string.IsNullOrWhiteSpace(DateFormat) ? Value?.ToShortDateString() : Value?.ToString(DateFormat));
            set => _dateAsString = value;
        }

        private string _timeAsString;
        public string TimeAsString
        {
            get => _timeAsString ?? (_timeAsString = string.IsNullOrWhiteSpace(TimeFormat) ? Value?.ToShortTimeString()?.ToUpper() : Value?.ToString(TimeFormat))?.ToUpper();
            set => _timeAsString = value;
        }
    }
}

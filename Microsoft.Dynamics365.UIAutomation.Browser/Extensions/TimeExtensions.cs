// Created by: Rodriguez Mustelier Angel (rodang)
// Modify On: 2020-02-09 14:05

using System;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public static class TimeExtensions
    {
        public static TimeSpan Seconds(this int value)
            => TimeSpan.FromSeconds(value);
    }
}
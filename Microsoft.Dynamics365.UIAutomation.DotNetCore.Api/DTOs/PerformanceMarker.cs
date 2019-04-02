// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    /// <summary>
    /// This enum is used to categorize the marker
    /// </summary>
    /// <seealso cref="dynamics.d.ts"/>
    /// <remarks>please also update dynamics.d.ts</remarks>
    public enum PerformanceMarkerType
    {
        /// <summary>
        /// Regular or undefined type of marker
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Major event marker
        /// </summary>
        MajorEvent = 1,

        /// <summary>
        /// Local store command marker
        /// </summary>
        LocalStore = 2
    }

    public class PerformanceMarker
    {
        /// <summary>
        /// the list of parameters
        /// </summary>
        public string[] Parameters = new string[0];

        /// <summary>
        /// The unique id of this marker instance
        /// </summary>
        public string Id;

        /// <summary>
        /// Contains all the linked marker ids (key = relationship name, value = id of the related marker)
        /// Direct pointers are not used here to avoid any cyclic dependencies, and memory leaks.
        /// </summary>
        public List<string> LinkedMarkers = new List<string>();

        /// <summary>
        /// This is used to index markers to store in dictionary
        /// </summary>
        private static int markersCounter = 0;

        /// <summary>
        /// The time used to calculate the a point's time stamp for output purposes
        /// </summary>
        private Int64 minTime = 0;

        /// <summary>
        /// Initializes a new instance of the PerformanceMarker class
        /// </summary>
        public PerformanceMarker()
        {
            this.Id = markersCounter.ToString();

            Interlocked.Increment(ref markersCounter);
        }

        /// <summary>
        /// Name of the marker
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Timestamp this marker was created on
        /// </summary>
        public Int64 Timestamp { get; set; }

        /// <summary>
        /// Marker type
        /// </summary>
        public PerformanceMarkerType MarkerType { get; set; }

        /// <summary>
        /// the property accesser minTime
        /// </summary>
        public Int64 ExecutionTime
        {
            get
            {
                Int64 adjustedTime = this.Timestamp - this.minTime;

                return adjustedTime;
            }
        }

        internal Int64 MinTime
        {
            set
            {
                this.minTime = value;
            }
        }
    }
}
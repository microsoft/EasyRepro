// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;

namespace Microsoft.Dynamics365.UIAutomation.Api
{
    public class AppNotification
    {
        internal AppNotification(Notfication page)
        {
            _page = page;
        }

        private Notfication _page;

        public Int32 Index { get; internal set; }
        public string Title { get; internal set; }
        public string Message { get; internal set; }
        public string DismissButtonText { get; internal set; }

        public void Dismiss()
        {
            _page.Close(this);
        }
    }
}
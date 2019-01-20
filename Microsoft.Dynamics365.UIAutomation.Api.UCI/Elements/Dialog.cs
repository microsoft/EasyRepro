// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class Dialogs : Element
    {
        private readonly WebClient _client;

        public Dialogs(WebClient client) : base()
        {
            _client = client;
        }

        /// <summary>
        /// Closes the warning dialog during login
        /// </summary>
        /// <returns></returns>
        public bool CloseWarningDialog()
        {
            return _client.CloseWarningDialog();
        }

        /// <summary>
        /// Clicks OK or Cancel on the confirmation dialog.  true = OK, false = Cancel
        /// </summary>
        /// <param name="clickConfirmButton"></param>
        /// <returns></returns>
        public bool ConfirmationDialog(bool clickConfirmButton)
        {
            return _client.ConfirmationDialog(clickConfirmButton);
        }

        /// <summary>
        /// Clicks Close As Won or Close As Loss on Opportunity Close dialog
        /// </summary>
        /// <param name="closeAsWon"></param>
        public void CloseOpportunity(bool closeAsWon)
        {
            _client.CloseOpportunity(closeAsWon);
        }

        /// <summary>
        /// Enters the values provided and closes an opportunity
        /// </summary>
        /// <param name="revenue">Value for Revenue field</param>
        /// <param name="closeDate">Value for Close Date field</param>
        /// <param name="description">Value for Description field</param>
        public void CloseOpportunity(double revenue, DateTime closeDate, string description)
        {
            _client.CloseOpportunity(revenue,closeDate,description);
        }

        /// <summary>
        /// Assigns a record to a user or team
        /// </summary>
        /// <param name="to">Enum used to assign record to user or team</param>
        /// <param name="userOrTeamName">Name of the user or team to assign to</param>
        public void Assign(Dialogs.AssignTo to, string userOrTeamName = "")
        {
            _client.AssignDialog(to, userOrTeamName);
        }

        public enum AssignTo
        {
            Me,
            User,
            Team
        }
    }
}

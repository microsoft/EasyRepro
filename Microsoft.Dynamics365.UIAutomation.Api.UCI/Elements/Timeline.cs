// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    /// <summary>
    /// Class object represents the Timeline widget in the Dynamics 365 pages.
    /// It contains methods which map to the actions invoked in the menu popout
    /// of the widget.
    /// </summary>
    public class Timeline : Element
    {
        private readonly WebClient _client;

        public Timeline(WebClient client) : base()
        {
            _client = client;
        }

        /// <summary>
        /// Adds a new Activity(Appointment) to the timeline widget.
        /// </summary>
        /// <param name="subject">Appointment Subject</param>
        /// <param name="location">Location of the Appointment</param>
        /// <param name="duration">Duration of Activity</param>
        /// <param name="description">Description</param>
        public void AddAppointment(string subject, string location, string duration, string description)
        {
            _client.OpenAndClickPopoutMenu(Reference.Timeline.Popout, Reference.Timeline.PopoutAppointment, 4000);
            _client.ThinkTime(4000);
            _client.SetValue(Elements.ElementId[Reference.Timeline.AppointmentSubject], subject, FormContextType.QuickCreate);
            _client.SetValue(Elements.ElementId[Reference.Timeline.AppointmentLocation], location, FormContextType.QuickCreate);
            _client.SetValue(Elements.ElementId[Reference.Timeline.AppointmentDescription], description, FormContextType.QuickCreate);
            _client.SetValue(Elements.ElementId[Reference.Timeline.AppointmentDuration], duration, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Clicks on the Save and Close button in the Appointment Page
        /// </summary>
        public void SaveAndCloseAppointment()
        {
            SaveAndClose(Reference.Timeline.Appointment);
        }

        /// <summary>
        /// Clicks on the Save and Close button in the Specified Activity Page
        /// </summary>
        /// <param name="activity">The name of the Activity, Valid Values are Appointment, Email, Task and PhoneCall</param>
        private void SaveAndClose(string activity)
        {
            _client.ClickButton(AppElements.Xpath[AppReference.QuickCreate.SaveAndCloseButton].Replace("[NAME]", Elements.ElementId[activity]));
        }

        /// <summary>
        /// Clicks on the Activity(Email) menu item in the timeline widget.
        /// </summary>
        public void ClickEmailMenuItem()
        {
            _client.OpenAndClickPopoutMenu(Reference.Timeline.Popout, Reference.Timeline.PopoutEmail, 4000);
        }

        /// <summary>
        /// Add a subject text to the Email page
        /// </summary>
        /// <param name="subject">Subject of the email</param>
        public void AddEmailSubject(string subject)
        {
            _client.SetValue(Elements.ElementId[Reference.Timeline.EmailSubject], subject);
        }

        /// <summary>
        /// Opens the multiselect control and adds the contacts to the To, CC or BCC line.
        /// </summary>
        /// <param name="toOptions">Object of type MultiValueOptionSet containing name of the Field and the values to be set</param>
        /// <param name="removeExistingValues">Remove any existing values in the To, CC, or BCC lines, if present</param>         
        public void AddEmailContacts(LookupItem[] toOptions, bool removeExistingValues = false)
        {
            _client.SetValue(toOptions, FormContextType.Entity, removeExistingValues);
        }

        /// <summary>
        /// Sets the durations field in the Email page
        /// </summary>
        /// <param name="duration">The duration as text</param>
        public void AddEmailDuration(string duration)
        {
            _client.SetValue(Elements.ElementId[Reference.Timeline.EmailDuration], duration);
        }

        /// <summary>
        /// Returns the contacts in the To, CC or Bcc line in Email page as a MultiValueOptionSet object.
        /// </summary>
        /// <param name="emailOptions">Object of type MultiValueOptionSet containing name of the Field</param>
        /// <returns>MultiValueOptionSet object where the values field contains all the contact names</returns>
        public MultiValueOptionSet GetEmail(MultiValueOptionSet emailOptions)
        {
            return _client.GetValue(emailOptions);
        }

        /// <summary>
        /// Opens the multiselect control and removes the contacts to the To, CC or BCC line specified in the 
        /// MultiValueOptionSet object.
        /// </summary>
        /// <param name="emailOptions">Object of type MultiValueOptionSet containing name of the Field and the values to be removed</param>
        /// <returns></returns>
        public bool RemoveEmail(MultiValueOptionSet emailOptions)
        {
            return _client.SetValue(emailOptions, FormContextType.Entity, true);
        }

        /// <summary>
        /// Clicks on the Save and Close button in the Email Page
        /// </summary>
        public void SaveAndCloseEmail()
        {
            //Emails no longer use the quick create form so must handle as special case
            _client.ClickCommand("Save & Close");
            //SaveAndClose(Reference.Timeline.Email);
        }

        /// <summary>
        /// Adds a new Activity(PhoneCall) to the timeline widget.
        /// </summary>
        /// <param name="subject">PhoneCall Subject</param>
        /// <param name="phoneNumber">Phone Number</param>
        /// <param name="description">Description</param>
        /// <param name="duration">Duration of Activity</param>
        public void AddPhoneCall(string subject, string phoneNumber, string description, string duration)
        {
            _client.OpenAndClickPopoutMenu(Reference.Timeline.Popout, Reference.Timeline.PopoutPhoneCall, 4000);
            _client.ThinkTime(4000);
            _client.SetValue(Elements.ElementId[Reference.Timeline.PhoneCallSubject], subject, FormContextType.QuickCreate);
            _client.SetValue(Elements.ElementId[Reference.Timeline.PhoneCallNumber], phoneNumber, FormContextType.QuickCreate);
            _client.SetValue(Elements.ElementId[Reference.Timeline.PhoneCallDescription], description, FormContextType.QuickCreate);
            _client.SetValue(Elements.ElementId[Reference.Timeline.PhoneCallDuration], duration, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Clicks on the Save and Close button in the PhoneCall Page
        /// </summary>
        public void SaveAndClosePhoneCall()
        {
            SaveAndClose(Reference.Timeline.PhoneCall);
        }

        /// <summary>
        /// Adds a new Activity(Task) to the timeline widget.
        /// </summary>
        /// <param name="subject">Task Subject</param>
        /// <param name="description">Task Description</param>
        /// <param name="duration">Duration of Activity</param>
        public void AddTask(string subject, string description, string duration)
        {
            _client.OpenAndClickPopoutMenu(Reference.Timeline.Popout, Reference.Timeline.PopoutTask, 4000);
            _client.ThinkTime(4000);
            _client.SetValue(Elements.ElementId[Reference.Timeline.TaskSubject], subject, FormContextType.QuickCreate);
            _client.SetValue(Elements.ElementId[Reference.Timeline.TaskDescription], description, FormContextType.QuickCreate);
            _client.SetValue(Elements.ElementId[Reference.Timeline.TaskDuration], duration, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Clicks on the Save and Close button in the Task Page
        /// </summary>
        public void SaveAndCloseTask()
        {
            SaveAndClose(Reference.Timeline.Task);
        }

        /// <summary>
        /// Adds a new Post to the timeline widget.
        /// </summary>
        /// <param name="text">The string value for the Post</param>
        public void AddPost(string text)
        {
            _client.OpenAndClickPopoutMenu(Reference.Timeline.Popout, Reference.Timeline.PopoutPost, 4000);
            _client.SetValue(Reference.Timeline.PostText, text, "textarea");
            _client.ClickButton(Elements.Xpath[Reference.Timeline.PostAdd]);
        }

        /// <summary>
        /// Adds a new Note to the timeline widget.
        /// </summary>
        /// <param name="title">The title of the Note</param>
        /// <param name="note">The string value for the Note</param>
        public void AddNote(string title, string note)
        {
            _client.OpenAndClickPopoutMenu(Reference.Timeline.Popout, Reference.Timeline.PopoutNote, 4000);
            _client.SetValue(Reference.Timeline.NoteTitle, title, "input");
            _client.SetValue(Reference.Timeline.NoteText, note, "iframe");
            _client.ClickButton(Elements.Xpath[Reference.Timeline.NoteAdd]);
        }
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    /// <summary>
    /// Class object represents the Timeline widget in the Dynamics 365 pages.
    /// It contains methods which map to the actions invoked in the menu popout
    /// of the widget.
    /// </summary>
    public class Timeline : Element
    {

        #region DTO
        public static class TimelineReference
        {
            public static string SaveAndClose = "//button[contains(@data-id,\"[NAME].SaveAndClose\")]";

            public static string Popout = "//button[contains(@id,\"notescontrol-action_bar_add_command\")]";
            public static string PopoutAppointment = "//li[contains(@id,\"notescontrol-createNewRecord_flyoutMenuItem_appointment\")]";
            public static string PopoutEmail = "//li[contains(@id,\"notescontrol-createNewRecord_flyoutMenuItem_email\")]";
            public static string PopoutPhoneCall = "//li[contains(@id,\"notescontrol-createNewRecord_flyoutMenuItem_phonecall\")]";
            public static string PopoutTask = "//li[contains(@id,\"notescontrol-createNewRecord_flyoutMenuItem_task\")]";
            public static string PopoutNote = "//li[contains(@id,\"notescontrol-createNewRecord_flyoutMenuItem_notes\")]";
            public static string PopoutPost = "//li[contains(@id,\"notescontrol-createNewRecord_flyoutMenuItem_post\")]";

            public static string PostText = "id(\"create_post_postText\")";
            public static string PostAdd = "//button[@data-id=\"notescontrol-author_post_testsave_button\"]";
            public static string PostCancel = "id(\"create_post_cancel_btn\")";

            public static string NoteTitle = "id(\"create_note_medium_title\")";
            public static string NoteText = "//iframe[contains(@class, \"fullPageContentEditorFrame\")]";
            public static string NoteTextBody = "//body[contains(@class, 'cke_wysiwyg_frame')]";
            public static string NoteAdd = "//button[contains(@id,'save_button') or contains(@id, 'create_note_add_btn')]";
            public static string NoteCancel = "id(\"create_note_cancel_btn\")";

            public static string TaskSubject = "subject";
            public static string TaskDescription = "description";
            public static string TaskDuration = "actualdurationminutes";
            public static string Task = "task";

            public static string PhoneCallSubject = "subject";
            public static string PhoneCallNumber = "phonenumber";
            public static string PhoneCallDescription = "description";
            public static string PhoneCallDuration = "actualdurationminutes";
            public static string PhoneCall = "phonecall";

            public static string EmailSubject = "subject";
            public static string EmailTo = "to";
            public static string EmailCC = "cc";
            public static string EmailBcc = "bcc";
            public static string EmailDescription = "description";
            public static string EmailDuration = "actualdurationminutes";
            public static string Email = "email";

            public static string AppointmentSubject = "subject";
            public static string AppointmentLocation = "location";
            public static string AppointmentDescription = "description";
            public static string AppointmentDuration = "scheduleddurationminutes";
            public static string Appointment = "appointment";
        }

        #endregion
        private readonly WebClient _client;

        public Timeline(WebClient client) : base()
        {
            _client = client;
        }
        #region public
        /// <summary>
        /// Adds a new Activity(Appointment) to the timeline widget.
        /// </summary>
        /// <param name="subject">Appointment Subject</param>
        /// <param name="location">Location of the Appointment</param>
        /// <param name="duration">Duration of Activity</param>
        /// <param name="description">Description</param>
        public void AddAppointment(string subject, string location, string duration, string description)
        {
            this.OpenAndClickPopoutMenu(TimelineReference.Popout, TimelineReference.PopoutAppointment, 4000);
            _client.ThinkTime(4000);
            Field.SetValue(_client, TimelineReference.AppointmentSubject, subject, FormContextType.QuickCreate);
            Field.SetValue(_client,TimelineReference.AppointmentLocation, location, FormContextType.QuickCreate);
            Field.SetValue(_client,TimelineReference.AppointmentDescription, description, FormContextType.QuickCreate);
            Field.SetValue(_client, TimelineReference.AppointmentDuration, duration, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Clicks on the Save and Close button in the Appointment Page
        /// </summary>
        public void SaveAndCloseAppointment()
        {
            SaveAndClose(TimelineReference.Appointment);
        }

        /// <summary>
        /// Clicks on the Save and Close button in the Specified Activity Page
        /// </summary>
        /// <param name="activity">The name of the Activity, Valid Values are Appointment, Email, Task and PhoneCall</param>
        private void SaveAndClose(string activity)
        {
            this.ClickButton(QuickCreate.QuickCreateReference.SaveAndCloseButton.Replace("[NAME]", activity));
        }

        /// <summary>
        /// Clicks on the Activity(Email) menu item in the timeline widget.
        /// </summary>
        public void ClickEmailMenuItem()
        {
            this.OpenAndClickPopoutMenu(TimelineReference.Popout, TimelineReference.PopoutEmail, 4000);
        }

        /// <summary>
        /// Add a subject text to the Email page
        /// </summary>
        /// <param name="subject">Subject of the email</param>
        public void AddEmailSubject(string subject)
        {
            Field.SetValue(_client,TimelineReference.EmailSubject, subject);
        }

        /// <summary>
        /// Opens the multiselect control and adds the contacts to the To, CC or BCC line.
        /// </summary>
        /// <param name="toOptions">Object of type MultiValueOptionSet containing name of the Field and the values to be set</param>
        /// <param name="removeExistingValues">Remove any existing values in the To, CC, or BCC lines, if present</param>         
        public void AddEmailContacts(LookupItem[] toOptions, bool removeExistingValues = false)
        {
            Lookup lookup = new Lookup(_client);
            lookup.SetValue(toOptions, FormContextType.Entity, removeExistingValues);
        }

        /// <summary>
        /// Sets the durations field in the Email page
        /// </summary>
        /// <param name="duration">The duration as text</param>
        public void AddEmailDuration(string duration)
        {
            Field.SetValue(_client, TimelineReference.EmailDuration, duration);
        }

        /// <summary>
        /// Returns the contacts in the To, CC or Bcc line in Email page as a MultiValueOptionSet object.
        /// </summary>
        /// <param name="emailOptions">Object of type MultiValueOptionSet containing name of the Field</param>
        /// <returns>MultiValueOptionSet object where the values field contains all the contact names</returns>
        public MultiValueOptionSet GetEmail(MultiValueOptionSet emailOptions)
        {
            Entity entity = new Entity(_client);
            return entity.GetValue(emailOptions);
        }

        /// <summary>
        /// Opens the multiselect control and removes the contacts to the To, CC or BCC line specified in the 
        /// MultiValueOptionSet object.
        /// </summary>
        /// <param name="emailOptions">Object of type MultiValueOptionSet containing name of the Field and the values to be removed</param>
        /// <returns></returns>
        public bool RemoveEmail(MultiValueOptionSet emailOptions)
        {
            return emailOptions.ClearValue(_client, emailOptions, FormContextType.Entity);
            //return _client.SetValue(emailOptions, FormContextType.Entity, true);
        }

        /// <summary>
        /// Clicks on the Save and Close button in the Email Page
        /// </summary>
        public void SaveAndCloseEmail()
        {
            //Emails no longer use the quick create form so must handle as special case
            Dialogs dialogs = new Dialogs(_client);
            dialogs.ClickCommand("Save & Close");
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
            this.OpenAndClickPopoutMenu(TimelineReference.Popout, TimelineReference.PopoutPhoneCall, 4000);
            _client.ThinkTime(4000);
            Field.SetValue(_client,TimelineReference.PhoneCallSubject, subject, FormContextType.QuickCreate);
            Field.SetValue(_client,TimelineReference.PhoneCallNumber, phoneNumber, FormContextType.QuickCreate);
            Field.SetValue(_client,TimelineReference.PhoneCallDescription, description, FormContextType.QuickCreate);
            Field.SetValue(_client,TimelineReference.PhoneCallDuration, duration, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Clicks on the Save and Close button in the PhoneCall Page
        /// </summary>
        public void SaveAndClosePhoneCall()
        {
            SaveAndClose(TimelineReference.PhoneCall);
        }

        /// <summary>
        /// Adds a new Activity(Task) to the timeline widget.
        /// </summary>
        /// <param name="subject">Task Subject</param>
        /// <param name="description">Task Description</param>
        /// <param name="duration">Duration of Activity</param>
        public void AddTask(string subject, string description, string duration)
        {
            this.OpenAndClickPopoutMenu(TimelineReference.Popout, TimelineReference.PopoutTask, 4000);
            _client.ThinkTime(4000);
            Field.SetValue(_client,TimelineReference.TaskSubject, subject, FormContextType.QuickCreate);
            Field.SetValue(_client,TimelineReference.TaskDescription, description, FormContextType.QuickCreate);
            Field.SetValue(_client,TimelineReference.TaskDuration, duration, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Clicks on the Save and Close button in the Task Page
        /// </summary>
        public void SaveAndCloseTask()
        {
            SaveAndClose(TimelineReference.Task);
        }

        /// <summary>
        /// Adds a new Post to the timeline widget.
        /// </summary>
        /// <param name="text">The string value for the Post</param>
        public void AddPost(string text)
        {
            this.OpenAndClickPopoutMenu(TimelineReference.Popout, TimelineReference.PopoutPost, 4000);
            this.SetValue(TimelineReference.PostText, text, "textarea");
            this.ClickButton(TimelineReference.PostAdd);
        }

        /// <summary>
        /// Adds a new Note to the timeline widget.
        /// </summary>
        /// <param name="title">The title of the Note</param>
        /// <param name="note">The string value for the Note</param>
        public void AddNote(string title, string note)
        {
            this.OpenAndClickPopoutMenu(TimelineReference.Popout, TimelineReference.PopoutNote, 4000);
            this.SetValue(TimelineReference.NoteTitle, title, "input");
            this.SetValue(TimelineReference.NoteText, note, "iframe");
            this.ClickButton(TimelineReference.NoteAdd);
        }

        #endregion

        #region Timeline

        /// <summary>
        /// This method opens the popout menus in the Dynamics 365 pages. 
        /// This method uses a thinktime since after the page loads, it takes some time for the 
        /// widgets to load before the method can find and popout the menu.
        /// </summary>
        /// <param name="popoutName">The By Object of the Popout menu</param>
        /// <param name="popoutItemName">The By Object of the Popout Item name in the popout menu</param>
        /// <param name="thinkTime">Amount of time(milliseconds) to wait before this method will click on the "+" popout menu.</param>
        /// <returns>True on success, False on failure to invoke any action</returns>
        internal BrowserCommandResult<bool> OpenAndClickPopoutMenu(By menuName, By menuItemName, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            return _client.Execute($"Open menu", driver =>
            {
                driver.ClickWhenAvailable(menuName);
                try
                {
                    driver.ClickWhenAvailable(menuItemName);
                }
                catch
                {
                    // Element is stale reference is thrown here since the HTML components 
                    // get destroyed and thus leaving the references null. 
                    // It is expected that the components will be destroyed and the next 
                    // action should take place after it and hence it is ignored.
                    return false;
                }

                return true;
            });
        }

        internal BrowserCommandResult<bool> CloseActivity(bool closeOrCancel, int thinkTime = Constants.DefaultThinkTime)
        {
            _client.ThinkTime(thinkTime);

            var xPathQuery = closeOrCancel
                ? Dialogs.DialogsReference.CloseActivity.Close
                : Dialogs.DialogsReference.CloseActivity.Cancel;

            var action = closeOrCancel ? "Close" : "Cancel";

            return _client.Execute(_client.GetOptions($"{action} Activity"), driver =>
            {
                var dialog = driver.WaitUntilAvailable(By.XPath(Dialogs.DialogsReference.DialogContext));

                var actionButton = dialog.FindElement(By.XPath(xPathQuery));

                actionButton?.Click();

                driver.WaitForTransaction();

                return true;
            });
        }

        /// <summary>
        /// This method opens the popout menus in the Dynamics 365 pages. 
        /// This method uses a thinktime since after the page loads, it takes some time for the 
        /// widgets to load before the method can find and popout the menu.
        /// </summary>
        /// <param name="popoutName">The name of the Popout menu</param>
        /// <param name="popoutItemName">The name of the Popout Item name in the popout menu</param>
        /// <param name="thinkTime">Amount of time(milliseconds) to wait before this method will click on the "+" popout menu.</param>
        /// <returns>True on success, False on failure to invoke any action</returns>
        internal BrowserCommandResult<bool> OpenAndClickPopoutMenu(string popoutName, string popoutItemName, int thinkTime = Constants.DefaultThinkTime)
        {
            return this.OpenAndClickPopoutMenu(By.XPath(popoutName), By.XPath(popoutItemName), thinkTime);
        }

        /// <summary>
        /// Provided a By object which represents a HTML Button object, this method will
        /// find it and click it.
        /// </summary>
        /// <param name="by">The object of Type By which represents a HTML Button object</param>
        /// <returns>True on success, False/Exception on failure to invoke any action</returns>
        internal BrowserCommandResult<bool> ClickButton(By by)
        {
            return _client.Execute($"Open Timeline Add Post Popout", driver =>
            {
                var button = driver.WaitUntilAvailable(by);
                if (button.TagName.Equals("button"))
                {
                    try
                    {
                        driver.ClickWhenAvailable(by);
                    }
                    catch
                    {
                        // Element is stale reference is thrown here since the HTML components 
                        // get destroyed and thus leaving the references null. 
                        // It is expected that the components will be destroyed and the next 
                        // action should take place after it and hence it is ignored.
                    }

                    return true;
                }
                else if (button.FindElements(By.TagName("button")).Any())
                {
                    button.FindElements(By.TagName("button")).First().Click();
                    return true;
                }
                else
                {
                    throw new InvalidOperationException($"Control does not exist");
                }
            });
        }

        /// <summary>
        /// Provided a fieldname as a XPath which represents a HTML Button object, this method will
        /// find it and click it.
        /// </summary>
        /// <param name="fieldNameXpath">The field as a XPath which represents a HTML Button object</param>
        /// <returns>True on success, Exception on failure to invoke any action</returns>
        internal BrowserCommandResult<bool> ClickButton(string fieldNameXpath)
        {
            try
            {
                return ClickButton(By.XPath(fieldNameXpath));
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Field: {fieldNameXpath} with Does not exist", e);
            }
        }



        /// <summary>
        /// A generic setter method which will find the HTML Textbox/Textarea object and populate
        /// it with the value provided. The expected tag name is to make sure that it hits
        /// the expected tag and not some other object with the similar fieldname.
        /// </summary>
        /// <param name="fieldName">The name of the field representing the HTML Textbox/Textarea object</param>
        /// <param name="value">The string value which will be populated in the HTML Textbox/Textarea</param>
        /// <param name="expectedTagName">Expected values - textbox/textarea</param>
        /// <returns>True on success, Exception on failure to invoke any action</returns>
        internal BrowserCommandResult<bool> SetValue(string fieldName, string value, string expectedTagName)
        {
            return _client.Execute($"SetValue (Generic)", driver =>
            {
                var inputbox = driver.WaitUntilAvailable(By.XPath(fieldName));
                if (expectedTagName.Equals(inputbox.TagName, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!inputbox.TagName.Contains("iframe", StringComparison.InvariantCultureIgnoreCase))
                    {
                        inputbox.Click(true);
                        inputbox.Clear();
                        inputbox.SendKeys(value);
                    }
                    else
                    {
                        driver.SwitchTo().Frame(inputbox);

                        driver.WaitUntilAvailable(By.TagName("iframe"));
                        driver.SwitchTo().Frame(0);

                        var inputBoxBody = driver.WaitUntilAvailable(By.TagName("body"));
                        inputBoxBody.Click(true);
                        inputBoxBody.SendKeys(value);

                        driver.SwitchTo().DefaultContent();
                    }

                    return true;
                }

                throw new InvalidOperationException($"Field: {fieldName} with tagname {expectedTagName} Does not exist");
            });
        }

        #endregion
    }
}

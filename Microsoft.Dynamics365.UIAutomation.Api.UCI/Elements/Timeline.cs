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
        #region DTO
        public class TimelineReference
        {
            public const string Timeline = "Timeline";
            #region private
            private string _SaveAndClose = "//button[contains(@data-id,\"[NAME].SaveAndClose\")]";

            private string _Popout = "//button[contains(@id,\"notescontrol-action_bar_add_command\")]";
            private string _PopoutAppointment = "//li[contains(@id,\"notescontrol-createNewRecord_flyoutMenuItem_appointment\")]";
            private string _PopoutEmail = "//li[contains(@id,\"notescontrol-createNewRecord_flyoutMenuItem_email\")]";
            private string _PopoutPhoneCall = "//li[contains(@id,\"notescontrol-createNewRecord_flyoutMenuItem_phonecall\")]";
            private string _PopoutTask = "//li[contains(@id,\"notescontrol-createNewRecord_flyoutMenuItem_task\")]";
            private string _PopoutNote = "//li[contains(@id,\"notescontrol-createNewRecord_flyoutMenuItem_notes\")]";
            private string _PopoutPost = "//li[contains(@id,\"notescontrol-createNewRecord_flyoutMenuItem_post\")]";

            private string _PostText = "id(\"create_post_postText\")";
            private string _PostAdd = "//button[@data-id=\"notescontrol-author_post_testsave_button\"]";
            private string _PostCancel = "id(\"create_post_cancel_btn\")";

            private string _NoteTitle = "id(\"create_note_medium_title\")";
            private string _NoteText = "//iframe[contains(@class, \"fullPageContentEditorFrame\")]";
            private string _NoteTextBody = "//body[contains(@class, 'cke_wysiwyg_frame')]";
            private string _NoteAdd = "//button[contains(@id,'save_button') or contains(@id, 'create_note_add_btn')]";
            private string _NoteCancel = "id(\"create_note_cancel_btn\")";

            private string _TaskSubject = "subject";
            private string _TaskDescription = "description";
            private string _TaskDuration = "actualdurationminutes";
            private string _Task = "task";

            private string _PhoneCallSubject = "subject";
            private string _PhoneCallNumber = "phonenumber";
            private string _PhoneCallDescription = "description";
            private string _PhoneCallDuration = "actualdurationminutes";
            private string _PhoneCall = "phonecall";

            private string _EmailSubject = "subject";
            private string _EmailTo = "to";
            private string _EmailCC = "cc";
            private string _EmailBcc = "bcc";
            private string _EmailDescription = "description";
            private string _EmailDuration = "actualdurationminutes";
            private string _Email = "email";

            private string _AppointmentSubject = "subject";
            private string _AppointmentLocation = "location";
            private string _AppointmentDescription = "description";
            private string _AppointmentDuration = "scheduleddurationminutes";
            private string _Appointment = "appointment";
            #endregion
            #region prop
            public string SaveAndClose { get => _SaveAndClose; set { _SaveAndClose = value; } }

            public string Popout { get => _Popout; set { _Popout = value; } }
            public string PopoutAppointment { get => _PopoutAppointment; set { _PopoutAppointment = value; } }
            public string PopoutEmail { get => _PopoutEmail; set { _PopoutEmail = value; } }
            public string PopoutPhoneCall { get => _PopoutPhoneCall; set { _PopoutPhoneCall = value; } }
            public string PopoutTask { get => _PopoutTask; set { _PopoutTask = value; } }
            public string PopoutNote { get => _PopoutNote; set { _PopoutNote = value; } }
            public string PopoutPost { get => _PopoutPost; set { _PopoutPost = value; } }

            public string PostText { get => _PostText; set { _PostText = value; } }
            public string PostAdd { get => _PostAdd; set { _PostAdd = value; } }
            public string PostCancel { get => _PostCancel; set { _PostCancel = value; } }

            public string NoteTitle { get => _NoteTitle; set { _NoteTitle = value; } }
            public string NoteText { get => _NoteText; set { _NoteText = value; } }
            public string NoteTextBody { get => _NoteTextBody; set { _NoteTextBody = value; } }
            public string NoteAdd { get => _NoteAdd; set { _NoteAdd = value; } }
            public string NoteCancel { get => _NoteCancel; set { _NoteCancel = value; } }

            public string TaskSubject { get => _TaskSubject; set { _TaskSubject = value; } }
            public string TaskDescription { get => _TaskDescription; set { _TaskDescription = value; } }
            public string TaskDuration { get => _TaskDuration; set { _TaskDuration = value; } }
            public string Task { get => _Task; set { _Task = value; } }

            public string PhoneCallSubject { get => _PhoneCallSubject; set { _PhoneCallSubject = value; } }
            public string PhoneCallNumber { get => _PhoneCallNumber; set { _PhoneCallNumber = value; } }
            public string PhoneCallDescription { get => _PhoneCallDescription; set { _PhoneCallDescription = value; } }
            public string PhoneCallDuration { get => _PhoneCallDuration; set { _PhoneCallDuration = value; } }
            public string PhoneCall { get => _PhoneCall; set { _PhoneCall = value; } }

            public string EmailSubject { get => _EmailSubject; set { _EmailSubject = value; } }
            public string EmailTo { get => _EmailTo; set { _EmailTo = value; } }
            public string EmailCC { get => _EmailCC; set { _EmailCC = value; } }
            public string EmailBcc { get => _EmailBcc; set { _EmailBcc = value; } }
            public string EmailDescription { get => _EmailDescription; set { _EmailDescription = value; } }
            public string EmailDuration { get => _EmailDuration; set { _EmailDuration = value; } }
            public string Email { get => _Email; set { _Email = value; } }

            public string AppointmentSubject { get => _AppointmentSubject; set { _AppointmentSubject = value; } }
            public string AppointmentLocation { get => _AppointmentLocation; set { _AppointmentLocation = value; } }
            public string AppointmentDescription { get => _AppointmentDescription; set { _AppointmentDescription = value; } }
            public string AppointmentDuration { get => _AppointmentDuration; set { _AppointmentDuration = value; } }
            public string Appointment { get => _Appointment; set { _Appointment = value; } }
            #endregion
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
            this.OpenAndClickPopoutMenu(_client.ElementMapper.TimelineReference.Popout, _client.ElementMapper.TimelineReference.PopoutAppointment, 4000);
            _client.ThinkTime(4000);
            Field objField = new Field(_client);
            objField.SetValue(_client, _client.ElementMapper.TimelineReference.AppointmentSubject, subject, FormContextType.QuickCreate);
            objField.SetValue(_client, _client.ElementMapper.TimelineReference.AppointmentLocation, location, FormContextType.QuickCreate);
            objField.SetValue(_client, _client.ElementMapper.TimelineReference.AppointmentDescription, description, FormContextType.QuickCreate);
            objField.SetValue(_client, _client.ElementMapper.TimelineReference.AppointmentDuration, duration, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Clicks on the Save and Close button in the Appointment Page
        /// </summary>
        public void SaveAndCloseAppointment()
        {
            SaveAndClose(_client.ElementMapper.TimelineReference.Appointment);
        }

        /// <summary>
        /// Clicks on the Save and Close button in the Specified Activity Page
        /// </summary>
        /// <param name="activity">The name of the Activity, Valid Values are Appointment, Email, Task and PhoneCall</param>
        private void SaveAndClose(string activity)
        {
            this.ClickButton(_client.ElementMapper.QuickCreateReference.SaveAndCloseButton.Replace("[NAME]", activity));
        }

        /// <summary>
        /// Clicks on the Activity(Email) menu item in the timeline widget.
        /// </summary>
        public void ClickEmailMenuItem()
        {
            this.OpenAndClickPopoutMenu(_client.ElementMapper.TimelineReference.Popout, _client.ElementMapper.TimelineReference.PopoutEmail, 4000);
        }

        /// <summary>
        /// Add a subject text to the Email page
        /// </summary>
        /// <param name="subject">Subject of the email</param>
        public void AddEmailSubject(string subject)
        {
            Field objField = new Field(_client);
            objField.SetValue(_client,_client.ElementMapper.TimelineReference.EmailSubject, subject);
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
            Field objField = new Field(_client);
            objField.SetValue(_client, _client.ElementMapper.TimelineReference.EmailDuration, duration);
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
            this.OpenAndClickPopoutMenu(_client.ElementMapper.TimelineReference.Popout, _client.ElementMapper.TimelineReference.PopoutPhoneCall, 4000);
            _client.ThinkTime(4000);
            Field objField = new Field(_client);
            objField.SetValue(_client, _client.ElementMapper.TimelineReference.PhoneCallSubject, subject, FormContextType.QuickCreate);
            objField.SetValue(_client, _client.ElementMapper.TimelineReference.PhoneCallNumber, phoneNumber, FormContextType.QuickCreate);
            objField.SetValue(_client, _client.ElementMapper.TimelineReference.PhoneCallDescription, description, FormContextType.QuickCreate);
            objField.SetValue(_client, _client.ElementMapper.TimelineReference.PhoneCallDuration, duration, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Clicks on the Save and Close button in the PhoneCall Page
        /// </summary>
        public void SaveAndClosePhoneCall()
        {
            SaveAndClose(_client.ElementMapper.TimelineReference.PhoneCall);
        }

        /// <summary>
        /// Adds a new Activity(Task) to the timeline widget.
        /// </summary>
        /// <param name="subject">Task Subject</param>
        /// <param name="description">Task Description</param>
        /// <param name="duration">Duration of Activity</param>
        public void AddTask(string subject, string description, string duration)
        {
            this.OpenAndClickPopoutMenu(_client.ElementMapper.TimelineReference.Popout, _client.ElementMapper.TimelineReference.PopoutTask, 4000);
            _client.ThinkTime(4000);
            Field objField = new Field(_client);
            objField.SetValue(_client, _client.ElementMapper.TimelineReference.TaskSubject, subject, FormContextType.QuickCreate);
            objField.SetValue(_client, _client.ElementMapper.TimelineReference.TaskDescription, description, FormContextType.QuickCreate);
            objField.SetValue(_client, _client.ElementMapper.TimelineReference.TaskDuration, duration, FormContextType.QuickCreate);
        }

        /// <summary>
        /// Clicks on the Save and Close button in the Task Page
        /// </summary>
        public void SaveAndCloseTask()
        {
            SaveAndClose(_client.ElementMapper.TimelineReference.Task);
        }

        /// <summary>
        /// Adds a new Post to the timeline widget.
        /// </summary>
        /// <param name="text">The string value for the Post</param>
        public void AddPost(string text)
        {
            this.OpenAndClickPopoutMenu(_client.ElementMapper.TimelineReference.Popout, _client.ElementMapper.TimelineReference.PopoutPost, 4000);
            this.SetValue(_client.ElementMapper.TimelineReference.PostText, text, "textarea");
            this.ClickButton(_client.ElementMapper.TimelineReference.PostAdd);
        }

        /// <summary>
        /// Adds a new Note to the timeline widget.
        /// </summary>
        /// <param name="title">The title of the Note</param>
        /// <param name="note">The string value for the Note</param>
        public void AddNote(string title, string note)
        {
            this.OpenAndClickPopoutMenu(_client.ElementMapper.TimelineReference.Popout, _client.ElementMapper.TimelineReference.PopoutNote, 4000);
            this.SetValue(_client.ElementMapper.TimelineReference.NoteTitle, title, "input");
            this.SetValue(_client.ElementMapper.TimelineReference.NoteText, note, "iframe");
            this.ClickButton(_client.ElementMapper.TimelineReference.NoteAdd);
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
        internal BrowserCommandResult<bool> OpenAndClickPopoutMenu(string menuName, string menuItemName, int thinkTime = Constants.DefaultThinkTime)
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
                ? _client.ElementMapper.DialogsReference.CloseActivity.Close
                : _client.ElementMapper.DialogsReference.CloseActivity.Cancel;

            var action = closeOrCancel ? "Close" : "Cancel";

            return _client.Execute(_client.GetOptions($"{action} Activity"), driver =>
            {
                var dialog = driver.WaitUntilAvailable(_client.ElementMapper.DialogsReference.DialogContext);

                var actionButton = driver.FindElement(dialog.Locator + xPathQuery);

                actionButton?.Click(_client);

                driver.Wait();

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
        //internal BrowserCommandResult<bool> OpenAndClickPopoutMenu(string popoutName, string popoutItemName, int thinkTime = Constants.DefaultThinkTime)
        //{
        //    return this.OpenAndClickPopoutMenu(popoutName, popoutItemName, thinkTime);
        //}

        /// <summary>
        /// Provided a By object which represents a HTML Button object, this method will
        /// find it and click it.
        /// </summary>
        /// <param name="by">The object of Type By which represents a HTML Button object</param>
        /// <returns>True on success, False/Exception on failure to invoke any action</returns>
        internal BrowserCommandResult<bool> ClickButton(string by)
        {
            return _client.Execute($"Open Timeline Add Post Popout", driver =>
            {
                var button = driver.WaitUntilAvailable(by);
                if (button.Tag.Equals("button"))
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
                else if (driver.FindElements(button.Locator + "//button").Any())
                {
                    driver.FindElements(button.Locator + "//button").First().Click(_client);
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
        //internal BrowserCommandResult<bool> ClickButton(string fieldNameXpath)
        //{
        //    try
        //    {
        //        return ClickButton(fieldNameXpath);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new InvalidOperationException($"Field: {fieldNameXpath} with Does not exist", e);
        //    }
        //}



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
                var inputbox = driver.WaitUntilAvailable(fieldName);
                if (expectedTagName.Equals(inputbox.Tag, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!inputbox.Tag.Contains("iframe", StringComparison.InvariantCultureIgnoreCase))
                    {
                        inputbox.Click(_client);
                        inputbox.Clear(_client, inputbox.Locator);
                        inputbox.SetValue(_client, value);
                    }
                    else
                    {
                        driver.SwitchToFrame(inputbox.Locator);

                        driver.WaitUntilAvailable("//iframe");
                        driver.SwitchToFrame("0");

                        var inputBoxBody = driver.WaitUntilAvailable("//body");
                        inputBoxBody.Click(_client);
                        inputBoxBody.SetValue(_client, value);

                        driver.SwitchToFrame("0");
                    }

                    return true;
                }

                throw new InvalidOperationException($"Field: {fieldName} with tagname {expectedTagName} Does not exist");
            });
        }

        #endregion
    }
}

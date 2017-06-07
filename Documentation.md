# Microsoft.Dynamics365.UIAutomation.Api

### Dynamics365.UIAutomation.Api.LoginPage
#### Login(System.Uri,System.Security.SecureString,System.Security.SecureString)
##### Summary 
Login Page
##### Parameters
**uri:** The Uri
**username:** The Username to login to CRM application
**password:** The Password to login to CRM application
##### Example
xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);

#### Login(System.Uri,System.Security.SecureString,System.Security.SecureString,System.Action{Microsoft.Dynamics365.UIAutomation.Api.LoginRedirectEventArgs})
##### Summary 
Login Page
##### Parameters
**uri:** The Uri
**username:** The Username to login to CRM application
**password:** The Password to login to CRM application
**redirectAction:** The RedirectAction
##### Example
xrmBrowser.LoginPage.Login(_xrmUri, _username, _password, ADFSLogin);

### Dynamics365.UIAutomation.Api.XrmDashboardPage
The Dashboard page.
#### SelectDashBoard(System.String,System.Int32)
##### Summary 
Switches between the DashBoard.
##### Parameters
**dashBoardName:** The name of the DashBoard you want to select
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Dashboard.SelectDashBoard("Sales Performance Dashboard");

### Dynamics365.UIAutomation.Api.XrmGlobalSearchPage
#### FilterWith(System.String,System.Int32)
##### Summary 
Filter by entity in the Global Search Results.
##### Parameters
**entity:** The entity you want to filter with.
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds
##### Example
xrmBrowser.GlobalSearch.FilterWith("Account");

#### Search(System.String,System.Int32)
##### Summary 
Searches for the specified criteria in Global Search.
##### Parameters
**criteria:** Search criteria.
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.GlobalSearch.Search("Contoso");

#### OpenRecord(System.String,System.Int32,System.Int32)
##### Summary 
Opens the specified record in the Global Search Results.
##### Parameters
**entity:** The entity you want to open a record.
**index:** The index of the record you want to open.
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.GlobalSearch.OpenRecord("Accounts",0) ;

### Dynamics365.UIAutomation.Api.XrmCommandBarPage
Command bar page.
#### ClickCommand(System.String,System.String,System.Boolean,System.Int32)
##### Summary 
Clicks the Command
##### Parameters
**name:** The Name of the command
**subName:** The SubName
**moreCommands:** The MoreCommands
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.CommandBar.ClickCommand("Close as Won");

### Dynamics365.UIAutomation.Api.XrmDialogPage
Dialog page.
#### CloseOpportunity(System.Double,System.DateTime,System.String,System.Int32)
##### Summary 
Closes the opportunity you are currently working on.
##### Parameters
**revenue:** The revenue you want to assign to the opportunity.
**closeDate:** The close date for the opportunity.
**description:** The description of the closing.
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Dialogs.CloseOpportunity(10000, DateTime.Now, "Testing the Close Opportunity API");

#### Assign(Microsoft.Dynamics365.UIAutomation.Api.XrmDialogPage.AssignTo,System.String,System.Int32)
##### Summary 
Assigns the record to a User or Team
##### Parameters
**to:** The User or Team you want to assign the record to
**value:** The value of the User or Team you want to find and select
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Dialogs.Assign(XrmDialogPage.AssignTo.Me, "");

#### Delete(System.Int32)
##### Summary 
Deletes the selected record.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Dialogs.Delete();

#### DuplicateDetection(System.Boolean,System.Int32)
##### Summary 
Checks for Duplicate Detection Dialog. If duplicate detection is enable then you can confirm the save or cancel.
##### Parameters
**save:** If set to true Save the record otherwise it will cancel.
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Dialogs.DuplicateDetection(true);

#### RunWorkflow(System.String,System.Int32)
##### Summary 
Run Work flow
##### Parameters
**name:** The name
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Dialogs.RunWorkflow("Account Set Phone Number");
 
### Dynamics365.UIAutomation.Api.XrmEntityPage
Xrm Entity page.
#### OpenEntity(System.String,System.Guid,System.Int32)
##### Summary 
Opens the Entity
##### Parameters
**entityName:** The Entity Name you want to open
**id:** The Guid
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Entity.OpenEntity(TestSettings.AccountLogicalName, Guid.Parse(TestSettings.AccountId));

#### OpenEntity(System.Uri,System.Int32)
##### Summary 
Opens the Entity
##### Parameters
**uri:** The uri
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Entity.OpenEntity(item.Url);

#### NavigateDown(System.Int32)
##### Summary 
Navigate Down the record
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Entity.NavigateDown();

#### NavigateUp(System.Int32)
##### Summary 
Navigate Up the record
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Entity.NavigateUp();

#### SelectForm(System.String,System.Int32)
##### Summary 
Selects the Form
##### Parameters
**name:** 
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Entity.SelectForm("Details");

#### SelectTab(System.String,System.Int32)
##### Summary 
Selects the tab and clicks. If the tab is expanded it will collapse it. If the tab is collapsed it will expand it.
##### Parameters
**name:** The name of the tab.
##### Example
xrmBrowser.Entity.SelectTab("Summary");

#### CollapseTab(System.String,System.Int32)
##### Summary 
Collapses the Tab on a CRM Entity form.
##### Parameters
**name:** The name of the Tab.
##### Example
xrmBrowser.Entity.CollapseTab("Summary");

#### ExpandTab(System.String,System.Int32)
##### Summary 
Expands the Tab on a CRM Entity form.
##### Parameters
**name:** The name of the Tab.
##### Example
xrmBrowser.Entity.ExpandTab("Summary");

#### SelectLookup(System.String,System.Int32)
##### Summary 
Set Lookup Value for the field
##### Parameters
**field:** The Field
**index:** The Index
##### Example
xrmBrowser.Entity.SelectLookup("customerid", 0);

#### SelectLookup(System.String,System.String)
##### Summary 
Set Lookup Value for the field
##### Parameters
**field:** The Field
**value:** The Value
##### Example
xrmBrowser.Entity.SelectLookup(TestSettings.LookupField, TestSettings.LookupName);

#### SelectLookup(System.String,System.Boolean)
##### Summary 
Set Lookup Value for the field
##### Parameters
**field:** The Field
**openLookupPage:** 

#### Popout(System.Int32)
##### Summary 
Popout the form
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Entity.Popout();

#### ClickSubgridAddButton(System.String,System.Int32)
##### Summary 
Click add button of subgridName
##### Parameters
**subgridName:** The SubgridName
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Entity.ClickSubgridAddButton("Stakeholders");

#### ClickSubgridGridViewButton(System.String,System.Int32)
##### Summary 
Click GridView button of subgridName
##### Parameters
**subgridName:** The subgridName
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.

#### SelectSubgridLookup(System.String,System.String,System.Int32)
##### Summary 
Set Lookup Value for Subgrid subgridName
##### Parameters
**subgridName:** The SubgridName
**value:** 
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Entity.SelectSubgridLookup("Stakeholders", "Alex Wu");

#### SelectSubgridLookup(System.String,System.Int32)
##### Summary 
Set Lookup Value for Subgrid
##### Parameters
**subgridName:** The subgridName
**index:** The Index
##### Example
xrmBrowser.Entity.SelectSubgridLookup("Stakeholders", 3);

#### SelectSubgridLookup(System.String,System.Boolean)
##### Summary 
Set Lookup Value for Subgrid
##### Parameters
**subgridName:** The SubgridName
**openLookupPage:** The Open Lookup Page
##### Example
xrmBrowser.Entity.SelectSubgridLookup("Stakeholders", true);

#### CloseEntity(System.Int32)
##### Summary 
Closes the current entity record you are on.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Entity.CloseEntity();

#### Save(System.Int32)
##### Summary 
Saves the specified entity record.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Entity.Save();

### Dynamics365.UIAutomation.Api.XrmGridPage
Xrm Grid Page
#### SwitchView(System.String,System.Int32)
##### Summary 
Switches the view.
##### Parameters
**viewName:** Name of the view.
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Grid.SwitchView("Open Opportunities");

#### Refresh(System.Int32)
##### Summary 
Refreshes this instance.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Grid.Refresh();

#### FirstPage(System.Int32)
##### Summary 
Firsts the page.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Grid.FirstPage();

#### NextPage(System.Int32)
##### Summary 
Nexts the page.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Grid.NextPage();

#### SelectAllRecords(System.Int32)
##### Summary 
Toggles the select all.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Grid.SelectAllRecords();

#### PreviousPage(System.Int32)
##### Summary 
Previouses the page.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Grid.PreviousPage();

#### OpenChart(System.Int32)
##### Summary 
Opens the chart.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Grid.OpenChart();

#### CloseChart(System.Int32)
##### Summary 
Closes the chart.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Grid.CloseChart();

#### Pin(System.Int32)
##### Summary 
Pins this instance.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Grid.Pin();

#### Search(System.String,System.Int32)
##### Summary 
Searches the specified search criteria.
##### Parameters
**searchCriteria:** The search criteria.
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Grid.Search("Test API");

#### Sort(System.String,System.Int32)
##### Summary 
Sorts the specified column name.
##### Parameters
**columnName:** Name of the column.
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Grid.Sort("Account Name");

#### GetGridItems
##### Summary 
Gets the grid items.
##### Example
var items = xrmBrowser.Grid.GetGridItems().Value;

#### OpenRecord(System.Int32,System.Int32)
##### Summary 
Opens the grid record.
##### Parameters
**index:** The index.
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Grid.OpenRecord(0);

#### SelectRecord(System.Int32,System.Int32)
##### Summary 
Selects the grid record.
##### Parameters
**index:** The index of the row you want to select. Index starts with 0.
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Grid.SelectRecord(1);

#### FilterByLetter(System.Char,System.Int32)
##### Summary 
Filters the by letter.
##### Parameters
**filter:** The filter.
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Grid.FilterByLetter('A');

#### FilterByAll(System.Int32)
##### Summary 
Filters the by all.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Grid.FilterByAll();

#### EnableFilter(System.Int32)
##### Summary 
Opens the filter.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Grid.EnableFilter();

#### SwitchChart(System.String,System.Int32)
##### Summary 
Switches the chart on the Grid.
##### Parameters
**chartName:** Name of the chart.
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Grid.SwitchChart("Accounts by Owner");

### Dynamics365.UIAutomation.Api.XrmGuidedHelpPage
Xrm Guided Help Page
#### CloseGuidedHelp
##### Summary 
Closes the Guided Help
##### Example
xrmBrowser.GuidedHelp.CloseGuidedHelp();

### Dynamics365.UIAutomation.Api.XrmLookupPage
Xrm Lookup Page
#### SwitchEntity(System.String,System.Int32)
##### Summary 
Switch Entity
##### Parameters
**entityName:** The Entity Name
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.

#### SwitchView(System.String,System.Int32)
##### Summary 
Switches the view
##### Parameters
**viewName:** The ViewName
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.

#### SelectItem(System.Int32,System.Int32)
##### Summary 
Selects the Item
##### Parameters
**index:** The Index
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Lookup.SelectItem(0);

#### SelectItem(System.String,System.Int32)
##### Summary 
Selects Item based on the value given
##### Parameters
**value:** The value
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Lookup.SelectItem("Alex Wu");

#### Add(System.Int32)
##### Summary 
Add Lookup
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Lookup.Add();

#### Select(System.Int32)
##### Summary 
Select from subgrid
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Lookup.Select();

## Dynamics365.UIAutomation.Api.XrmNavigationPage
The Xrm Navigation page.
#### OpenHomePage(System.Int32)
##### Summary 
Opens the home page
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Navigation.OpenHomePage();

#### OpenSubArea(System.String,System.String,System.Int32)
##### Summary 
Opens the Sub Area
##### Parameters
**area:** The area you want to open
**subArea:** The subarea you want to open
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Navigation.OpenSubArea("Sales", "Opportunities");

#### OpenRelated(System.String,System.Int32)
##### Summary 
Opens the Related Menu
##### Parameters
**relatedArea:** The Related area
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Navigation.OpenRelated("Cases");

#### GlobalSearch(System.String,System.Int32)
##### Summary 
Global Search
##### Parameters
**searchText:** The SearchText
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Navigation.GlobalSearch("Contoso");

#### OpenAdvancedFind(System.Int32)
##### Summary 
Open the Advanced Find
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Navigation.OpenAdvancedFind();

#### QuickCreate(System.String,System.Int32)
##### Summary 
Open Quick Create
##### Parameters
**entity:** The entity name
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Navigation.QuickCreate("Account");

#### SignOut(System.Int32)
##### Summary 
SignOut
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Navigation.SignOut();

#### OpenGuidedHelp(System.Int32)
##### Summary 
Opens the Guided Help
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Navigation.OpenGuidedHelp();

#### OpenAdminPortal(System.Int32)
##### Summary 
Opens the Admin Portal
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Navigation.OpenAdminPortal();

#### OpenOptions(System.Int32)
##### Summary 
Opens the Options
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Navigation.OpenOptions();

#### OpenPrintPreview(System.Int32)
##### Summary 
Opens the print preview
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Navigation.OpenPrintPreview();

#### OpenAppsForDynamicsCRM(System.Int32)
##### Summary 
Open Apps for Dynamics
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Navigation.OpenAppsForDynamicsCRM();

#### OpenWelcomeScreen(System.Int32)
##### Summary 
Opens Welcome Screen from navigation bar
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Navigation.OpenWelcomeScreen();

#### OpenAbout(System.Int32)
##### Summary 
Opens About from navigation bar
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Navigation.OpenAbout();

#### OpenOptOutLearningPath(System.Int32)
##### Summary 
Opens OptOut Learning Path
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Navigation.OpenOptOutLearningPath();

#### OpenPrivacyStatement(System.Int32)
##### Summary 
Opens the Privacy Statement
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Navigation.OpenPrivacyStatement();

### Dynamics365.UIAutomation.Api.XrmBusinessProcessFlow
Business process flow.
#### NextStage(System.Int32)
##### Summary 
Moves to the Next stage in the Business Process Flow.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.BusinessProcessFlow.NextStage();

#### PreviousStage(System.Int32)
##### Summary 
Moves to the Previous stage in the Business Process Flow.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.BusinessProcessFlow.PreviousStage();

#### Hide(System.Int32)
##### Summary 
Hides the Business Process flow UI.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.BusinessProcessFlow.Hide();

#### SelectStage(System.Int32,System.Int32)
##### Summary 
Selects the Business Process Flow stage.
##### Parameters
**stagenumber:** The stage number that you would like to select. The stages start with 0.
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.BusinessProcessFlow.SelectStage(0);

#### SetActive(System.Int32)
##### Summary 
Sets the current selected Stage as Active.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.BusinessProcessFlow.SetActive();

#### SelectBusinessProcessFlow(System.String,System.Int32)
##### Summary 
Selects the Business Process Flow from the Dialog.
##### Parameters
**name:** The name of the business process flow you want to select.
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.BusinessProcessFlow.SelectBusinessProcessFlow("Opportunity Saless Process Alt");

### Dynamics365.UIAutomation.Api.XrmNotficationPage
Xrm Notfication Page
#### CloseNotifications(System.Int32)
##### Summary 
Closes the Notifications
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Notifications.CloseNotifications();

### Dynamics365.UIAutomation.Api.XrmQuickCreatePage
Xrm Quick Create Page
### Methods
#### Constructor
#### #ctor(Microsoft.Dynamics365.UIAutomation.Browser.InteractiveBrowser)
##### Summary 
Initializes a new instance of the class.
##### Parameters
**browser:** The browser.

#### Cancel(System.Int32)
##### Summary 
Cancel the Quick Create Page
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.QuickCreate.Cancel();

#### Save(System.Int32)
##### Summary 
Save the Quick create page
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.QuickCreate.Save();

#### SelectLookup(System.String,System.Int32)
##### Summary 
Set Lookup Value for the field
##### Parameters
**field:** The Field Value
**index:** The Index
##### Example
xrmBrowser.QuickCreate.SelectLookup("primarycontactid", 0);

#### SetValue(System.String,System.Boolean)
##### Summary 
Sets the value of a Checkbox field.
##### Parameters
**field:** Field name or ID.
**check:** If set to true [check].

#### SetValue(System.String,System.DateTime)
##### Summary 
Sets the value of a Date Field.
##### Parameters
**field:** The field id or name.
**date:** DateTime value.

#### SetValue(System.String,System.String)
##### Summary 
Sets the value of a Text/Description field.
##### Parameters
**field:** The field id.
**value:** The value.
##### Example
xrmBrowser.QuickCreate.SetValue("name", "Test API Account");

#### SetValue(Microsoft.Dynamics365.UIAutomation.Api.Field)
##### Summary 
Sets the value of a Field.
##### Parameters
**field:** The field .

#### SetValue(Microsoft.Dynamics365.UIAutomation.Api.OptionSet)
##### Summary 
Sets the value of a picklist.
##### Parameters
**option:** The option you want to set.

#### SetValue(Microsoft.Dynamics365.UIAutomation.Api.CompositeControl)
##### Summary 
Sets the value of a Composite control.
##### Parameters
**control:** The Composite control values you want to set.

## Dynamics365.UIAutomation.Api.Pages.XrmActivityFeedPage
Activity feed page.
#### SelectTab(Microsoft.Dynamics365.UIAutomation.Api.Pages.XrmActivityFeedPage.Tab,System.Int32)
##### Summary 
Selects the tab.
##### Parameters
**tabname:** The Tabname.
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.ActivityFeed.SelectTab(Api.Pages.XrmActivityFeedPage.Tab.Activities);

#### AddNote(System.String,System.Int32)
##### Summary 
Adds Notes to the activity feed
##### Parameters
**noteText:** The NoteText
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.ActivityFeed.AddNote("Test Add Note");

#### AddPost(System.String,System.Int32)
##### Summary 
Adds Post to the activity feed
##### Parameters
**postText:** The postText
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.ActivityFeed.AddPost("Test Add Post");

#### FilterActivitiesByStatus(Microsoft.Dynamics365.UIAutomation.Api.Pages.XrmActivityFeedPage.Status,System.Int32)
##### Summary 
Filters Activities by Status
##### Parameters
**status:** The Status
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.

#### OpenActivitiesAssociatedView(System.Int32)
##### Summary 
Opens Activities Associated View
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.

#### AddPhoneCall(System.String,System.Boolean,System.Boolean,System.Int32)
##### Summary 
Adds PhoneCall to the activity feed
##### Parameters
**description:** The description
**leftVoiceMail:** The leftVoiceMail
**outgoing:** The outgoing
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.ActivityFeed.AddPhoneCall("Test Phone call Description",false);

#### AddTask(System.String,System.String,System.DateTime,Microsoft.Dynamics365.UIAutomation.Api.OptionSet,System.Int32)
##### Summary 
Adds task to the activity feed
##### Parameters
**subject:** The subject
**description:** The description
**dueDate:** The dueDate
**priority:** The priority
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.

#### AddEmail(System.Int32)
##### Summary 
Adds Email to the activity feed
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.ActivityFeed.AddEmail();

#### AddAppointment(System.Int32)
##### Summary 
Adds Appointment to the activity feed
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.ActivityFeed.AddAppointment();

## Dynamics365.UIAutomation.Api.XrmRelatedGridPage
Xrm Related Grid Page
#### SwitchView(System.String,System.Int32)
##### Summary 
Switches from one view to another
##### Parameters
**viewName:** Name of the view to which you want to switch
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Related.SwitchView("Active Cases");

#### Refresh(System.Int32)
##### Summary 
Refresh
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.

#### FirstPage(System.Int32)
##### Summary 
Firsts the page.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.

#### NextPage(System.Int32)
##### Summary 
Nexts the page.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.

#### SelectAllRecords(System.Int32)
##### Summary 
Toggles the select all.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.

#### PreviousPage(System.Int32)
##### Summary 
Previouses the page.
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.

#### Search(System.String,System.Int32)
##### Summary 
Searches the specified search criteria.
##### Parameters
**searchCriteria:** The search criteria.
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Related.Search("F");

#### Sort(System.String,System.Int32)
##### Summary 
Sorts the specified column name.
##### Parameters
**columnName:** Name of the column.
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Related.Sort("createdon");

#### GetGridItems(System.Int32)
##### Summary 
Get the Grid Items
##### Parameters
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.

#### OpenGridRow(System.Int32,System.Int32)
##### Summary 
Opens the grid record.
##### Parameters
**index:** The index.
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Related.OpenGridRow(0);

#### ClickCommand(System.String,System.String,System.Boolean,System.Int32)
##### Summary 
Clicks the Command
##### Parameters
**name:** The name
**subName:** The subName
**moreCommands:** The moreCommands
**thinkTime:** Used to simulate a wait time between human interactions. The Default is 2 seconds.
##### Example
xrmBrowser.Related.ClickCommand("ADD NEW CASE");

### Dynamics365.UIAutomation.Api.XrmPage
Xrm Page
#### SetValue(System.String,System.Boolean)
##### Summary 
Sets the value of a Checkbox field.
##### Parameters
**field:** Field name or ID.
**check:** If set to true [check].
##### Example
xrmBrowser.Entity.SetValue("creditonhold",true);

#### SetValue(System.String,System.DateTime)
##### Summary 
Sets the value of a Date Field.
##### Parameters
**field:** The field id or name.
**date:** DateTime value.
##### Example
xrmBrowser.Entity.SetValue("birthdate", DateTime.Parse("11/1/1980"));

#### SetValue(System.String,System.String)
##### Summary 
Sets the value of a Text/Description field.
##### Parameters
**field:** The field id.
**value:** The value.
##### Example
SetValue(Elements.ElementId[Reference.Dialogs.CloseOpportunity.ActualRevenueId], revenue.ToString());

#### SetValue(Microsoft.Dynamics365.UIAutomation.Api.OptionSet)
##### Summary 
Sets the value of a picklist.
##### Parameters
**option:** The option you want to set.
##### Example
xrmBrowser.Entity.SetValue(new OptionSet {Name = "preferredcontactmethodcode", Value = "Email"});

#### SetValue(Microsoft.Dynamics365.UIAutomation.Api.CompositeControl)
##### Summary 
Sets the value of a Composite control.
##### Parameters
**control:** The Composite control values you want to set.
##### Example
xrmBrowser.Entity.SetValue(new CompositeControl() { Id = "fullname", Fields = fields });

#### SetValue(Microsoft.Dynamics365.UIAutomation.Api.Lookup)
##### Summary 
Sets the value of a Lookup.
##### Parameters
**control:** The lookup field name, value or index of the lookup.
##### Example
this.SetValue(new Lookup() { Name = Elements.ElementId[Reference.Dialogs.Assign.UserOrTeamLookupId], Value = value });

#### GetValue(System.String)
##### Summary 
Gets the value of a Text/Description field.
##### Parameters
**field:** The field id.
##### Example
string mobilePhone = xrmBrowser.Entity.GetValue("mobilephone");

#### GetValue(Microsoft.Dynamics365.UIAutomation.Api.CompositeControl)
##### Summary 
Gets the value of a Composite control.
##### Parameters
**control:** The Composite control values you want to set.
##### Example
string fullName = xrmBrowser.Entity.GetValue(new CompositeControl() { Id = "fullname", Fields = fields });

#### GetValue(Microsoft.Dynamics365.UIAutomation.Api.OptionSet)
##### Summary 
Gets the value of a picklist.
##### Parameters
**option:** The option you want to set.
##### Example
string options = xrmBrowser.Entity.GetValue(new OptionSet { Name = "preferredcontactmethodcode"});

#### GetValue(Microsoft.Dynamics365.UIAutomation.Api.Lookup)
##### Summary 
Gets the value of a Lookup.
##### Parameters
**control:** The lookup field name, value or index of the lookup.
##### Example
string lookupValue = xrmBrowser.Entity.GetValue(new Lookup { Name = "primarycontactid" });

### Dynamics365.UIAutomation.Api.Pages.XrmDocumentPage
The XRM Document Page that provides methods to interact with the browswer DOM.
#### getElementById(System.String)
##### Summary 
Returns the document element that has the ID attribute with the specified value.
##### Parameters
**id:** The identifier.
#### getElementByCss(System.String)
##### Summary 
Returns the document element that has the CSS attribute with the specified value.
##### Parameters
**css:** The CSS.
#### getElementByXPath(System.String)
##### Summary 
Returns the document element that has the specified XPath value.
##### Parameters
**xpath:** The xpath that is used to references nodes in the document.
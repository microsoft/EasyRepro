---
name: Bug report
about: Create an issue to report a problem
title: "[BUG] <Issue Description>"
labels: bug
assignees: ''

---

## Bug Report

### Issues should only be created for items related to [covered functionality](https://github.com/microsoft/EasyRepro#crm-functionality-covered). 

### [Not covered](https://github.com/microsoft/EasyRepro#crm-functionality-not-covered) functionality, feature requests, and questions should use the Feature Request or Question templates.


**EasyRepro Version**
  - [ ] Microsoft Dynamics 365 Online Version 8.2 (8.2.x) (DB 8.2.x) online
  - [ ] Microsoft Dynamics 365 Online Version 9.0 (9.0.x) (DB 9.0.x) online
  - [ ] Microsoft Dynamics 365 Online Version 9.0.2 (9.0.2) (DB 9.0.2) online
  - [ ] Microsoft Dynamics 365 Online Version 9.1 (9.1.x) (DB 9.1.x) online
  
**UCI or Classic Web**
  - [ ] UCI (XrmApp)
  - [ ] Classic Web (XrmBrowser)

**Online or On Premise**
  - [ ] Online
  - [ ] On Premise

**Browser**
  - [ ] Chrome
  - [ ] Firefox
  - [ ] IE
  - [ ] Edge

**Describe the bug**  
<!-- A clear and concise description of what the bug is. -->

**Special formatting / display**  
<!-- Does the control have any formatting applied to it (e.g. editable grid or two option displayed as a checkbox), is it in the header/footer, etc.? -->

**Code to reproduce**  
<!-- Minimum code to reproduce the behavior -->
<!-- Please enclose in a code block

```csharp
xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

xrmApp.Navigation.OpenApp(UCIAppName.Sales);

xrmApp.Navigation.QuickCreate("contact");

xrmApp.QuickCreate.SetValue(new LookupItem() { Name = "parentcustomerid", Value = "Test" });

xrmApp.QuickCreate.Save();
```
 -->
 
**Expected behavior**  
<!-- A clear and concise description of what you expected to happen. -->

**Screenshots**  
<!-- You are encouraged to add screenshots to help diagnose your problem. -->

**Additional context**  
<!-- Add any other context about the issue or things you've already tried here. -->

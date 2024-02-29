# Overview #
The purpose of this library is to provide Dynamics customers the ability to facilitate automated UI testing for their projects. These API's provide an easy to use set of commands that make setting up UI testing quick and easy. The functionality provided covers the core Dynamics 365 commands that end users would perform on a typical workday and working to extend that coverage to more functionality.

## Breaking Changes from earlier versions of Easy Repro
The impact to all tests is extremely minimal. In the case there is a potential breaking change, please review the section below.
### Migraiton to .NET Core 
This version of Easy Repro uses .NET Core 6. Previous versions used .NET Framework 4.6.1. This impacts how configuration for tests are retrieved.
All tests have been updated showing how to use the runsettings file in combination with the ClassInitialize attribute.
[ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
### Timeline
Previous tests using Timeline references will need to be updated to create an instance of Timeline.TimelineReference. 
This will allow developers to update the locator at run time avoiding modification of source code.
Previous tests used the Elements object as shown here [https://github.com/microsoft/EasyRepro/blob/1935401875313a9059481d8af0a3708f66a3fe08/Microsoft.Dynamics365.UIAutomation.Sample/UCI/Timeline/AddActivity.cs#L150]
New tests should use the Timeline.TimelineReference as shown here: [https://github.com/microsoft/EasyRepro/blob/558c0bf9e4df8ee483c076378e442fe8f3b43ce5/Microsoft.Dynamics365.UIAutomation.Sample/UCI/Timeline/AddActivity.cs#L154]

## Software Requirements
### Supported Version
#### Supported Operating System
Windows 11, Windows 10 , Windows 7, Windows 8, Windows 8.1, Windows Server 2008, Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2  
#### Supported Visual studio 
Microsoft Visual Studio 2015, Microsoft Visual Studio 2017, Microsoft Visual Studio 2019, Microsoft Visual Studio 2022

### Dynamics 365 Versions
Easy Repro supports the latest version of Microsoft Dynamics 365 Customer Engagement.

#### Selenium Webdriver & Support
Selenium is a set of different software tools each with a different approach to supporting browser automation. These tools are highly flexible, allowing many options for locating and manipulating elements within a browser, and one of its key features is the support for automating multiple browser platforms. This package contains the .NET bindings for the newer, more concise and object-based Selenium WebDriver API, which uses native OS-level events to manipulate the browser, bypassing the JavaScript sandbox, and does not require the Selenium Server to automate the browser.


| File Name         | Version | Release date   |
| ------------------|---------|----------------|
| Selenium.WebDriver| v4.8  | January 23, 2023 |
| Selenium.Support  | v4.8  | January 23, 2023 |

#### Playwright Webdriver & Support
Playwright enables reliable end-to-end testing for modern web apps. It is built to enable cross-browser web automation that is ever-green, capable, reliable and fast. Learn more at https://playwright.dev/dotnet/.


| File Name         | Version | Release date   |
| ------------------|---------|----------------|
| Microsoft.Playwright| v1.40.0  | November 21, 2023 |

## Coverage #
### Dynamics 365 Functionality Covered
- Activity Record Wall
- Business Process Flow
- Charts
- Commandbar
- Dashboards
- Entities (Create, Update, Delete, Duplicate Detection, Assign)
- Global Search
- Guided Help
- Grids
- Sub-Grids
- Navigation
- Notifications
- Performance Center
- Quick Create
- Rich Text Editor
- Run Workflows
- Run Reports

### Dynamics 365 Functionality Not Covered
- Settings Area  (Administration)
- Customization 
- Advanced Find Query execution(Open Advanced Find is available)
- Export to Excel
- Templates
- Other Add on Applications

Although we don't have specific commands to cover the above funcationality, we have generic commands that will allow you to still interact with those features. 

## Known Issues #
- Firefox has sometimes been inconsistent with testing results.  The click command is inconsistent. This is a known issue and has been reported to that team that manages that driver. 

## Setup #
#### Install Instructions:
The Sample project should have NUGET packages added and you just need to build the solution and restore the packages.  For the specific browser drivers, you can add them using NUGET. The default driver is Chrome and the NUGET package is already installed on the Sample project.   

#### Run the Sample Project

Change the keys in your `app.config` below to match your environment. Open the Test explorer window. Right click and run one test to ensure everything works. Trying the `CreateAccount` test under `Microsoft.Dynamics365.UIAutomation.Sample` is a good start.

```cs
<add key="OnlineUsername" value="name@name.onmicrosoft.com" />
<add key="OnlinePassword" value="*********" />
<add key="OnlineCrmUrl" value="https://org.crm.dynamics.com/" />
```

#### MFA Support
If the account you are using has MFA (Multi-Factor Authentication) enabled, you will also need to provide an MFA Secret Key.
You can obtain an MFA secret key for your account using these instructions - you will need Microsoft's Authenticator App to get this working:

<ol >
<li>Go to your Office365 'Security info' page at: <a href="https://mysignins.microsoft.com/security-info" target="_blank">https://mysignins.microsoft.com/security-info</a></li> 
<li>Enter your O365 login and password</li>
<li>Click 'Add method' and select "Authenticator app" in Combobox, click Add</li>
<li>Click on "I want to use a different authenticator app" link</li>
<li>Click on the 'Next' button, click on the 'Next' button</li>
<li>Click on 'Can't scan image?'</li>
<li>Make a note of the 'Secret key' which is displayed</li>
<li>Open Authenticator app and click the '+' to add an account and select 'work or school account'</li>
<li>Scan the QR code displayed on the web page</li>
<li>When the App displays a six-digit code, click 'Next' on the web page</li>
<li>Enter the six-digit verification code from the app into the web page and Click the 'Next' button</li>
<li>Add your MFA Secret key from step 7 <i>with the spaces removed</i> to your app.config as shown below, in the same section that you add your username and password</li>
</ol>

```cs
<add key="MfaSecretKey" value="MfaSecretKeyWithSpacesRemoved" />
```

#### Supported Browser WebDriver
 The application supports following browser:

| Browser    | WebDriver           | Version           | Release date      |
| -----------|-------------------- |-----------------  | --------------    |
| Chrome     | ChromeDriver        |[SeleniumWebDriver.ChromeDriver](https://www.nuget.org/packages/Selenium.WebDriver.ChromeDriver)         |  Found in NuGet Package  |
| Firefox    | GeckoDriver         |v.0.16.1           |  April 27, 2017   |
| Edge       | MicrosoftWebDriver  |v.10.0.15063.0     |  April 18, 2017   |

#### Browser Driver Download and install instructions
If you want to download the drivers manually you can go to the specific driver download links and put them in the bin folder of your project. 

#### Chrome:
Download the Chrome Web driver from the below link
[Download chrome driver](https://sites.google.com/a/chromium.org/chromedriver/downloads)
#### FireFox:
The Firefox web driver can be installed through Nuget package manager.
- In Visual studio ,Go to Tools -> NuGet Package Manager -> Manage Nuget Packages for the solution
***FileName*** : Selenium.WebDriver.GeckoDriver.Win64
#### Edge:
Click on the below link to download Edge driver
[Download Edge driver](https://www.microsoft.com/en-us/download/details.aspx?id=48212)

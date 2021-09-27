# Overview #
The purpose of this library is to provide Dynamics customers the ability to facilitate automated UI testing for their projects. These API's provide an easy to use set of commands that make setting up UI testing quick and easy. The functionality provided covers the core CRM commands that end users would perform on a typical workday and working to extend that coverage to more functionality.

## Software Requirements
### Supported Version
#### Supported Operating System
Windows 10 , Windows 7, Windows 8, Windows 8.1, Windows Server 2008, Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2  
#### Supported Visual studio 
Microsoft Visual Studio 2015, Microsoft Visual Studio 2017

### CRM Versions
Easy Repro supports below CRM Versions	
- Microsoft Dynamics 365 Online Version 8.2 (8.2.x) (DB 8.2.x) online
- Microsoft Dynamics 365 Online Version 9.0 (9.0.x) (DB 9.0.x) online
- Microsoft Dynamics 365 Online Version 9.0.2 (9.0.2) (DB 9.0.2) online
- Microsoft Dynamics 365 Online Version 9.1 (9.1.x) (DB 9.1.x) online

#### Selenium Webdriver & Support
Selenium is a set of different software tools each with a different approach to supporting browser automation. These tools are highly flexible, allowing many options for locating and manipulating elements within a browser, and one of its key features is the support for automating multiple browser platforms. This package contains the .NET bindings for the newer, more concise and object-based Selenium WebDriver API, which uses native OS-level events to manipulate the browser, bypassing the JavaScript sandbox, and does not require the Selenium Server to automate the browser.


| File Name         | Version | Release date   |
| ------------------|---------|----------------|
| Selenium.WebDriver| v3.11.2  | April 12, 2018 |
| Selenium.Support  | v3.11.2  | April 12, 2018 |

## Coverage #
### CRM Functionality Covered
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
- Run Workflows
- Run Reports

### CRM Functionality Not Covered
- Settings Area  (Administration)
- Customization 
- Advanced Find Query execution(Open Advanced Find is available)
- Export to Excel
- Templates
- Other Add on Applications

Although we don't have specific commands to cover the above funcationality, we have generic commands that will allow you to still interact with those features. 

## Known Issues #
- Microsoft Edge Browser, based on Chromium, is currently not supported.
- Firefox has sometimes been inconsistent with testing results.  The click command is inconsistent. This is a known issue and has been reported to that team that manages that driver. 
- IE Driver has a 32-bit version and a 64-bit version. If you're using the 32-bit version on 64-bit windows with 64-bit IE then the driver will not work as intended.  Use the appropriate version of IE Driver with the specific version of IE. 

## Setup #
#### Install Instructions:
The Sample project should have NUGET packages added and you just need to build the solution and restore the packages.  For the specific browser drivers, you can add them using NUGET. The default driver is Chrome and the NUGET package is already installed on the Sample project.   

#### Run the Sample Project

Change the keys in your `app.config` below to match your environment. Open the Test explorer window. Right click and run one test to ensure everything works. Trying the `CreateAccount` test under `Microsoft.Dynamics365.UIAutomation.Sample.Web` is a good start.

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
| Chrome     | ChromeDriver        |v.2.38.0.0         |  April 21, 2018    |
| Firefox    | GeckoDriver         |v.0.16.1           |  April 27, 2017   |
| IE         | IEDriver            |v.3.4              |  April 22, 2017   |
| Edge       | MicrosoftWebDriver  |v.10.0.15063.0     |  April 18, 2017   |

#### Browser Driver Download and install instructions
If you want to download the drivers manually you can go to the specific driver download links and put them in the bin folder of your project. 

#### Chrome:
Download the Chrome Web driver from the below link
[Download chrome driver](https://sites.google.com/a/chromium.org/chromedriver/downloads)
#### Internet Explorer:
Download the IE Web driver from the below link
[Download IE driver](http://selenium-release.storage.googleapis.com/3.4/IEDriverServer_x64_3.4.0.zip) 
#### FireFox:
The Firefox web driver can be installed through Nuget package manager.
- In Visual studio ,Go to Tools -> NuGet Package Manager -> Manage Nuget Packages for the solution
***FileName*** : Selenium.WebDriver.GeckoDriver.Win64
#### Edge:
Click on the below link to download Edge driver
[Download Edge driver](https://www.microsoft.com/en-us/download/details.aspx?id=48212)

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
- Microsoft Dynamics® CRM Online 2016 Update (8.1.1.1003) (DB 8.1.1.1003)
- Microsoft Dynamics 365 Version 1612 (8.2.1.185) (DB 8.2.1.185) online

#### Selenium Webdriver & Support
Selenium is a set of different software tools each with a different approach to supporting browser automation. These tools are highly flexible, allowing many options for locating and manipulating elements within a browser, and one of its key features is the support for automating multiple browser platforms. This package contains the .NET bindings for the newer, more concise and object-based Selenium WebDriver API, which uses native OS-level events to manipulate the browser, bypassing the JavaScript sandbox, and does not require the Selenium Server to automate the browser.


| File Name         | Version | Release date   |
| ------------------|---------|----------------|
| Selenium.WebDriver| v3.5.1  | April 21, 2017 |
| Selenium.Support  | v3.5.1  | April 21, 2017 |

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
- Edge Browser is currently not supported due to known bug that is planned for future release.
- Firefox has sometimes been inconsistent with testing results.  The click command is inconsistent. This is a known issue and has been reported to that team that manages that driver. 
- IE Driver has a 32-bit version and a 64-bit version. If you're using the 32-bit version on 64-bit windows with 64-bit IE then the driver will not work as intended.  Use the appropriate version of IE Driver with the specific version of IE. 

## Setup #
#### Install Instructions:
The Sample project should have NUGET packages added and you just need to build the solution and restore the packages.  For the specific browser drivers, you can add them using NUGET. The default driver is Chrome and the NUGET package is already installed on the Sample project.   

#### Supported Browser WebDriver
 The application supports following browser:
  
| Browser    | WebDriver           | Version           | Release date      |
| -----------|-------------------- |-----------------  | --------------    |
| Chrome     | ChromeDriver        |v.2.31.0.0         |  April 4, 2017    |
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

*******************************************************************************
Creating Nuget Package
*******************************************************************************
Description-
To create a nuget package for a specific "nuspec" file run the following
command and a nuget package "nupkg" file will be created with the Id and
version present in that "nuspec" file.
Run it in command prompt or powershell from this directory where the readme
resides.

Command-
nuget pack <nuspec file name>.nuspec  -prop configuration=release -BasePath ..\Microsoft.Dynamics365.UIAutomation.Api\

Output-
File with the following file name <Id>.<Version>.nupkg

*******************************************************************************
Publishing Nuget Package
*******************************************************************************
Description-
To publish a specific nuget package to the source repository we required the
ApiKey which is got from the https://www.nuget.org/ account details.
The apikey is a secret which should not be checked but kept secure.

Additional information -
https://docs.microsoft.com/en-us/nuget/create-packages/publish-a-package

Command-
nuget push <nuget pacakge name>.nupkg -Source https://www.nuget.org/api/v2/package -ApiKey <api key>

Output-
The nuget package will be uploaded to www.nuget.org
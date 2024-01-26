param(
[string]$a
)


write-host "Starting Tests"
Write-Host $a
dotnet vstest Microsoft.Dynamics365.UIAutomation.Sample.dll --TestCaseFilter:"FullyQualifiedName~Microsoft.Dynamics365.UIAutomation.Sample.RegressionTests.Regression_Grid_SwitchView_GetItems_Sort_SearchRecord_HighlightRecord_OpenRecord" --logger:trx --settings:"EasyRepro.runsettings"
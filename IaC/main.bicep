@description('The name of the environment. This must be dev, test, or prod.')
param logAnalyticsWorkspace string = '${uniqueString(resourceGroup().id)}la'
@description('The name of the environment. This must be dev, test, or prod.')
param applicationInsightsName string = '${uniqueString(resourceGroup().id)}ai'
@description('The name of the environment. This must be dev, test, or prod.')
param location string = resourceGroup().location
@allowed([
  'nonprod'
  'prod'
])
@description('The name of the environment. This must be dev, test, or prod.')
param environmentType string

var auditingEnabled = environmentType == 'prod'

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2021-12-01-preview' = {
  name: logAnalyticsWorkspace
  location: location
}





// resource diagnosticLogs 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
//   name: 'DataverseDiagnosticsSettings'
//   scope: appInsights
//   properties: {
//     workspaceId: logAnalytics.id
//     // logs: [
//     //   {
//     //     category: 'AllMetrics'
//     //     enabled: true
//     //     retentionPolicy: {
//     //       days: 30
//     //       enabled: true 
//     //     }
//     //   }
//     // ]
//   }
// }

module appInsights 'modules/appInsights.bicep' = {
  name: 'appService'
  params: {
    applicationInsightsName: applicationInsightsName
    location: location
    logAnalayticsId: logAnalytics.id
    environmentType: environmentType
  }
}

output connectionString string = appInsights.outputs.connectionString

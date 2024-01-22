param location string
param applicationInsightsName string
param logAnalayticsId string
@allowed([
  'nonprod'
  'prod'
])
param environmentType string


resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: applicationInsightsName
  kind: 'Dataverse'
  location: location
  tags: {
    Environment: environmentType
  }
  properties:{
    Application_Type: 'other'
    //ImmediatePurgeDataOn30Days: true
    DisableIpMasking: true
    WorkspaceResourceId: logAnalayticsId
  }
}

output connectionString string = appInsights.properties.ConnectionString

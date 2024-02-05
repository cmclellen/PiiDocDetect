@description('Location for all resources.')
param location string = resourceGroup().location

@description('The environment.')
param environment string

@description('Tags to tag all resources.')
param tags object = {
  Environment: environment
}

var resourceNameFormat = '{0}-piiid{1}-${environment}'

module ai './modules/ai.bicep' = {
  name: 'ai'
  params: {
    location: location
    resourceNameFormat: resourceNameFormat
    tags: tags
    functionAppId: function.outputs.functionAppId
    functionAppPrincipalId: function.outputs.functionAppPrincipalId
  }
}

module function './modules/function.bicep' = {
  name: 'function'
  params: {
    location: location
    resourceNameFormat: resourceNameFormat
    tags: tags
    lawId: monitoring.outputs.lawId
    appInsightsConnectionString: monitoring.outputs.appInsightsConnectionString
    appInsightsInstrumentationKey: monitoring.outputs.appInsightsInstrumentationKey
  }
}

module monitoring './modules/monitoring.bicep' = {
  name: 'monitoring'
  params: {
    location: location
    resourceNameFormat: resourceNameFormat
    tags: tags
  }
}

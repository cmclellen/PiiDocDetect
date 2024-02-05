param location string
param resourceNameFormat string
param tags object
param lawId string
param appInsightsInstrumentationKey string
param appInsightsConnectionString string

resource sa 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: replace(format(resourceNameFormat, 'st', ''), '-', '')
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    allowBlobPublicAccess: true
    minimumTlsVersion: 'TLS1_2'
    supportsHttpsTrafficOnly: true
  }
  tags: tags

  resource defaultBlobService 'blobServices' existing = {
    name: 'default'

    resource seeddata 'containers' = {
      name: 'seeddata'
      properties: {
        publicAccess: 'Blob'
      }
    }
  }
}

var storageAccountConnectionString = 'DefaultEndpointsProtocol=https;AccountName=${sa.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${sa.listKeys().keys[0].value}'

resource hostingPlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: format(resourceNameFormat, 'asp', '')
  location: location
  tags: tags
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
}

resource hostingPlanDiagSettings 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  scope: hostingPlan
  name: hostingPlan.name
  properties: {
    workspaceId: lawId
    metrics: [
      {
        category: 'AllMetrics'
        enabled: true
      }
    ]
  }
}

resource functionApp 'Microsoft.Web/sites@2023-01-01' = {
  name: format(resourceNameFormat, 'func', '')
  location: location
  tags: tags
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: hostingPlan.id
    clientCertEnabled: false
    siteConfig: {
      ftpsState: 'Disabled'
      minTlsVersion: '1.2'
    }
    httpsOnly: true
  }
}

module appSettingsModule 'appsettings.bicep' = {
  name: '${functionApp.name}-appSettings'
  params: {
    functionApp: functionApp.name
    currentAppSettings: list('${functionApp.id}/config/appsettings', '2023-01-01').properties
    newAppSettings: {
      AzureWebJobsStorage: storageAccountConnectionString
      WEBSITE_CONTENTAZUREFILECONNECTIONSTRING: storageAccountConnectionString
      WEBSITE_CONTENTSHARE: toLower(functionApp.name)
      FUNCTIONS_EXTENSION_VERSION: '~4'
      APPINSIGHTS_INSTRUMENTATIONKEY: appInsightsInstrumentationKey
      APPLICATIONINSIGHTS_CONNECTION_STRING: appInsightsConnectionString
      FUNCTIONS_WORKER_RUNTIME: 'dotnet-isolated'
      WEBSITE_RUN_FROM_PACKAGE: '1'
      WEBSITE_MAX_DYNAMIC_APPLICATION_SCALE_OUT: '1'
    }
  }
}

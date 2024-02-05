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
  }
}

module storage './modules/storage.bicep' = {
  name: 'storage'
  params: {
    location: location
    resourceNameFormat: resourceNameFormat
    tags: tags
  }
}

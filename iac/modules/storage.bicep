param location string
param resourceNameFormat string
param tags object

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

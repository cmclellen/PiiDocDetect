param functionApp string
param currentAppSettings object
param newAppSettings object

resource appSettings 'Microsoft.Web/sites/config@2023-01-01' = {
  name: '${functionApp}/appsettings'
  properties: union(currentAppSettings, newAppSettings)
}

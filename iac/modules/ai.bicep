param location string
param resourceNameFormat string
param tags object
param functionAppPrincipalId string
param functionAppId string

@allowed([
  'F0'
  'S0'
])
param sku string = 'F0'

resource cognitiveService 'Microsoft.CognitiveServices/accounts@2023-05-01' = {
  name: format(resourceNameFormat, 'di', '')
  location: location
  tags: tags
  sku: {
    name: sku
  }
  identity: {
    type: 'SystemAssigned'
  }
  kind: 'FormRecognizer'
  properties: {
    customSubDomainName: 'pii-doc-identify'
    apiProperties: {
      statisticsEnabled: false
    }
    networkAcls: {
      defaultAction: 'Allow'
    }
    publicNetworkAccess: 'Enabled'
  }
}

//   resource deployment_ada 'deployments' = {
//     name: 'ada-deployment'
//     sku: {
//       name: 'Standard'
//       capacity: 1
//     }
//     properties: {
//       model: {
//         format: 'OpenAI'
//         name: 'text-embedding-ada-002'
//         version: '2'
//       }
//     }
//     tags: tags
//   }

//   resource deployment_gpt35 'deployments' = {
//     name: 'gpt35-deployment'
//     sku: {
//       name: 'Standard'
//       capacity: 1
//     }
//     properties: {
//       model: {
//         format: 'OpenAI'
//         name: 'gpt-35-turbo'
//         version: '1106'
//       }
//     }
//     tags: tags
//   }
// }

// resource search 'Microsoft.Search/searchServices@2023-11-01' = {
//   name: format(resourceNameFormat, 'srch', '')
//   location: location
//   sku: {
//     name: 'basic'
//   }
//   properties: {
//     replicaCount: 1
//     partitionCount: 1
//     hostingMode: 'default'
//   }
//   tags: tags
// }

var roleIds = [
  'a97b65f3-24c7-4388-baec-2e87135dc908' // Cognitive Services User
]

resource roleDefinitions 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = [for roleId in roleIds: {
  scope: subscription()
  name: roleId
}]

resource roleAssignments 'Microsoft.Authorization/roleAssignments@2022-04-01' = [for (roleId, ind) in roleIds: {
  scope: cognitiveService
  name: guid(cognitiveService.id, functionAppId, roleDefinitions[ind].id)
  properties: {
    roleDefinitionId: roleDefinitions[ind].id
    principalId: functionAppPrincipalId
    principalType: 'ServicePrincipal'
  }
}]

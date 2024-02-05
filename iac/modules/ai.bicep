param location string
param resourceNameFormat string
param tags object

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

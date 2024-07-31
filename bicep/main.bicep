targetScope = 'subscription'

param resourceGroupName string = 'sunyam-lexicon-rg'
param workspaceName string = 'sunyam-lexicon-2024'

resource rg 'Microsoft.Resources/resourceGroups@2024-03-01' existing = {
  name: resourceGroupName
}

resource keyVault 'Microsoft.KeyVault/vaults@2024-04-01-preview' existing = {
  name: 'BicepVault'
  scope: rg
}

module azureSQL 'azureSql-module.bicep' = {
  name: '${workspaceName}-azuresql'
  scope: rg
  params: {
    administratorLoginPassword: keyVault.getSecret('azuresql-admin-pass')
    administratorLogin: 'azuresql-admin'
  }
}

module postgresSQL 'postgreSql-module.bicep' = {
  name: '${workspaceName}-postgresql'
  scope: rg
  params: {
    administratorLoginPassword: keyVault.getSecret('postgresql-admin-pass')
    adminstratorLogin: 'postgresql-admin'
  }
}

module apiManagement 'apiManagement-module.bicep' = {
  name: '${workspaceName}-apiManagement'
  scope: rg
  params: {
    publisherEmail: 'owner@development.com'
    publisherName: 'owner'
  }
}

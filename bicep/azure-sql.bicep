@description('Azure SQL Admin user login name')
param administratorLogin string = 'azuresql-admin'

@secure()
@description('Azure SQL Admin user login password')
param administratorLoginPassword string

@description('Location for all resources')
param location string = resourceGroup().location

@description('Demo server name')
param serverName string = 'lexicon-demoserver-azuresql'

@description('Demo database name')
param sqlDBName string = 'azuresql-demodb'

@description('Demo database SKU name')
param skuName string = 'Standard'

@description('Demo database SKU tier')
param skuTier string = 'Standard'

resource sqlServer 'Microsoft.Sql/servers@2023-08-01-preview' = {
  name: serverName
  location: location
  properties: {
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorLoginPassword
  }
}

resource sqlDB 'Microsoft.Sql/servers/databases@2023-08-01-preview' = {
  parent: sqlServer
  name: sqlDBName
  location:location
  sku: {
    name: skuName
    tier: skuTier
  }
}

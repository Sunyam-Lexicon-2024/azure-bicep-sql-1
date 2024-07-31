@description('Postgres Admin user login name')
param adminstratorLogin string

@description('Postgres Admin user login password')
@secure()
param administratorLoginPassword string

@description('Location for all resources')
param location string = resourceGroup().location

@description('Demo server name')
param serverName string = 'lexicon-demoserver-postgres'

@description('Server type specification')
param serverEdition string = 'GeneralPurpose'

@description('Storage capacity of database')
param skuSizeGB int = 128

@description('Database instance type')
param dbInstanceType string = 'Standard_D4ds_v4'

@description('Database redundancy mode')
param haMode string = 'ZoneRedundant'

@description('Zone availability configuration')
param availabilityZone string = '1'

@description('Postgres version')
param version string = '12'

@description('External VNET reference')
param virtualNetworkExternalId string = ''

@description('Subnet reference')
param subnetName string = ''

@description('Private Azure ARM DNS reference')
param privateDnsZoneArmResourceId string = ''

@description('Postgres database name')
param postgresDBName string = 'postgres-demodb'

resource postgresServer 'Microsoft.DBforPostgreSQL/flexibleServers@2023-12-01-preview' = {
  name: serverName
  location: location
  sku: {
    name: dbInstanceType
    tier: serverEdition
  }
  properties: {
    version: version
    administratorLogin: adminstratorLogin
    administratorLoginPassword: administratorLoginPassword
    network: {
      delegatedSubnetResourceId: (empty(virtualNetworkExternalId)
        ? json('null')
        : json('\'${virtualNetworkExternalId}/subnets/${subnetName}\''))
      privateDnsZoneArmResourceId: (empty(virtualNetworkExternalId) ? json('null') : privateDnsZoneArmResourceId)
    }
    highAvailability: {
      mode: haMode
    }
    storage: {
      storageSizeGB: skuSizeGB
    }
    backup: {
      backupRetentionDays: 7
      geoRedundantBackup: 'Disabled'
    }
    availabilityZone: availabilityZone
  }
}

resource postgresDB 'Microsoft.DBforPostgreSQL/flexibleServers/databases@2023-12-01-preview' = {
  name: postgresDBName
  parent: postgresServer
  properties: {
    charset: 'UTF8'
    collation: 'en_US.utf8'
  }
}

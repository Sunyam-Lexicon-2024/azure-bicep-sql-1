@description('API Managemnt service instance')
param apiManagementServiceName string = 'apiservice${uniqueString(resourceGroup().id)}'

@description('Service owner email')
@minLength(1)
param publisherEmail string

@description('Service owner name')
@minLength(1)
param publisherName string

@description('API management pricing tier')
@allowed([
  'Consumption'
  'Developer'
  'Basic'
  'BasicV2'
  'Standard'
  'StandardV2'
  'Premium'
])
param sku string = 'Developer'

@description('API Management service instance size')
@allowed([
  0
  1
  2
])
param skuCount int = 1

@description('Location for all resources')
param location string = resourceGroup().location

resource apiManagementService 'Microsoft.ApiManagement/service@2023-09-01-preview' = {
  name: apiManagementServiceName
  location: location
  sku: {
    name: sku
    capacity: skuCount
  }
  properties: {
    publisherEmail: publisherEmail
    publisherName: publisherName
  }
}

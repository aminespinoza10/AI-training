param openAiEndpoint string
param openAiApiKey string
param wbData object = base64ToJson(loadFileAsBase64('./workbook_template.json'))
param rgLocation string = resourceGroup().location

module appInsights './appInsights.bicep' = {
  name: 'appInsightsModule'
  params: {
    rgLocation: rgLocation
    wbSerializedData: wbData
  }
}

module apiManagement './apiManagement.bicep' = {
  name: 'apiManagementModule'
  params: {
    openAiEndpoint: openAiEndpoint
    openAiApiKey: openAiApiKey
    appInsightsInstrumentationKey: appInsights.outputs.instrumentationKey
    appInsightsId: appInsights.outputs.id
    rgLocation: rgLocation
  }
}

module dashboard './dashboard.bicep' = {
  name: 'dashboardModule'
  params: {
    apiGateway: apiManagement.outputs.gatewayUrl
    appInsightsId: appInsights.outputs.id
    appInsightsName: appInsights.name
  }
}

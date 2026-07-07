# Manage Your Education and Skills Funding Document Exchange File Processor Function

The Manage Your Education and Skills Funding (MYESF) Document Exchange File Processor Function is used by the MYESF Document Exchange Data Api to allow the following:

- Perform virus scans on uploaded documents before external providers can view them
- Send requests for notification emails to be sent to supporting email service

## Provider

[The Department for Education](https://www.gov.uk/government/organisations/department-for-education)

## About this project

This project is an Isolated Worker Azure Functions project utilizing an Azure Function App for deployment.

**Note:** The project is currently being updated to be containerised via Docker where the deployment method and target will change, this document will be updated when these changes have been finalised.

# Local Configuration Guide

In order to run the application locally a valid `local.settings.json` file will need to be created in the `Pds.DocumentExchange.FileProcessor.Func` project. Below, and included in the repo, there is `local.settings.example.json` which can be used as a base and populated with the required values, which can be retrieved from the Azure Portal.

## Local Settings (`local.settings.json`)

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "AzureWebJobsDashboard": "UseDevelopmentStorage=true",
    "ServiceBusConnection": "",
    "DocumentExchangeFileProcessorServicesConfiguration:ApiBaseAddress": "",
    "DocumentExchangeFileProcessorServicesConfiguration:AppUri": "",
    "DocumentExchangeFileProcessorServicesConfiguration:Authority": "https://login.microsoftonline.com/",
    "DocumentExchangeFileProcessorServicesConfiguration:ClientId": "",
    "DocumentExchangeFileProcessorServicesConfiguration:ClientSecret": "",
    "DocumentExchangeFileProcessorServicesConfiguration:TenantId": "",
    "FUNCTIONS_EXTENSION_VERSION": "~4",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "PdsApplicationInsights:InstrumentationKey": "",
    "PdsApplicationInsights:Environment": "local"
  }
}
```

### Setting Details

- **`AzureWebJobsStorage`**
  The core application setting used by the Azure Functions and Azure WebJobs runtime to establish a connection to an Azure Storage account.

- **`AzureWebJobsDashboard`**
  The core application setting used by the Azure Functions and Azure WebJobs runtime to establish a connection to an Azure Jobs dashboard.

- **`ServiceBusConnection`**
  The standard application setting used by Azure Functions and Azure WebJobs to securely connect to an Azure Service Bus.

- **`DocumentExchangeFileProcessorServicesConfiguration:ApiBaseAddress`**  
  The base URL endpoint used by a client application to route network requests to the Docex Data API backend.

- **`DocumentExchangeFileProcessorServicesConfiguration:AppUri`**  
  The unique Application ID URI used as the identifier for the protected Docex Data API resource within the Identity Provider.

- **`DocumentExchangeFileProcessorServicesConfiguration:Authority`**  
  The base URL of the Identity Provider responsible for authenticating and issuing tokens for the DocEx Data API client.
 
- **`DocumentExchangeFileProcessorServicesConfiguration:ClientId`**  
  The application (client) ID registered in azure ad.

- **`DocumentExchangeFileProcessorServicesConfiguration:ClientSecret`**  
  The confidential credential used by the application to securely prove its identity to the Identity Provider.

- **`DocumentExchangeFileProcessorServicesConfiguration:TenantId`**  
  The unique identifier for your azure ad tenant.

- **`FUNCTIONS_EXTENSION_VERSION`**  
  The functions extension version number.

- **`FUNCTIONS_WORKER_RUNTIME`**  
  The functions runtime mode.

- **`PdsApplicationInsights:InstrumentationKey`**  
  The key value for Application Insights resource for logging purposes.

- **`PdsApplicationInsights:Environment`**  
  The environment which the app is running on for Application Insights for logging purposes.


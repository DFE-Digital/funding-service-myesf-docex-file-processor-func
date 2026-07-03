# Usage
For running locally, add an app settings file called "local.settings.json" with content as follows:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "AzureWebJobsDashboard": "UseDevelopmentStorage=true",
    "ServiceBusConnection": "[your Azure ServiceBus connection string]"
  }
}
```
# AzureShowcase
This repository has two parts:
1. A .NET 6 web application used to navigate several features of the Azure Platform (Blob Storage, Service Bus, Message Queues,.. ).
2. An Azure Function project used to demonstrate the use of Azure Functions.

For the Web application there are two main features:
- a direct upload to the configured Blob Storage, any file type supported via file picker.
- a direct sending of a message to the configured Service Bus Queue.

For the Azure Functions, there are 2 Functions configured:
- first function is CopyQueueMessageToBlob, which is a queue triggered function that copies the message from the configured queue to the configured blob storage.
- second function is CopyBlobToQueue, which is a blob triggered function that copies the blob to the configured queue.

The diagram in this repository also shows this app's flow. 

## Prerequisites
- Azure Subscription
- Azure Storage Account
- Azure Service Bus Queue

#### Configuration
- Open the remaining solution in Visual Studio 2022
- Open the AzureFunctionsShowcase in a separate project
- Open the appsettings.json file and fill in the required values for the Azure Storage Account and Service Bus Queue. (Both web app and azure functions)
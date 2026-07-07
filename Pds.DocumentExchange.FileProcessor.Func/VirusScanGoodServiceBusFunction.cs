using Microsoft.Azure.Functions.Worker;
using Pds.Core.Logging;
using Pds.DocumentExchange.Data.Services.DTOs;
using Pds.DocumentExchange.Data.Services.Enums;
using Pds.DocumentExchange.FileProcessor.Services.Interfaces;
using Pds.DocumentExchange.FileProcessor.Services.Models;

namespace Pds.DocumentExchange.FileProcessor.Func
{
    /// <summary>
    /// VirusScanGood ServiceBus queue triggered Azure Function.
    /// </summary>
    public class VirusScanGoodServiceBusFunction
    {
        private const string Name = nameof(VirusScanGoodServiceBusFunction);
        private readonly IFileProcessorApiClient _fileProcessorApiClient;
        private readonly ILoggerAdapter<VirusScanGoodServiceBusFunction> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirusScanGoodServiceBusFunction"/> class.
        /// </summary>
        /// <param name="fileProcessorApiClient">The file processor service.</param>
        /// <param name="logger">logger.</param>
        public VirusScanGoodServiceBusFunction(
            IFileProcessorApiClient fileProcessorApiClient,
            ILoggerAdapter<VirusScanGoodServiceBusFunction> logger)
        {
            _fileProcessorApiClient = fileProcessorApiClient;
            _logger = logger;
        }

        /// <summary>
        /// Entry point to the Azure Function.
        /// </summary>
        /// <param name="message">The queue item that triggered this function to run.</param>
        /// <returns>Async Task.</returns>
        [Function("VirusScanGoodServiceBusFunction")]
        public async Task Run(
            [ServiceBusTrigger("VirusScanGood", Connection = "ServiceBusConnection")] ScannedFileQueueMessage message)
        {
            _logger?.LogInformation($"{Name} Started");

            try
            {
                if (message == null)
                {
                    _logger?.LogInformation($"{Name} Message is null");
                    return;
                }

                _logger?.LogInformation($"{Name} Processing message {message}.");

                var documentReference = new DocumentReference
                {
                    BatchIdentifier = message.BatchIdentifier,
                    ParentBatchIdentifier = message.ParentBatchIdentifier,
                    FileName = message.FileName
                };

                switch (message.DocumentDirection)
                {
                    case ExchangeDocumentDirection.PublishedByAgency:
                        await _fileProcessorApiClient.AgencyFilesVirusScanSuccessful(documentReference);
                        break;
                    case ExchangeDocumentDirection.SentByOrganisation:
                        await _fileProcessorApiClient.OrganisationFilesVirusScanSuccessful(documentReference);
                        break;
                    default:
                        throw new Exception($"{Name} documentDirection must be a valid value. {message.DocumentDirection}");
                }

                _logger?.LogInformation($"{Name} Virus scan successful for {message}.");
            }
            catch (Exception exception)
            {
                _logger?.LogError(exception, $"{Name} Error occurred while processing message {message}");
                throw;
            }
            finally
            {
                _logger?.LogInformation($"{Name} Completed");
            }
        }
    }
}
using Microsoft.Azure.Functions.Worker;
using Pds.Core.Logging;
using Pds.DocumentExchange.Data.Services.DTOs;
using Pds.DocumentExchange.Data.Services.Enums;
using Pds.DocumentExchange.FileProcessor.Services.Interfaces;
using Pds.DocumentExchange.FileProcessor.Services.Models;

namespace Pds.DocumentExchange.FileProcessor.Func
{
    /// <summary>
    /// VirusScanBad ServiceBus queue triggered Azure Function.
    /// </summary>
    public class VirusScanBadServiceBusFunction
    {
        private const string Name = nameof(VirusScanBadServiceBusFunction);
        private readonly IFileProcessorApiClient _fileProcessorApiClient;
        private readonly ILoggerAdapter<VirusScanBadServiceBusFunction> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirusScanBadServiceBusFunction"/> class.
        /// </summary>
        /// <param name="fileProcessorApiClient">The file processor service.</param>
        /// <param name="logger">logger.</param>
        public VirusScanBadServiceBusFunction(
            IFileProcessorApiClient fileProcessorApiClient,
            ILoggerAdapter<VirusScanBadServiceBusFunction> logger)
        {
            _fileProcessorApiClient = fileProcessorApiClient;
            _logger = logger;
        }

        /// <summary>
        /// Entry point to the Azure Function.
        /// </summary>
        /// <param name="message">The queue item that triggered this function to run.</param>
        /// <returns>Async Task.</returns>
        [Function("VirusScanBadServiceBusFunction")]
        public async Task Run(
            [ServiceBusTrigger("VirusScanBad", Connection = "ServiceBusConnection")]ScannedFileQueueMessage message)
        {
            _logger?.LogInformation($"{Name} Started");

            try
            {
                if (message == null)
                {
                    _logger?.LogInformation($"{Name} Message is null");
                    return;
                }

                _logger?.LogInformation(
                $"{Name} Processing message {message}.");

                var documentReference = new DocumentReference
                {
                    BatchIdentifier = message.BatchIdentifier,
                    ParentBatchIdentifier = message.ParentBatchIdentifier,
                    FileName = message.FileName
                };

                switch (message.DocumentDirection)
                {
                    case ExchangeDocumentDirection.PublishedByAgency:
                        await _fileProcessorApiClient.BatchesVirusScanFail(documentReference);
                        break;
                    case ExchangeDocumentDirection.SentByOrganisation:
                        await _fileProcessorApiClient.UploadBatchesVirusScanFail(documentReference);
                        break;
                    default:
                        throw new Exception($"{Name} documentDirection must be a valid value. {message.DocumentDirection}");
                }

                _logger?.LogInformation($"{Name} The virus scan found a threat in this batch! message details: {message}");
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
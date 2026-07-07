using Microsoft.Azure.Functions.Worker;
using Pds.Core.Logging;
using Pds.DocumentExchange.Data.Services.DTOs;
using Pds.DocumentExchange.Data.Services.Enums;
using Pds.DocumentExchange.FileProcessor.Services.Interfaces;

namespace Pds.DocumentExchange.FileProcessor.Func
{
    /// <summary>
    /// VirusScanGood ServiceBus queue triggered Azure Function.
    /// </summary>
    public class ReadyForEmailServiceBusFunction
    {
        private const string Name = nameof(ReadyForEmailServiceBusFunction);
        private readonly IFileProcessorApiClient _fileProcessorApiClient;
        private readonly ILoggerAdapter<ReadyForEmailServiceBusFunction> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadyForEmailServiceBusFunction"/> class.
        /// </summary>
        /// <param name="fileProcessorApiClient">The file processor service.</param>
        /// <param name="logger">logger.</param>
        public ReadyForEmailServiceBusFunction(IFileProcessorApiClient fileProcessorApiClient, ILoggerAdapter<ReadyForEmailServiceBusFunction> logger)
        {
            _fileProcessorApiClient = fileProcessorApiClient;
            _logger = logger;
        }

        /// <summary>
        /// Entry point to the Azure Function.
        /// </summary>
        /// <param name="message">The queue item that triggered this function to run.</param>
        /// <returns>Async Task.</returns>
        [Function("ReadyForEmailServiceBusFunction")]
        public async Task Run(
            [ServiceBusTrigger("ReadyForEmail", Connection = "ServiceBusConnection")] ScannedBatchQueueMessage message)
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

                switch (message.DocumentDirection)
                {
                    case ExchangeDocumentDirection.PublishedByAgency:
                        await _fileProcessorApiClient.AgencyPublishCompleteTask(message.ParentBatchIdentifier);
                        break;
                    case ExchangeDocumentDirection.SentByOrganisation:
                        await _fileProcessorApiClient.OrganisationUploadCompleteTask(message.ParentBatchIdentifier);
                        break;
                    default:
                        throw new Exception($"{Name} documentDirection must be a valid value. {message.DocumentDirection}");
                }

                _logger?.LogInformation($"{Name} Processed message {message}.");
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
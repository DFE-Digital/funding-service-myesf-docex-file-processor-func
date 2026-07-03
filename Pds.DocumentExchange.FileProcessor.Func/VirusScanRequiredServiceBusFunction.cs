using Microsoft.Azure.Functions.Worker;
using Pds.Core.Logging;
using Pds.DocumentExchange.Data.Services.DTOs;
using Pds.DocumentExchange.Data.Services.Enums;
using Pds.DocumentExchange.FileProcessor.Services.Interfaces;

namespace Pds.DocumentExchange.FileProcessor.Func
{
    /// <summary>
    /// VirusScanRequired ServiceBus queue triggered Azure Function.
    /// </summary>
    public class VirusScanRequiredServiceBusFunction
    {
        private const string Name = nameof(VirusScanRequiredServiceBusFunction);
        private readonly IFileProcessorApiClient _fileProcessorApiClient;
        private readonly ILoggerAdapter<VirusScanRequiredServiceBusFunction> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirusScanRequiredServiceBusFunction"/> class.
        /// </summary>
        /// <param name="fileProcessorApiClient">The file processor service.</param>
        /// <param name="logger">logger.</param>
        public VirusScanRequiredServiceBusFunction(
            IFileProcessorApiClient fileProcessorApiClient,
            ILoggerAdapter<VirusScanRequiredServiceBusFunction> logger)
        {
            _fileProcessorApiClient = fileProcessorApiClient;
            _logger = logger;
        }

        /// <summary>
        /// Entry point to the Azure Function.
        /// </summary>
        /// <param name="message">The queue item that triggered this function to run.</param>
        /// <returns>Async Task.</returns>
        [Function("VirusScanRequiredServiceBusFunction")]
        public async Task Run(
            [ServiceBusTrigger("VirusScanRequired", Connection = "ServiceBusConnection")]VirusScanRequestMessage message)
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
                        await _fileProcessorApiClient.AgencyBatchDocumentVirusScan(message.ParentBatchIdentifier);
                        break;
                    case ExchangeDocumentDirection.SentByOrganisation:
                        await _fileProcessorApiClient.OrganisationUploadDocumentVirusScan(message.ParentBatchIdentifier);
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
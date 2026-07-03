using Microsoft.Extensions.Options;
using Pds.Core.ApiClient;
using Pds.Core.ApiClient.Interfaces;
using Pds.Core.Logging;
using Pds.DocumentExchange.FileProcessor.Services.DTOs.Configuration;
using Pds.DocumentExchange.FileProcessor.Services.Interfaces;
using Pds.DocumentExchange.FileProcessor.Services.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pds.DocumentExchange.FileProcessor.Services.Implementations
{
    /// <inheritdoc cref="IFileProcessorApiClient" />
    public class FileProcessorApiClient : BaseApiClient<DocumentExchangeFileProcessorServicesConfiguration>, IFileProcessorApiClient
    {
        private readonly ILoggerAdapter<FileProcessorApiClient> _logger;
        private readonly DocumentExchangeFileProcessorServicesConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileProcessorApiClient"/> class.
        /// </summary>
        /// <param name="authenticationService">The authentication service.</param>
        /// <param name="httpClient">httpClient.</param>
        /// <param name="configurationOptions">configurationOptions.</param>
        /// <param name="logger">logger.</param>
        public FileProcessorApiClient(
            IAuthenticationService<DocumentExchangeFileProcessorServicesConfiguration> authenticationService,
            HttpClient httpClient,
            IOptions<DocumentExchangeFileProcessorServicesConfiguration> configurationOptions,
            ILoggerAdapter<FileProcessorApiClient> logger) : base(authenticationService, httpClient, configurationOptions)
        {
            _configuration = configurationOptions.Value;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task AgencyBatchDocumentVirusScan(string batchIdentifier)
        {
            var uri = $"/api/agency/batches/{batchIdentifier}/perform-virus-scan";
            await Post(uri, batchIdentifier);
        }

        /// <inheritdoc />
        public async Task OrganisationUploadDocumentVirusScan(string batchIdentifier)
        {
            var uri = $"/api/upload/perform-virus-scan/batches/{batchIdentifier}/perform-virus-scan";
            await Post(uri, batchIdentifier);
        }

        /// <inheritdoc />
        public async Task OrganisationFilesVirusScanSuccessful(DocumentReference documentReference)
        {
            await Post("/api/exchange/batches/organisation-files-virus-scan-successful", documentReference);
        }

        /// <inheritdoc />
        public async Task AgencyFilesVirusScanSuccessful(DocumentReference documentReference)
        {
            await Post("/api/exchange/batches/agency-files-virus-scan-successful", documentReference);
        }

        /// <inheritdoc />
        public async Task AgencyPublishCompleteTask(string parentBatchIdentifier)
        {
            await Post("/api/notification/agency-publish-complete", parentBatchIdentifier);
        }

        /// <inheritdoc />
        public async Task OrganisationUploadCompleteTask(string parentBatchIdentifier)
        {
            await Post("/api/notification/organisation-upload-complete", parentBatchIdentifier);
        }

        /// <inheritdoc />
        public async Task BatchesVirusScanFail(DocumentReference documentReference)
        {
            await Post("/api/agency/batches/virus-scan-fail", documentReference);
        }

        /// <inheritdoc />
        public async Task UploadBatchesVirusScanFail(DocumentReference documentReference)
        {
            await Post("/api/upload/virus-scan-fail/batches/virus-scan-fail", documentReference);
        }
    }
}
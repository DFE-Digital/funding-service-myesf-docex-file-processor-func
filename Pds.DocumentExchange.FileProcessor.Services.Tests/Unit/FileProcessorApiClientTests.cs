using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Pds.Core.ApiClient.Exceptions;
using Pds.Core.ApiClient.Interfaces;
using Pds.Core.Logging;
using Pds.DocumentExchange.FileProcessor.Services.DTOs.Configuration;
using Pds.DocumentExchange.FileProcessor.Services.Implementations;
using Pds.DocumentExchange.FileProcessor.Services.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Pds.DocumentExchange.FileProcessor.Services.Tests.Unit
{
    [TestClass]
    public class FileProcessorApiClientTests
    {
        private const string TestFakeAccessToken = "AccessToken";
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        private Mock<ILoggerAdapter<FileProcessorApiClient>> _mockLogger;
        private FileProcessorApiClient _apiClient;

        [TestMethod, TestCategory("Unit")]
        public async Task AgencyBatchDocumentVirusScan_ReturnsSuccess()
        {
            // Arrange
            Setup((HttpStatusCode)StatusCodes.Status200OK);

            // Act
            await _apiClient.AgencyBatchDocumentVirusScan("batchIdentifier");

            // Assert
            _mockHttpMessageHandler.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(r => r.RequestUri.Equals("https://DummyUrl/api/agency/batches/batchIdentifier/perform-virus-scan")), ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod, TestCategory("Unit")]
        public async Task AgencyBatchDocumentVirusScan_ReturnsHttpRequestException()
        {
            // Arrange
            Setup((HttpStatusCode)StatusCodes.Status500InternalServerError);

            // Act
            Func<Task> act = async () => await _apiClient.AgencyBatchDocumentVirusScan("batchIdentifier");

            // Assert
            await act.Should().ThrowAsync<ApiGeneralException>().Where(e => e.ResponseStatusCode.Equals((HttpStatusCode)StatusCodes.Status500InternalServerError));
        }

        [TestMethod, TestCategory("Unit")]
        public async Task OrganisationUploadDocumentVirusScan_ReturnsSuccess()
        {
            // Arrange
            Setup((HttpStatusCode)StatusCodes.Status200OK);

            // Act
            await _apiClient.OrganisationUploadDocumentVirusScan("batchIdentifier");

            // Assert
            _mockHttpMessageHandler.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(r => r.RequestUri.Equals("https://DummyUrl/api/upload/perform-virus-scan/batches/batchIdentifier/perform-virus-scan")), ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod, TestCategory("Unit")]
        public async Task OrganisationUploadDocumentVirusScan_ReturnsHttpRequestException()
        {
            // Arrange
            Setup((HttpStatusCode)StatusCodes.Status500InternalServerError);

            // Act
            Func<Task> act = async () => await _apiClient.OrganisationUploadDocumentVirusScan("batchIdentifier");

            // Assert
            await act.Should().ThrowAsync<ApiGeneralException>().Where(e => e.ResponseStatusCode.Equals((HttpStatusCode)StatusCodes.Status500InternalServerError));
        }

        [TestMethod, TestCategory("Unit")]
        public async Task OrganisationFilesVirusScanSuccessful_ReturnsSuccess()
        {
            // Arrange
            Setup((HttpStatusCode)StatusCodes.Status200OK);

            // Act
            await _apiClient.OrganisationFilesVirusScanSuccessful(new DocumentReference());

            // Assert
            _mockHttpMessageHandler.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(r => r.RequestUri.Equals("https://DummyUrl/api/exchange/batches/organisation-files-virus-scan-successful")), ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod, TestCategory("Unit")]
        public async Task OrganisationFilesVirusScanSuccessful_ReturnsHttpRequestException()
        {
            // Arrange
            Setup((HttpStatusCode)StatusCodes.Status500InternalServerError);

            // Act
            Func<Task> act = async () => await _apiClient.OrganisationFilesVirusScanSuccessful(new DocumentReference());

            // Assert
            await act.Should().ThrowAsync<ApiGeneralException>().Where(e => e.ResponseStatusCode.Equals((HttpStatusCode)StatusCodes.Status500InternalServerError));
        }

        [TestMethod, TestCategory("Unit")]
        public async Task AgencyFilesVirusScanSuccessful_ReturnsSuccess()
        {
            // Arrange
            Setup((HttpStatusCode)StatusCodes.Status200OK);

            // Act
            await _apiClient.AgencyFilesVirusScanSuccessful(new DocumentReference());

            // Assert
            _mockHttpMessageHandler.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(r => r.RequestUri.Equals("https://DummyUrl/api/exchange/batches/agency-files-virus-scan-successful")), ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod, TestCategory("Unit")]
        public async Task AgencyFilesVirusScanSuccessful_ReturnsHttpRequestException()
        {
            // Arrange
            Setup((HttpStatusCode)StatusCodes.Status500InternalServerError);

            // Act
            Func<Task> act = async () => await _apiClient.AgencyFilesVirusScanSuccessful(new DocumentReference());

            // Assert
            await act.Should().ThrowAsync<ApiGeneralException>().Where(e => e.ResponseStatusCode.Equals((HttpStatusCode)StatusCodes.Status500InternalServerError));
        }

        [TestMethod, TestCategory("Unit")]
        public async Task OrganisationUploadCompleteTask_ReturnsSuccess()
        {
            // Arrange
            Setup((HttpStatusCode)StatusCodes.Status200OK);

            // Act
            await _apiClient.OrganisationUploadCompleteTask("parentBatchIdentifier");

            // Assert
            _mockHttpMessageHandler.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(r => r.RequestUri.Equals("https://DummyUrl/api/notification/organisation-upload-complete")), ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod, TestCategory("Unit")]
        public async Task OrganisationUploadCompleteTask_ReturnsHttpRequestException()
        {
            // Arrange
            Setup((HttpStatusCode)StatusCodes.Status500InternalServerError);

            // Act
            Func<Task> act = async () => await _apiClient.OrganisationUploadCompleteTask("parentBatchIdentifier");

            // Assert
            await act.Should().ThrowAsync<ApiGeneralException>().Where(e => e.ResponseStatusCode.Equals((HttpStatusCode)StatusCodes.Status500InternalServerError));
        }

        [TestMethod, TestCategory("Unit")]
        public async Task AgencyUploadCompleteTask_ReturnsSuccess()
        {
            // Arrange
            Setup((HttpStatusCode)StatusCodes.Status200OK);

            // Act
            await _apiClient.AgencyPublishCompleteTask("parentBatchIdentifier");

            // Assert
            _mockHttpMessageHandler.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(r => r.RequestUri.Equals("https://DummyUrl/api/notification/agency-publish-complete")), ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod, TestCategory("Unit")]
        public async Task AgencyUploadCompleteTask_ReturnsHttpRequestException()
        {
            // Arrange
            Setup((HttpStatusCode)StatusCodes.Status500InternalServerError);

            // Act
            Func<Task> act = async () => await _apiClient.AgencyPublishCompleteTask("parentBatchIdentifier");

            // Assert
            await act.Should().ThrowAsync<ApiGeneralException>().Where(e => e.ResponseStatusCode.Equals((HttpStatusCode)StatusCodes.Status500InternalServerError));
        }

        [TestMethod, TestCategory("Unit")]
        public async Task BatchesVirusScanFail_ReturnsSuccess()
        {
            // Arrange
            Setup((HttpStatusCode)StatusCodes.Status200OK);

            // Act
            await _apiClient.BatchesVirusScanFail(new DocumentReference());

            // Assert
            _mockHttpMessageHandler.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(r => r.RequestUri.Equals("https://DummyUrl/api/agency/batches/virus-scan-fail")), ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod, TestCategory("Unit")]
        public async Task BatchesVirusScanFail_ReturnsHttpRequestException()
        {
            // Arrange
            Setup((HttpStatusCode)StatusCodes.Status500InternalServerError);

            // Act
            Func<Task> act = async () => await _apiClient.BatchesVirusScanFail(new DocumentReference());

            // Assert
            await act.Should().ThrowAsync<ApiGeneralException>().Where(e => e.ResponseStatusCode.Equals((HttpStatusCode)StatusCodes.Status500InternalServerError));
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UploadBatchesVirusScanFail_ReturnsSuccess()
        {
            // Arrange
            Setup((HttpStatusCode)StatusCodes.Status200OK);

            // Act
            await _apiClient.UploadBatchesVirusScanFail(new DocumentReference());

            // Assert
            _mockHttpMessageHandler.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(r => r.RequestUri.Equals("https://DummyUrl/api/upload/virus-scan-fail/batches/virus-scan-fail")), ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UploadBatchesVirusScanFail_ReturnsHttpRequestException()
        {
            // Arrange
            Setup((HttpStatusCode)StatusCodes.Status500InternalServerError);

            // Act
            Func<Task> act = async () => await _apiClient.UploadBatchesVirusScanFail(new DocumentReference());

            // Assert
            await act.Should().ThrowAsync<ApiGeneralException>().Where(e => e.ResponseStatusCode.Equals((HttpStatusCode)StatusCodes.Status500InternalServerError));
        }

        private void Setup(HttpStatusCode statusCode)
        {
            var mockFactory = new Mock<IHttpClientFactory>();
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode
                }).Verifiable();

            var client = new HttpClient(_mockHttpMessageHandler.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var configurationOptions = Options.Create(
                new DocumentExchangeFileProcessorServicesConfiguration
                {
                    ApiBaseAddress = "https://DummyUrl"
                });
            var mockAuthenticationService = new Mock<IAuthenticationService<DocumentExchangeFileProcessorServicesConfiguration>>(MockBehavior.Strict);
            mockAuthenticationService.Setup(x => x.GetAccessTokenForAAD()).Returns(Task.FromResult(TestFakeAccessToken));

            _mockLogger = new Mock<ILoggerAdapter<FileProcessorApiClient>>();

            _apiClient = new FileProcessorApiClient(mockAuthenticationService.Object, client, configurationOptions, _mockLogger.Object);
        }
    }
}
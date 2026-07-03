using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Logging;
using Pds.DocumentExchange.Data.Services.DTOs;
using Pds.DocumentExchange.Data.Services.Enums;
using System;
using System.Threading.Tasks;

namespace Pds.DocumentExchange.FileProcessor.Func.Tests.Unit
{
    [TestClass]
    public class VirusScanRequiredServiceBusFunctionTests : BaseServiceBusFunctionTests
    {
        private static readonly string Name = nameof(VirusScanRequiredServiceBusFunction);
        private readonly string _startedMessage = $"{Name} Started";
        private readonly string _nullMessage = $"{Name} Message is null";
        private readonly string _completedMessage = $"{Name} Completed";
        private readonly Mock<ILoggerAdapter<VirusScanRequiredServiceBusFunction>> _mockLogger;
        private readonly VirusScanRequiredServiceBusFunction _virusScanRequiredServiceBusFunction;
        private VirusScanRequestMessage _message;

        public VirusScanRequiredServiceBusFunctionTests()
        {
            _message = new VirusScanRequestMessage
            {
                ParentBatchIdentifier = "parentBatchIdentifier",
                DocumentDirection = ExchangeDocumentDirection.PublishedByAgency
            };

            _mockLogger = new Mock<ILoggerAdapter<VirusScanRequiredServiceBusFunction>>();
            _virusScanRequiredServiceBusFunction = new VirusScanRequiredServiceBusFunction(MockFileProcessingApiClient.Object, _mockLogger.Object);
        }

        [TestMethod, TestCategory("Unit")]
        public void VirusScanRequiredServiceBusFunction_WhenMessageIsNull_LogsNullMessage()
        {
            // Arrange
            _message = null;

            // Act
            Func<Task> act = async () => { await _virusScanRequiredServiceBusFunction.Run(_message); };

            // Assert
            act.Should().NotThrowAsync();

            _mockLogger.Verify(x => x.LogInformation(_startedMessage), Times.Once);
            _mockLogger.Verify(x => x.LogInformation(_nullMessage), Times.Once);
            _mockLogger.Verify(x => x.LogInformation(_completedMessage), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public void VirusScanRequiredServiceBusFunction_WhenDocumentDirectionIsPublishByAgency_FunctionProcessorDoesNotThrowException()
        {
            // Arrange
            _message.DocumentDirection = ExchangeDocumentDirection.PublishedByAgency;
            var processedMessage = GetProcessMessage();
            var processingMessage = GetProcessingMessage();

            MockFileProcessingApiClient
                .Setup(e => e.AgencyBatchDocumentVirusScan(_message.ParentBatchIdentifier))
                .Verifiable();

            // Act
            Func<Task> act = async () => { await _virusScanRequiredServiceBusFunction.Run(_message); };

            // Assert
            act.Should().NotThrowAsync();

            MockFileProcessingApiClient.Verify();

            VerifyLogging(_mockLogger, _startedMessage, processingMessage, processedMessage, _completedMessage);
        }

        [TestMethod, TestCategory("Unit")]
        public void VirusScanRequiredServiceBusFunction_WhenDocumentDirectionIsPublishByAgencyAndApiThrowsException_FunctionProcessorLogsAndThrowsException()
        {
            // Arrange
            _message.DocumentDirection = ExchangeDocumentDirection.PublishedByAgency;
            var processingMessage = GetProcessingMessage();
            var errorMessage = GetErrorMessage();
            MockFileProcessingApiClient
                .Setup(e => e.AgencyBatchDocumentVirusScan(_message.ParentBatchIdentifier))
                .Throws(Exception)
                .Verifiable();

            // Act
            Func<Task> act = async () => { await _virusScanRequiredServiceBusFunction.Run(_message); };

            // Assert
            act.Should().ThrowAsync<Exception>();
            MockFileProcessingApiClient.Verify();
            VerifyLoggingAndException(_mockLogger, _startedMessage, processingMessage, errorMessage, _completedMessage);
        }

        [TestMethod, TestCategory("Unit")]
        public void VirusScanRequiredServiceBusFunction_WhenDocumentDirectionIsSentByOrganisation_FunctionProcessorDoesNotThrowException()
        {
            // Arrange
            _message.DocumentDirection = ExchangeDocumentDirection.SentByOrganisation;
            var processedMessage = GetProcessMessage();
            var processingMessage = GetProcessingMessage();
            MockFileProcessingApiClient
                .Setup(e => e.OrganisationUploadDocumentVirusScan(_message.ParentBatchIdentifier))
                .Verifiable();

            // Act
            Func<Task> act = async () => { await _virusScanRequiredServiceBusFunction.Run(_message); };

            // Assert
            act.Should().NotThrowAsync();

            MockFileProcessingApiClient.Verify();
            VerifyLogging(_mockLogger, _startedMessage, processingMessage, processedMessage, _completedMessage);
        }

        [TestMethod, TestCategory("Unit")]
        public void VirusScanRequiredServiceBusFunction_WhenDocumentDirectionIsSentByOrganisation_FunctionProcessorLogsAndThrowsException()
        {
            // Arrange
            _message.DocumentDirection = ExchangeDocumentDirection.SentByOrganisation;
            var processingMessage = GetProcessingMessage();
            var errorMessage = GetErrorMessage();

            MockFileProcessingApiClient
                .Setup(e => e.OrganisationUploadDocumentVirusScan(_message.ParentBatchIdentifier))
                .Throws(Exception)
                .Verifiable();

            // Act
            Func<Task> act = async () => { await _virusScanRequiredServiceBusFunction.Run(_message); };

            // Assert
            act.Should().ThrowAsync<Exception>();
            MockFileProcessingApiClient.Verify();

            VerifyLoggingAndException(_mockLogger, _startedMessage, processingMessage, errorMessage, _completedMessage);
        }

        [TestMethod, TestCategory("Unit")]
        public void VirusScanRequiredServiceBusFunction_WhenDocumentDirectionIsInvalid_ThrowsExceptionAndLogsError()
        {
            // Arrange
            _message.DocumentDirection = (ExchangeDocumentDirection)2;

            var exceptionMessage = $"{Name} documentDirection must be a valid value. {(ExchangeDocumentDirection)2}";
            var processingMessage = GetProcessingMessage();
            var errorMessage = GetErrorMessage();
            Exception = new Exception(exceptionMessage);

            // Act
            Func<Task> act = async () => { await _virusScanRequiredServiceBusFunction.Run(_message); };

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage(exceptionMessage);

            _mockLogger.Verify(x => x.LogInformation(_startedMessage), Times.Once);
            _mockLogger.Verify(x => x.LogInformation(processingMessage), Times.Once);

            _mockLogger.Verify(ml => ml.LogError(It.Is<Exception>(e => e.Message == exceptionMessage), errorMessage), Times.Once);

            _mockLogger.Verify(x => x.LogInformation(_completedMessage), Times.Once);
        }

        private string GetProcessMessage()
        {
            return $"{Name} The virus scan found a threat in this batch! message details: {_message}";
        }

        private string GetProcessingMessage()
        {
            return $"{Name} Processing message {_message}.";
        }

        private string GetErrorMessage()
        {
            return $"{Name} Error occurred while processing message {_message}";
        }
    }
}
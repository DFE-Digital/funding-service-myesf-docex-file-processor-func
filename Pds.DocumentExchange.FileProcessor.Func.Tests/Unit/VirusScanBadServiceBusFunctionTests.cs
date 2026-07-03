using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Logging;
using Pds.DocumentExchange.Data.Services.DTOs;
using Pds.DocumentExchange.Data.Services.Enums;
using Pds.DocumentExchange.FileProcessor.Services.Models;
using System;
using System.Threading.Tasks;

namespace Pds.DocumentExchange.FileProcessor.Func.Tests.Unit
{
    [TestClass]
    public class VirusScanBadServiceBusFunctionTests : BaseServiceBusFunctionTests
    {
        private static readonly string Name = nameof(VirusScanBadServiceBusFunction);
        private readonly Mock<ILoggerAdapter<VirusScanBadServiceBusFunction>> _mockLogger;
        private readonly VirusScanBadServiceBusFunction _virusScanBadServiceBusFunction;
        private readonly string _startedMessage = $"{Name} Started";
        private readonly string _nullMessage = $"{Name} Message is null";
        private readonly string _completedMessage = $"{Name} Completed";
        private ScannedFileQueueMessage _message;

        public VirusScanBadServiceBusFunctionTests()
        {
            _message = new ScannedFileQueueMessage
            {
                BatchIdentifier = "batchIdentifier",
                ParentBatchIdentifier = "parentBatchIdentifier",
                FileName = "fileName"
            };

            DocumentReference = new DocumentReference
            {
                BatchIdentifier = _message.BatchIdentifier,
                ParentBatchIdentifier = _message.ParentBatchIdentifier,
                FileName = _message.FileName
            };

            _mockLogger = new Mock<ILoggerAdapter<VirusScanBadServiceBusFunction>>();

            _virusScanBadServiceBusFunction = new VirusScanBadServiceBusFunction(MockFileProcessingApiClient.Object, _mockLogger.Object);
        }

        [TestMethod, TestCategory("Unit")]
        public void VirusScanBadServiceBusFunction_WhenMessageIsNull_LogsNullMessage()
        {
            // Arrange
            _message = null;

            // Act
            Func<Task> act = async () => { await _virusScanBadServiceBusFunction.Run(_message); };

            // Assert
            act.Should().NotThrowAsync();

            _mockLogger.Verify(x => x.LogInformation(_startedMessage), Times.Once);
            _mockLogger.Verify(x => x.LogInformation(_nullMessage), Times.Once);
            _mockLogger.Verify(x => x.LogInformation(_completedMessage), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public void VirusScanBadServiceBusFunction_WhenDocumentDirectionIsPublishByAgency_FunctionProcessorDoesNotThrowException()
        {
            // Arrange
            _message.DocumentDirection = ExchangeDocumentDirection.PublishedByAgency;

            MockFileProcessingApiClient
                .Setup(e => e.BatchesVirusScanFail(DocumentReferenceParameter))
                .Verifiable();

            // Act
            Func<Task> act = async () => { await _virusScanBadServiceBusFunction.Run(_message); };

            // Assert
            act.Should().NotThrowAsync();

            MockFileProcessingApiClient.Verify();
            var processedMessage = GetProcessMessage();
            var processingMessage = GetProcessingMessage();

            VerifyLogging(_mockLogger, _startedMessage, processingMessage, processedMessage, _completedMessage);
        }

        [TestMethod, TestCategory("Unit")]
        public void VirusScanBadServiceBusFunction_WhenDocumentDirectionIsPublishByAgencyAndApiThrowsException_FunctionProcessorLogsAndThrowsException()
        {
            // Arrange
            _message.DocumentDirection = ExchangeDocumentDirection.PublishedByAgency;

            MockFileProcessingApiClient
                .Setup(e => e.BatchesVirusScanFail(DocumentReferenceParameter))
                .Throws(Exception)
                .Verifiable();
            var processingMessage = GetProcessingMessage();
            var errorMessage = GetErrorMessage();

            // Act
            Func<Task> act = async () => { await _virusScanBadServiceBusFunction.Run(_message); };

            // Assert
            act.Should().ThrowAsync<Exception>();
            MockFileProcessingApiClient.Verify();

            VerifyLoggingAndException(_mockLogger, _startedMessage, processingMessage, errorMessage, _completedMessage);
        }

        [TestMethod, TestCategory("Unit")]
        public void VirusScanBadServiceBusFunction_WhenDocumentDirectionIsSentByOrganisation_FunctionProcessorDoesNotThrowException()
        {
            // Arrange
            _message.DocumentDirection = ExchangeDocumentDirection.SentByOrganisation;

            MockFileProcessingApiClient
                .Setup(e => e.UploadBatchesVirusScanFail(DocumentReferenceParameter))
                .Verifiable();
            var processedMessage = GetProcessMessage();
            var processingMessage = GetProcessingMessage();

            // Act
            Func<Task> act = async () => { await _virusScanBadServiceBusFunction.Run(_message); };

            // Assert
            act.Should().NotThrowAsync();

            MockFileProcessingApiClient.Verify();

            VerifyLogging(_mockLogger, _startedMessage, processingMessage, processedMessage, _completedMessage);
        }

        [TestMethod, TestCategory("Unit")]
        public void VirusScanBadServiceBusFunction_WhenDocumentDirectionIsSentByOrganisationAndApiThrowsException_FunctionProcessorLogsAndThrowsException()
        {
            // Arrange
            _message.DocumentDirection = ExchangeDocumentDirection.SentByOrganisation;

            MockFileProcessingApiClient
                .Setup(e => e.UploadBatchesVirusScanFail(DocumentReferenceParameter))
                .Throws(Exception)
                .Verifiable();
            var processingMessage = GetProcessingMessage();
            var errorMessage = GetErrorMessage();

            // Act
            Func<Task> act = async () => { await _virusScanBadServiceBusFunction.Run(_message); };

            // Assert
            act.Should().ThrowAsync<Exception>();
            MockFileProcessingApiClient.Verify();

            VerifyLoggingAndException(_mockLogger, _startedMessage, processingMessage, errorMessage, _completedMessage);
        }

        [TestMethod, TestCategory("Unit")]
        public void VirusScanBadServiceBusFunction_WhenDocumentDirectionIsInvalid_ThrowsExceptionAndLogsError()
        {
            // Arrange
            _message.DocumentDirection = (ExchangeDocumentDirection)2;

            var exceptionMessage = $"{Name} documentDirection must be a valid value. {(ExchangeDocumentDirection)2}";

            Exception = new Exception(exceptionMessage);
            var processingMessage = GetProcessingMessage();
            var errorMessage = GetErrorMessage();

            // Act
            Func<Task> act = async () => { await _virusScanBadServiceBusFunction.Run(_message); };

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
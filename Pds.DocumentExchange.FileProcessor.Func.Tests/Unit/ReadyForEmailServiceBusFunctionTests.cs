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
    public class ReadyForEmailServiceBusFunctionTests : BaseServiceBusFunctionTests
    {
        private const string Name = nameof(ReadyForEmailServiceBusFunction);
        private readonly Mock<ILoggerAdapter<ReadyForEmailServiceBusFunction>> _mockLogger;
        private readonly ReadyForEmailServiceBusFunction _readyForEmailServiceBusFunction;
        private readonly string _startedMessage = $"{Name} Started";
        private readonly string _nullMessage = $"{Name} Message is null";
        private readonly string _completedMessage = $"{Name} Completed";
        private ScannedBatchQueueMessage _message;

        public ReadyForEmailServiceBusFunctionTests()
        {
            _message = new ScannedBatchQueueMessage
            {
                ParentBatchIdentifier = "parentBatchIdentifier",
                BatchIdentifier = "batchIdentifier"
            };

            _mockLogger = new Mock<ILoggerAdapter<ReadyForEmailServiceBusFunction>>();
            _readyForEmailServiceBusFunction = new ReadyForEmailServiceBusFunction(MockFileProcessingApiClient.Object, _mockLogger.Object);
        }

        [TestMethod, TestCategory("Unit")]
        public void ReadyForEmailServiceBusFunction_WhenMessageIsNull_LogsNullMessage()
        {
            // Arrange
            _message = null;

            // Act
            Func<Task> act = async () => { await _readyForEmailServiceBusFunction.Run(_message); };

            // Assert
            act.Should().NotThrowAsync();

            _mockLogger.Verify(x => x.LogInformation(_startedMessage), Times.Once);
            _mockLogger.Verify(x => x.LogInformation(_nullMessage), Times.Once);
            _mockLogger.Verify(x => x.LogInformation(_completedMessage), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public void ReadyForEmailServiceBusFunction_WhenDocumentDirectionIsPublishByAgency_FunctionProcessorDoesNotThrowException()
        {
            // Arrange
            _message.DocumentDirection = ExchangeDocumentDirection.PublishedByAgency;

            MockFileProcessingApiClient
                .Setup(e => e.AgencyPublishCompleteTask(_message.ParentBatchIdentifier))
                .Verifiable();

            var processingMessage = GetProcessingMessage();
            var processedMessage = GetProcessMessage();

            // Act
            Func<Task> act = async () => { await _readyForEmailServiceBusFunction.Run(_message); };

            // Assert
            act.Should().NotThrowAsync();

            MockFileProcessingApiClient.Verify();

            VerifyLogging(_mockLogger, _startedMessage, processingMessage, processedMessage, _completedMessage);
        }

        [TestMethod, TestCategory("Unit")]
        public void ReadyForEmailServiceBusFunction_WhenDocumentDirectionIsPublishByAgencyAndApiThrowsException_FunctionProcessorLogsAndThrowsException()
        {
            // Arrange
            _message.DocumentDirection = ExchangeDocumentDirection.PublishedByAgency;

            MockFileProcessingApiClient
                .Setup(e => e.AgencyPublishCompleteTask(_message.ParentBatchIdentifier))
                .Throws(Exception)
                .Verifiable();

            var processingMessage = GetProcessingMessage();
            var errorMessage = GetErrorMessage();

            // Act
            Func<Task> act = async () => { await _readyForEmailServiceBusFunction.Run((ScannedBatchQueueMessage)_message); };

            // Assert
            act.Should().ThrowAsync<Exception>();
            MockFileProcessingApiClient.Verify();

            VerifyLoggingAndException(_mockLogger, _startedMessage, processingMessage, errorMessage, _completedMessage);
        }

        [TestMethod, TestCategory("Unit")]
        public void ReadyForEmailServiceBusFunction_WhenDocumentDirectionIsSentByOrganisation_FunctionProcessorDoesNotThrowException()
        {
            // Arrange
            _message.DocumentDirection = ExchangeDocumentDirection.SentByOrganisation;

            MockFileProcessingApiClient
                .Setup(e => e.OrganisationUploadCompleteTask(_message.ParentBatchIdentifier))
                .Verifiable();

            var processingMessage = GetProcessingMessage();
            var processedMessage = GetProcessMessage();

            // Act
            Func<Task> act = async () => { await _readyForEmailServiceBusFunction.Run(_message); };

            // Assert
            act.Should().NotThrowAsync();

            MockFileProcessingApiClient.Verify();

            VerifyLogging(_mockLogger, _startedMessage, processingMessage, processedMessage, _completedMessage);
        }

        [TestMethod, TestCategory("Unit")]
        public void ReadyForEmailServiceBusFunction_WhenDocumentDirectionIsSentByOrganisationAndApiThrowsException_FunctionProcessorLogsAndThrowsException()
        {
            // Arrange
            _message.DocumentDirection = ExchangeDocumentDirection.SentByOrganisation;

            MockFileProcessingApiClient
                .Setup(e => e.OrganisationUploadCompleteTask(_message.ParentBatchIdentifier))
                .Throws(Exception)
                .Verifiable();
            var processingMessage = GetProcessingMessage();
            var errorMessage = GetErrorMessage();

            // Act
            Func<Task> act = async () => { await _readyForEmailServiceBusFunction.Run(_message); };

            // Assert
            act.Should().ThrowAsync<Exception>();
            MockFileProcessingApiClient.Verify();

            VerifyLoggingAndException(_mockLogger, _startedMessage, processingMessage, errorMessage, _completedMessage);
        }

        [TestMethod, TestCategory("Unit")]
        public void ReadyForEmailServiceBusFunction_WhenDocumentDirectionIsInvalid_ThrowsExceptionAndLogsError()
        {
            // Arrange
            _message.DocumentDirection = (ExchangeDocumentDirection)2;

            var exceptionMessage = $"{Name} documentDirection must be a valid value. {(ExchangeDocumentDirection)2}";
            var processingMessage = GetProcessingMessage();
            var errorMessage = GetErrorMessage();

            Exception = new Exception(exceptionMessage);

            // Act
            Func<Task> act = async () => { await _readyForEmailServiceBusFunction.Run(_message); };

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage(exceptionMessage);

            _mockLogger.Verify(x => x.LogInformation(_startedMessage), Times.Once);
            _mockLogger.Verify(x => x.LogInformation(processingMessage), Times.Once);

            _mockLogger.Verify(ml => ml.LogError(It.Is<Exception>(e => e.Message == exceptionMessage), errorMessage), Times.Once);

            _mockLogger.Verify(x => x.LogInformation(_completedMessage), Times.Once);
        }

        private string GetProcessMessage()
        {
            return $"{Name} Processed message {_message}.";
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
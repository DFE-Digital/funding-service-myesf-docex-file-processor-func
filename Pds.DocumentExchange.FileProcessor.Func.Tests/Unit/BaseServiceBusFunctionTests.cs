using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Logging;
using Pds.DocumentExchange.FileProcessor.Services.Interfaces;
using Pds.DocumentExchange.FileProcessor.Services.Models;
using System;

namespace Pds.DocumentExchange.FileProcessor.Func.Tests.Unit
{
    [TestClass]
    public class BaseServiceBusFunctionTests
    {
        private Mock<IFileProcessorApiClient> mockFileProcessingApiClient = new Mock<IFileProcessorApiClient>();

        private DocumentReference _documentReference;

        protected Exception Exception { get; set; } = new Exception();

        protected DocumentReference DocumentReference { get => _documentReference; set => _documentReference = value; }

        protected Mock<IFileProcessorApiClient> MockFileProcessingApiClient { get => mockFileProcessingApiClient; set => mockFileProcessingApiClient = value; }

        protected DocumentReference DocumentReferenceParameter
            => It.Is<DocumentReference>(r =>
                r.ParentBatchIdentifier == _documentReference.ParentBatchIdentifier &&
                r.BatchIdentifier == _documentReference.BatchIdentifier &&
                r.FileName == _documentReference.FileName);

        /// <summary>
        /// Verify logging.
        /// </summary>
        /// <typeparam name="T">Type of logger.</typeparam>
        /// <param name="mockLogger">mock logger.</param>
        /// <param name="startMessage">Start message.</param>
        /// <param name="processingMessage">Processing message.</param>
        /// <param name="processedMessage">Processed message.</param>
        /// <param name="completedMessage">Completed message.</param>
        public void VerifyLogging<T>(Mock<ILoggerAdapter<T>> mockLogger, string startMessage, string processingMessage, string processedMessage, string completedMessage)
        {
            mockLogger.Verify(x => x.LogInformation(startMessage), Times.Once);
            mockLogger.Verify(x => x.LogInformation(processingMessage), Times.Once);
            mockLogger.Verify(x => x.LogInformation(processedMessage), Times.Once);
            mockLogger.Verify(x => x.LogInformation(completedMessage), Times.Once);
        }

        /// <summary>
        /// Verify logging and error.
        /// </summary>
        /// <typeparam name="T">Type of logger.</typeparam>
        /// <param name="mockLogger">mock logger.</param>
        /// <param name="startMessage">Start message.</param>
        /// <param name="processingMessage">Processing message.</param>
        /// <param name="errorMessage">Error message.</param>
        /// <param name="completedMessage">Completed message.</param>
        public void VerifyLoggingAndException<T>(Mock<ILoggerAdapter<T>> mockLogger, string startMessage, string processingMessage, string errorMessage, string completedMessage)
        {
            mockLogger.Verify(x => x.LogInformation(startMessage), Times.Once);
            mockLogger.Verify(x => x.LogInformation(processingMessage), Times.Once);
            mockLogger.Verify(e => e.LogError(Exception, errorMessage), Times.Once);
            mockLogger.Verify(x => x.LogInformation(completedMessage), Times.Once);
        }
    }
}
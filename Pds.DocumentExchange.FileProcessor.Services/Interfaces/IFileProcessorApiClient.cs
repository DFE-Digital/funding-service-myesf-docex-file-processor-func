using Pds.DocumentExchange.FileProcessor.Services.Models;
using System.Threading.Tasks;

namespace Pds.DocumentExchange.FileProcessor.Services.Interfaces
{
    /// <summary>
    /// File Processor Api Client service.
    /// </summary>
    public interface IFileProcessorApiClient
    {
        /// <summary>
        /// Method to call batch virus scan.
        /// </summary>
        /// <param name="batchIdentifier">the batch identifier.</param>
        /// <returns>async task.</returns>
        Task AgencyBatchDocumentVirusScan(string batchIdentifier);

        /// <summary>
        /// Method to call batch virus scan on upload.
        /// </summary>
        /// <param name="batchIdentifier">the batch identifier.</param>
        /// <returns>async task.</returns>
        Task OrganisationUploadDocumentVirusScan(string batchIdentifier);

        /// <summary>
        /// Method to call organisation files virus scan successful.
        /// </summary>
        /// <param name="documentReference">document reference object.</param>
        /// <returns>async task.</returns>
        Task OrganisationFilesVirusScanSuccessful(DocumentReference documentReference);

        /// <summary>
        /// Method to call organisation files virus scan successful.
        /// </summary>
        /// <param name="documentReference">document reference object.</param>
        /// <returns>async task.</returns>
        Task AgencyFilesVirusScanSuccessful(DocumentReference documentReference);

        /// <summary>
        /// Method to call agency publish complete.
        /// </summary>
        /// <param name="parentBatchIdentifier">the parent batch identifier.</param>
        /// <returns>async task.</returns>
        Task AgencyPublishCompleteTask(string parentBatchIdentifier);

        /// <summary>
        /// Method to call organisation upload complete.
        /// </summary>
        /// <param name="parentBatchIdentifier">the parent batch identifier.</param>
        /// <returns>async task.</returns>
        Task OrganisationUploadCompleteTask(string parentBatchIdentifier);

        /// <summary>
        /// Method to call batches virus scan fail.
        /// </summary>
        /// <param name="documentReference">document reference object.</param>
        /// <returns>async task.</returns>
        Task BatchesVirusScanFail(DocumentReference documentReference);

        /// <summary>
        /// Method to call upload batches virus scan fail.
        /// </summary>
        /// <param name="documentReference">document reference object.</param>
        /// <returns>async task.</returns>
        Task UploadBatchesVirusScanFail(DocumentReference documentReference);
    }
}
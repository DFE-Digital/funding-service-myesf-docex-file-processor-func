using Pds.DocumentExchange.Data.Services.Enums;
using System.Text.Json.Serialization;

namespace Pds.DocumentExchange.Data.Services.DTOs
{
    /// <summary>
    /// A service bus queue message with information about a file which has been scanned by the antivirus.
    /// </summary>
    public class ScannedFileQueueMessage : IMessage
    {
        /// <summary>
        /// Gets or sets the batch identifier.
        /// </summary>
        public string BatchIdentifier { get; set; }

        /// <inheritdoc/>
        public string ParentBatchIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the scanned file name.
        /// </summary>
        public string FileName { get; set; }

        /// <inheritdoc/>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ExchangeDocumentDirection DocumentDirection { get; set; }


        /// <inheritdoc/>
        public override string ToString()
        {
            return $"BatchIdentifier: {BatchIdentifier}, ParentBatchIdentifier: {ParentBatchIdentifier}, Filename: {FileName}, DocumentDirection: {DocumentDirection}";
        }
    }
}
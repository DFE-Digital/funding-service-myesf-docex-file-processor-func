using Pds.DocumentExchange.Data.Services.Enums;
using System.Text.Json.Serialization;

namespace Pds.DocumentExchange.Data.Services.DTOs
{
    /// <summary>
    /// A service bus queue message with information about a batch which has been fully scanned by the antivirus.
    /// </summary>
    public class ScannedBatchQueueMessage : IMessage
    {
        /// <summary>
        /// Gets or sets the batch identifier.
        /// </summary>
        public string BatchIdentifier { get; set; }

        /// <inheritdoc/>
        public string ParentBatchIdentifier { get; set; }

        /// <inheritdoc/>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ExchangeDocumentDirection DocumentDirection { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"BatchIdentifier: {BatchIdentifier}, ParentBatchIdentifier: {ParentBatchIdentifier}, DocumentDirection: {DocumentDirection}";
        }
    }
}
using Pds.DocumentExchange.Data.Services.Enums;
using System.Text.Json.Serialization;

namespace Pds.DocumentExchange.Data.Services.DTOs
{
    /// <summary>
    /// A service bus queue message representing a virus scan request for a (parent) batch.
    /// </summary>
    public class VirusScanRequestMessage : IMessage
    {
        /// <inheritdoc/>
        public string ParentBatchIdentifier { get; set; }

        /// <inheritdoc/>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ExchangeDocumentDirection DocumentDirection { get; set; }
    }
}
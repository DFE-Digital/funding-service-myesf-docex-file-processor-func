using Pds.DocumentExchange.Data.Services.Enums;
using System.Text.Json.Serialization;

namespace Pds.DocumentExchange.Data.Services.DTOs
{
    /// <summary>
    /// A service bus queue message.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Gets or sets the (parent) batch identifier.
        /// </summary>
        string ParentBatchIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the direction of the exchanged documents.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        ExchangeDocumentDirection DocumentDirection { get; set; }
    }
}
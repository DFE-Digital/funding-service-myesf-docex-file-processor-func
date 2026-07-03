using Pds.DocumentExchange.Data.Services.Enums;

namespace Pds.DocumentExchange.Data.Services.DTOs
{
    /// <summary>The Organisation Info.</summary>
    public class OrganisationInfo
    {
        /// <summary>Gets or sets the type of the organisation identifier.</summary>
        /// <seealso cref="OrganisationIdentifierType" />
        public OrganisationIdentifierType IdentifierType { get; set; }

        /// <summary>Gets or sets the organisation identifier.</summary>
        public string Identifier { get; set; }

        /// <summary>Gets or sets the organisation name.</summary>
        public string Name { get; set; }
    }
}
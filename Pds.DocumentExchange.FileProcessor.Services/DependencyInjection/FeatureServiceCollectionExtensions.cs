using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pds.Core.ApiClient.Interfaces;
using Pds.Core.ApiClient.Services;
using Pds.Core.Utils.Implementations;
using Pds.Core.Utils.Interfaces;
using Pds.DocumentExchange.FileProcessor.Services.DTOs.Configuration;
using Pds.DocumentExchange.FileProcessor.Services.Implementations;
using Pds.DocumentExchange.FileProcessor.Services.Interfaces;

namespace Pds.DocumentExchange.FileProcessor.Services.DependencyInjection
{
    /// <summary>
    /// Extensions class for <see cref="IServiceCollection"/> for registering the feature's services.
    /// </summary>
    public static class FeatureServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services for the current feature to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the feature's services to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddFeatureServices(this IServiceCollection services)
        {
            services.AddOptions<DocumentExchangeFileProcessorServicesConfiguration>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection(nameof(DocumentExchangeFileProcessorServicesConfiguration)).Bind(settings);
                });
            services.AddHttpClient<IFileProcessorApiClient, FileProcessorApiClient>();
            services.AddTransient(typeof(IAuthenticationService<>), typeof(AuthenticationService<>));
            services.AddTransient(typeof(IDateTimeProvider), typeof(DateTimeProvider));

            return services;
        }
    }
}
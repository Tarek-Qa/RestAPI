using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

[assembly: FunctionsStartup(typeof(RestAPI.Startup))]

namespace RestAPI
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var KeyVaultUrl = new Uri(Environment.GetEnvironmentVariable("KeyVaultUrl"));
            var secretClient = new SecretClient(KeyVaultUrl, new DefaultAzureCredential());
            var cs = secretClient.GetSecret("sql").Value.Value;
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(cs));

            var openAiApiKey = secretClient.GetSecret("aikey-1").Value.Value;
            builder.Services.AddSingleton(new OpenAIService(openAiApiKey));

            builder.Services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
            {
                return new DefaultOpenApiConfigurationOptions
                {
                    Info = new OpenApiInfo
                    {
                        Title = "My Rest API",
                        Version = "v1",
                        Description = "API documentation for my Azure Functions",
                        Contact = new OpenApiContact
                        {
                            Name = "Tarek Qassoumeh",
                            Email = "tarek.cosmic85@gmail.com",
                            Url = new Uri("https://github.com/Tarek-Qa")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "MIT",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        }
                    }
                };
            });
        }
    }
}

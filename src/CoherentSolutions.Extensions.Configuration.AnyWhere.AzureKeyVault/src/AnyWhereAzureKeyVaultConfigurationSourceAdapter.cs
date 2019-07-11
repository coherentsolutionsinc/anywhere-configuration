using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

using Newtonsoft.Json.Linq;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault
{
    public class AnyWhereAzureKeyVaultConfigurationSourceAdapter : IAnyWhereConfigurationAdapter
    {
        private static class KeyVault
        {
            private sealed class Connection
            {
                private const string ACCESS_TOKEN = "access_token";

                private const string ERROR_IDENTIFIER = "error";

                private const string ERROR_DESCRIPTION = "error_description";

                private readonly HttpClient http;

                private string accessToken;

                public string ApiEndpoint { get; }

                public string ApiVersion { get; }

                public Connection(
                    HttpClient http,
                    string apiEndpoint,
                    string apiVersion)
                {
                    this.http = http ?? throw new ArgumentNullException(nameof(http));

                    this.ApiEndpoint = apiEndpoint ?? throw new ArgumentNullException(nameof(apiEndpoint));
                    this.ApiVersion = apiVersion ?? throw new ArgumentNullException(nameof(apiVersion));
                }

                public async Task<string> GetAccessTokenAsync(
                    string authority,
                    string resource,
                    string scope)
                {
                    if (!string.IsNullOrEmpty(this.accessToken))
                    {
                        return this.accessToken;
                    }

                    var uri = $"http://{this.ApiEndpoint}/metadata/identity/oauth2/token"
                      + $"?api-version={this.ApiVersion}"
                      + $"&resource={HttpUtility.UrlEncode(resource)}";

                    var response = await this.http.GetAsync(uri);
                    if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == (HttpStatusCode) 429 /* Too many requests */)
                    {
                        throw new HttpRequestException($"The AIMS returned '{response.StatusCode}' status code, retrying if possible.");
                    }

                    var content = await response.Content.ReadAsStringAsync();

                    JObject resultObject;
                    try
                    {
                        resultObject = JObject.Parse(content);
                    }
                    catch (Exception e)
                    {
                        throw new AnyWhereAzureKeyVaultConfigurationSourceAdapterException(
                            "Invalid JSON response received from AIMS",
                            ExceptionDispatchInfo.Capture(e).SourceException);
                    }

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new AnyWhereAzureKeyVaultConfigurationSourceAdapterException(
                            $"The AIMS returned '{response.StatusCode}' status code with the following error: "
                          + $"'{resultObject[ERROR_IDENTIFIER].Value<string>()}'/'{resultObject[ERROR_DESCRIPTION].Value<string>()}'");
                    }

                    this.accessToken = resultObject[ACCESS_TOKEN].Value<string>();
                    return this.accessToken;
                }
            }

            private const string AIMS_ENDPOINT = "169.254.169.254";

            private const string AIMS_VERSION = "2018-10-01";

            public static KeyVaultClient CreateClientUsingServicePrinciple(
                string clientId,
                string clientSecret)
            {
                return new KeyVaultClient(
                    async (
                        authority,
                        resource,
                        scope) =>
                    {
                        var authenticationContext = new AuthenticationContext(authority);
                        var credentials = new ClientCredential(clientId, clientSecret);
                        return (await authenticationContext.AcquireTokenAsync(resource, credentials)).AccessToken;
                    });
            }

            public static KeyVaultClient CreateClientUsingManagedIdentity()
            {
                var http = new HttpClient
                {
                    DefaultRequestHeaders =
                    {
                        {
                            "Metadata", "true"
                        }
                    }
                };

                var connection = new Connection(http, AIMS_ENDPOINT, AIMS_VERSION);
                return new KeyVaultClient(
                    new KeyVaultCredential(connection.GetAccessTokenAsync),
                    http);
            }
        }

        public void ConfigureAppConfiguration(
            IConfigurationBuilder configurationBuilder,
            IAnyWhereConfigurationEnvironmentReader environmentReader)
        {
            if (configurationBuilder == null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }

            if (environmentReader == null)
            {
                throw new ArgumentNullException(nameof(environmentReader));
            }

            var vaultBaseUri = environmentReader.GetString("VAULT");
            var secretsString = environmentReader.GetString("SECRETS");

            var clientId = environmentReader.GetString("CLIENT_ID", optional: true);
            var clientSecret = environmentReader.GetString("CLIENT_SECRET", optional: true);

            if (string.IsNullOrWhiteSpace(secretsString))
            {
                return;
            }

            var values = new Dictionary<string, string>();
            using (var client = string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret)
                ? KeyVault.CreateClientUsingManagedIdentity()
                : KeyVault.CreateClientUsingServicePrinciple(clientId, clientSecret))
            {
                foreach (var secret in new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerable(secretsString.AsSpan()))
                {
                    var name = secret.Name;
                    var alias = secret.Alias;
                    var version = secret.Version;

                    if (string.IsNullOrEmpty(alias))
                    {
                        alias = name;
                    }

                    values[alias] = client.GetSecretAsync(vaultBaseUri, name, version, CancellationToken.None)
                       .GetAwaiter()
                       .GetResult()
                       .Value;
                }
            }

            configurationBuilder.Add(new AnyWhereAzureKeyVaultConfigurationSourceAdapterConfigurationSource(values));
        }
    }
}
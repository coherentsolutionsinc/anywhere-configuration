using System;
using System.Net;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using System.Web;

using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

using Newtonsoft.Json.Linq;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault
{
    public static class AnyWhereAzureKeyVaultConfigurationSourceAdapterKeyVaultClientFactory
    {
        private sealed class MsiAccessTokenCallback
        {
            private const string AIMS_ENDPOINT = "169.254.169.254";

            private const string AIMS_VERSION = "2018-10-01";

            private const string ACCESS_TOKEN = "access_token";

            private const string ERROR_IDENTIFIER = "error";

            private const string ERROR_DESCRIPTION = "error_description";

            private readonly HttpClient httpClient;

            public MsiAccessTokenCallback(
                HttpClient httpClient)
            {
                this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            }

            public async Task<string> GetAccessTokenAsync(
                string authority,
                string resource,
                string scope)
            {
                var uri = $"http://{AIMS_ENDPOINT}/metadata/identity/oauth2/token"
                  + $"?api-version={AIMS_VERSION}"
                  + $"&resource={HttpUtility.UrlEncode(resource)}";

                var response = await this.httpClient.GetAsync(uri);
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

                return resultObject[ACCESS_TOKEN].Value<string>();
            }
        }

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
            var httpClient = new HttpClient
            {
                DefaultRequestHeaders =
                {
                    {
                        "Metadata", "true"
                    }
                }
            };

            return new KeyVaultClient(
                new KeyVaultCredential(new MsiAccessTokenCallback(httpClient).GetAccessTokenAsync),
                httpClient);
        }
    }
}
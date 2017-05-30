using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CryptoBot.ExchangeApi.Market.Poloniex
{
    internal sealed class DataRetriever : IDataRetriever
    {
        public async Task<string> PerformRequest(string serverUrl, string command, IDictionary<string, string> parameters)
        {
            try
            {
                // Create the web request
                var url = $"{serverUrl}?command={command}";
                if (!ReferenceEquals(parameters, null)) {
                    foreach (var parameter in parameters)
                    {
                        url = string.Concat(url, $"&{parameter.Key}={parameter.Value}");
                    }
                }
                var webRequest = WebRequest.CreateHttp(new Uri(url));

                // Perform webrequest
                using (var webResponse = (HttpWebResponse)await webRequest.GetResponseAsync())
                {
                    // Verify reponse OK
                    if (webResponse.StatusCode != HttpStatusCode.OK)
                    {
                        throw new ExchangeApiException(
                            $"Http error '{webResponse.StatusCode}' during request: {webResponse.StatusDescription}");
                    }

                    var response = webResponse.GetResponseStream();
                    if (ReferenceEquals(response, null))
                    {
                        throw new ExchangeApiException("No response data provided");
                    }

                    // Parse response as JSON
                    using (var reader = new StreamReader(response))
                    {
                        var objJson = reader.ReadToEnd();

                        // ReSharper disable once InvertIf
                        if (objJson.Contains("\"error\""))
                        {
                            var error = JsonConvert.DeserializeObject<PoloniexError>(objJson);

                            throw new ExchangeApiException($"Poloniex error occurred: {error.Error}");
                        }

                        return objJson;
                    }
                }
            }
            catch (UriFormatException e)
            {
                throw new ExchangeApiException("Specified API url not valid", e);
            }
            catch (IOException e)
            {
                throw new ExchangeApiException("Unable to read JSON response", e);
            }
            catch (WebException e)
            {
                throw new ExchangeApiException("Error peform web request", e);
            }
        }

        public async Task<T> PerformRequest<T>(string serverUrl, string command, IDictionary<string, string> parameters)
        {
            try
            {
                // Perform request
                string response = await PerformRequest(serverUrl, command, parameters);

                // Convert to object
                return JsonConvert.DeserializeObject<T>(response);
            }
            catch (JsonSerializationException e)
            {
                throw new ExchangeApiException("Unable to deserialize JSON to object", e);
            }
        }
        
        // ReSharper disable once ClassNeverInstantiated.Local
        private sealed class PoloniexError
        {
            [JsonProperty("error")]
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Error { get; set; }
        }
    }
}

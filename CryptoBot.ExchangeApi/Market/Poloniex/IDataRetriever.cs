using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoBot.ExchangeApi.Market.Poloniex
{
    public interface IDataRetriever
    {
        Task<string> PerformRequest(string serverUrl, string command, IDictionary<string, string> parameters);

        Task<T> PerformRequest<T>(string serverUrl, string command, IDictionary<string, string> parameters);
    }
}
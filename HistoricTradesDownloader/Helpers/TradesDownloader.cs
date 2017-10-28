using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CryptoBot.ExchangeApi.Market;
using CryptoBot.Utils.General;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace HistoricTradesDownloader.Helpers
{
    internal class TradesDownloader
    {
        private readonly ImmutableList<IMarketApi> _marketApis;

        public TradesDownloader(IList<IMarketApi> marketApis)
        {
            _marketApis = ImmutableList.CreateRange(marketApis);
        }

        public IList<Exchange> GetExchanges()
        {
            return _marketApis.Select(m => m.Exchange).Distinct().ToList();
        }

        public async Task<IList<CurrencyPair>> LoadCurrencyPairs(Exchange exchange)
        {
            var marketApi = _marketApis.FirstOrDefault(m => m.Exchange == exchange);
            if (ReferenceEquals(marketApi, null))
            {
                return new List<CurrencyPair>();
            }

            return await marketApi.ReadCurrencyPairs();
        }

        public async Task DownloadHistoricTrades(Exchange exchange, CurrencyPair currencyPair, DateTime startTime, DateTime endTime, 
            string csvPath, Action<int> updateProgress)
        {
            var totalHours = (endTime - startTime).TotalHours;
            var totalDownloadedHours = 0;
            var downloadStartTime = startTime;
            
            // Configure CSV Writer
            var options = new TypeConverterOptions
            {
                Formats = new[] { "yyyy-MM-ddTHH:mm:ss.fffK" },
                DateTimeStyle = DateTimeStyles.AdjustToUniversal
            };
            var configuration = new Configuration {CultureInfo = CultureInfo.InvariantCulture};
            configuration.TypeConverterOptionsCache.AddOptions<DateTime>(options);

            using (var csvWriter = new CsvWriter(new StreamWriter(csvPath), configuration))
            {
                while (downloadStartTime < endTime)
                {
                    // Determine end time for download
                    var downloadEndTime = downloadStartTime.AddHours(1);
                    downloadEndTime = downloadEndTime > endTime ? endTime : downloadEndTime;

                    // Download data for max an hour
                    var time = downloadStartTime;
                    var historicTrades = await Task.Run(() => ReadHistoricTrades(exchange, currencyPair, time, downloadEndTime));

                    // Write data to CSV
                    csvWriter.WriteRecords(historicTrades);
                    await csvWriter.FlushAsync();

                    // Update progress bar
                    totalDownloadedHours++;
                    updateProgress((int) Math.Round(totalDownloadedHours / totalHours * 100));

                    // Increase download start time for next iteration
                    downloadStartTime = downloadStartTime.AddHours(1);

                    // Wait some time to not exceed max requests of market api
                    await Task.Delay(TimeSpan.FromSeconds(0.5));
                }
            }
        }

        private async Task<IList<Trade>> ReadHistoricTrades(Exchange exchange, CurrencyPair currencyPair, DateTime startTime, DateTime endTime)
        {
            var marketApi = _marketApis.FirstOrDefault(m => m.Exchange == exchange);
            if (ReferenceEquals(marketApi, null))
            {
                return new List<Trade>();
            }

            return await marketApi.ReadHistoricTrades(currencyPair, startTime, endTime);
        }
    }
}

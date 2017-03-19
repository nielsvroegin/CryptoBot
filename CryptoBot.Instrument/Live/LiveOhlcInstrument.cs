using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CryptoBot.ExchangeApi;
using CryptoBot.ExchangeApi.Market;
using CryptoBot.Instrument.Static;
using CryptoBot.Utils.General;
using CryptoBot.Utils.Assertions;
using CryptoBot.Utils.Helpers;
using CryptoBot.Utils.Logging;
using log4net;

namespace CryptoBot.Instrument.Live
{
    internal class LiveOhlcInstrument
    {
        private static readonly ILog Logger = ApplicationLogging.CreateLogger<LiveOhlcInstrument>();
        private static readonly SemaphoreSlim UpdateSemaphore = new SemaphoreSlim(1, 1);

        public Exchange Exchange { get; }
        public CurrencyPair CurrencyPair { get; }
        
        private readonly IMarketApi _marketApi;
        private readonly IList<LiveOhlcSeries> _ohlcSeries = new List<LiveOhlcSeries>();

        public LiveOhlcInstrument(Exchange exchange, CurrencyPair currencyPair, IMarketApi marketApi)
        {
            _marketApi = Preconditions.CheckNotNull(marketApi);
            Exchange = Preconditions.CheckNotNull(exchange);
            CurrencyPair = Preconditions.CheckNotNull(currencyPair);

            // Add Ohlc series to dictionary
            _ohlcSeries.Add(new LiveOhlcSeries(300)); // 5 min
			_ohlcSeries.Add(new LiveOhlcSeries(900)); // 15 min
			_ohlcSeries.Add(new LiveOhlcSeries(1800)); // 30 min
			_ohlcSeries.Add(new LiveOhlcSeries(7200)); // 2 hour
            _ohlcSeries.Add(new LiveOhlcSeries(14400)); // 4 hour
            _ohlcSeries.Add(new LiveOhlcSeries(86400)); // 1 day
        }

        internal async Task<OhlcInstrument> Update(TickData tickData)
        {
            // Only allow one update at a time
            await UpdateSemaphore.WaitAsync();

            try
            {
                // Update all OhlcSeries concurrently
                var updateTasks = _ohlcSeries.Select(ohlcSeries => UpdateOhlcSeries(ohlcSeries, tickData));
                await Task.WhenAll(updateTasks);

                return ToInstrument();
            }
            finally
            {
                UpdateSemaphore.Release();
            }
        }

        private async Task UpdateOhlcSeries(LiveOhlcSeries ohlcSeries, TickData tickData)
        {
            // When OhlcSeries doesn't contain any data yet, try to initialize
            if (!ohlcSeries.Initialized)
            {
                await InitializeSeries(ohlcSeries, tickData.TickTimeEpoch);
            }

            // Provide tick to series
            ohlcSeries.AddTick(tickData);
        }

        private async Task InitializeSeries(LiveOhlcSeries ohlcSeries, long tickTime)
        {
            OhlcSeries historicOhlc = null;
            try
            {
                // Determine time span to collect initial data
                var startTimeEpoch = tickTime - ohlcSeries.TimeSpanSeconds * ohlcSeries.MaxSeriesSize;

                var startTime = DateTimeHelper.ToDateTime(startTimeEpoch);
                var endTime = DateTimeHelper.ToDateTime(tickTime);

                // Read historic ohlc
                historicOhlc = await _marketApi.ReadOhlcSeries(CurrencyPair, startTime, endTime,
                    ohlcSeries.TimeSpanSeconds);
            }
            catch (ExchangeApiException e)
            {
                Logger.Warn($"Error loading historic data for OhlcSeries {ohlcSeries.TimeSpanSeconds}.", e);
            }
            finally
            {
                var ohlcItems = historicOhlc != null ? historicOhlc.OhlcItems : ImmutableList<OhlcItem>.Empty;

                // Init live ohlc series with historic data or empty list so series is initialized
                ohlcSeries.Initialize(ohlcItems);
            }
        }

        private OhlcInstrument ToInstrument()
        {
            return new OhlcInstrument(Exchange, CurrencyPair, _ohlcSeries.Select(l => l.ToOhlcSeries()).ToList());
        }
   }
}

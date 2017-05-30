using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoBot.ExchangeApi.Market;
using CryptoBot.ExchangeApi.Market.Poloniex;
using CryptoBot.Utils.General;
using HistoricTradesDownloader;
using HistoricTradesDownloader.Helpers;

namespace HistoricTradesDownloader.ViewModels
{
    public class DownloaderViewModel : ObservableObject
    {
        private readonly TradesDownloader _tradesDownloader;

        private Exchange? _selectedExchange;
        private CurrencyPair _selectedCurrencyPair;

        public IList<Exchange> Exchanges { get; private set; }
        public IList<CurrencyPair> CurrencyPairs { get; private set; }

        public Exchange? SelectedExchange
        {
            get { return _selectedExchange; }
            set
            {
                _selectedExchange = value;
                if (_selectedExchange.HasValue)
                {
                    LoadCurrencyPairs(_selectedExchange.Value);
                }
            }
        }
        
        public CurrencyPair SelectedCurrencyPair { get; set; }

        public DownloaderViewModel()
        {
            // Initialized TradesDownloader
            IList<IMarketApi> marketApis = new List<IMarketApi>
            {
                new PoloniexMarketApi()
            };
            _tradesDownloader = new TradesDownloader(marketApis);

            // Set properties
            Exchanges = _tradesDownloader.GetExchanges();
        }

        private async void LoadCurrencyPairs(Exchange exchange)
        {
            CurrencyPairs = await Task.Run(() => _tradesDownloader.LoadCurrencyPairs(exchange));

            OnPropertyChanged("CurrencyPairs");
        }
    }
}

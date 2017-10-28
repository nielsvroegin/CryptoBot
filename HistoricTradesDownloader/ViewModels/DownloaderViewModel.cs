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
using System.Windows.Input;
using HistoricTradesDownloader.ViewModels.Commands;
using Microsoft.Win32;

namespace HistoricTradesDownloader.ViewModels
{
    public class DownloaderViewModel : ObservableObject
    {
        private readonly TradesDownloader _tradesDownloader;

        private Exchange? _selectedExchange;        

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

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public String CsvPath { get; set; }

        public ICommand ChooseCsvLocationCmd { get; }

        public ICommand StartDownloadCmd { get; }

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
            StartTime = DateTime.Now.AddMonths(-1);
            EndTime = DateTime.Now;

            // Set commands
            ChooseCsvLocationCmd = new RelayCommand(e => ChooseCsvLocation());
            StartDownloadCmd = new RelayCommand(e => StartDownload(), e => CanDownload());
        }

        private async void LoadCurrencyPairs(Exchange exchange)
        {
            CurrencyPairs = await Task.Run(() => _tradesDownloader.LoadCurrencyPairs(exchange));

            OnPropertyChanged("CurrencyPairs");
        }

        private bool CanDownload()
        {
            return SelectedExchange != null && SelectedCurrencyPair != null && StartTime != null && EndTime != null && !String.IsNullOrEmpty(CsvPath);
        }
        
        private void ChooseCsvLocation()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Csv file (*.csv)|*.csv";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            bool? result = saveFileDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                CsvPath = saveFileDialog.FileName;
                OnPropertyChanged("CsvPath");
            }

        }

        private void StartDownload()
        {

        }
    }
}

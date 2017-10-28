using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CryptoBot.ExchangeApi.Market;
using CryptoBot.ExchangeApi.Market.Poloniex;
using CryptoBot.Utils.General;
using HistoricTradesDownloader.Helpers;
using System.Windows.Input;
using CryptoBot.Utils.Assertions;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using HistoricTradesDownloader.ViewModels.Commands;
using Microsoft.Win32;

namespace HistoricTradesDownloader.ViewModels
{
    public class DownloaderViewModel : ObservableObject
    {
        private readonly TradesDownloader _tradesDownloader;

        private Exchange? _selectedExchange;
        private int _downloadProgress;
        private bool _isNotDownloading;
        private string _csvPath;
        private IList<CurrencyPair> _currencyPairs = new List<CurrencyPair>();

        public IList<Exchange> Exchanges { get; }

        public IList<CurrencyPair> CurrencyPairs
        {
            get => _currencyPairs;
            private set
            {
                _currencyPairs = value;
                OnPropertyChanged("CurrencyPairs");
                OnPropertyChanged("AllowCurrencyPairSelection");
            }
        }

        public Exchange? SelectedExchange
        {
            get => _selectedExchange;
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

        public string CsvPath
        {
            get => _csvPath;
            private set
            {
                _csvPath = value;
                OnPropertyChanged("CsvPath");
            }
        }

        public int DownloadProgress
        {
            get => _downloadProgress;
            private set
            {
                _downloadProgress = value;
                OnPropertyChanged("DownloadProgress");
            }
        }

        public bool IsNotDownloading
        {
            get => _isNotDownloading;
            private set
            {
                _isNotDownloading = value;
                OnPropertyChanged("IsNotDownloading");
                OnPropertyChanged("AllowCurrencyPairSelection");
            }
        }

        public bool AllowCurrencyPairSelection => IsNotDownloading && CurrencyPairs.Any();

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
            IsNotDownloading = true;
            Exchanges = _tradesDownloader.GetExchanges();
            StartTime = DateTime.Now.AddHours(-1);
            EndTime = DateTime.Now;

            // Set commands
            ChooseCsvLocationCmd = new RelayCommand(e => ChooseCsvLocation());
            StartDownloadCmd = new RelayCommand(e => StartDownload(), e => CanDownload());
        }

        private async void LoadCurrencyPairs(Exchange exchange)
        {
            CurrencyPairs = await Task.Run(() => _tradesDownloader.LoadCurrencyPairs(exchange));
        }

        private bool CanDownload()
        {
            return SelectedExchange != null && SelectedCurrencyPair != null && StartTime < EndTime && !String.IsNullOrEmpty(CsvPath) && IsNotDownloading;
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
            }
        }

        private async void StartDownload()
        {
            Preconditions.CheckNotNull(SelectedExchange);
            Preconditions.CheckNotNull(SelectedCurrencyPair);
            Preconditions.CheckNotNull(StartTime);
            Preconditions.CheckNotNull(EndTime);
            Preconditions.CheckNotNull(CsvPath);
            
            try
            {
                IsNotDownloading = false;
                DownloadProgress = 0;

                // Download the trades
                await _tradesDownloader.DownloadHistoricTrades(
                    SelectedExchange.GetValueOrDefault(),
                    SelectedCurrencyPair,
                    StartTime,
                    EndTime,
                    CsvPath,
                    progress => { DownloadProgress = progress; }
                );
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("The directory to save the CSV could not be found");
            }
            finally
            {
                DownloadProgress = 0;
                IsNotDownloading = true;
            }

            // Notify user download completed
            MessageBox.Show("The historic trades have been downloaded and stored in the CSV file.", "Download completed!");
        }
    }
}

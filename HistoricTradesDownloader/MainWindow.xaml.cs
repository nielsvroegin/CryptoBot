using System.Collections.Generic;
using System.Windows;
using CryptoBot.ExchangeApi.Market;
using CryptoBot.ExchangeApi.Market.Poloniex;
using HistoricTradesDownloader.ViewModels;

namespace HistoricTradesDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new DownloaderViewModel();
        }
    }
}

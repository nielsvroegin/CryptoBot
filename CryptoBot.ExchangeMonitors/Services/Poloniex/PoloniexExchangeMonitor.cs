using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using CryptoBot.Utils.Assertions;
using CryptoBot.Utils.General;
using CryptoBot.Utils.Logging;
using CryptoBot.Utils.ServiceHandler;
using log4net;
using Newtonsoft.Json.Linq;
using WampSharp.Binding;
using WampSharp.Core.Listener;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Realm;
using WampSharp.WebSocket4Net;

namespace CryptoBot.ExchangeMonitors.Services.Poloniex
{
    /// <summary>
    /// Exchange monitor service implementation for Poloniex exchange
    /// </summary>
    public sealed class PoloniexExchangeMonitor : IManagedService, IExchangeMonitor
    {
        private static readonly ILog Logger = ApplicationLogging.CreateLogger<PoloniexExchangeMonitor>();
        private const string ServerAddress = "wss://api.poloniex.com";
        private const string TickerTopic = "ticker";

        private readonly IList<Subscription> _subscriptions = new List<Subscription>();
        private readonly IWampChannelFactory _wampChannelFactory;
        private readonly Dictionary<CurrencyPair, IDisposable> _tradesAndOrdersSubscription = new Dictionary<CurrencyPair, IDisposable>();

        private volatile bool _isConnected;
        private IWampChannel _channel;
        private IDisposable _tickerSubscription;

        /// <summary>
        /// Corresponding exchange for Ticker service
        /// </summary>
        public Exchange Exchange => Exchange.Poloniex;

        /// <summary>
        /// Constructor
        /// </summary>
        public PoloniexExchangeMonitor() : this(new WampChannelFactory()) { }

        /// <summary>
        /// Constructor
        /// </summary>
        public PoloniexExchangeMonitor(IWampChannelFactory wampChannelFactory)
        {
            _wampChannelFactory = Preconditions.CheckNotNull(wampChannelFactory);
        }

        /// <inheritdoc />
        public void Start()
        {
            // Open channel to poloniex
            var mJsonBinding = new JTokenJsonBinding();
			Func<IControlledWampConnection<JToken>> connectionFactory = () => new WebSocket4NetTextConnection<JToken>(ServerAddress, mJsonBinding);
            _channel = _wampChannelFactory.CreateChannel("realm1", connectionFactory, mJsonBinding);
            _channel.RealmProxy.Monitor.ConnectionBroken += OnConnectionBroken;
            _channel.RealmProxy.Monitor.ConnectionEstablished += OnConnectionEstablised;
            _channel.RealmProxy.Monitor.ConnectionError += OnConnectionError;
            _channel.Open().Wait(5000);
            
            Logger.Info("Poloniex ticker service has been started");
        }

        /// <inheritdoc />
        public void Stop()
        {
            // Stop subscription and close channel
            _tickerSubscription?.Dispose();
            _tradesAndOrdersSubscription.Values.ForEach(s => s.Dispose());
            _tradesAndOrdersSubscription.Clear();
            _channel.Close();

            Logger.Info("Poloniex ticker service has been stopped");
        }

        /// <inheritdoc />
        public void Subscribe(CurrencyPair currencyPair, IExchangeMonitorSubscriber subscriber)
        {
            Preconditions.CheckNotNull(currencyPair);
            Preconditions.CheckNotNull(subscriber);

            // Subscribe subscriber
            lock (_subscriptions)
            {
                if (ReferenceEquals(FindSubscription(currencyPair, subscriber), null))
                {
                    _subscriptions.Add(new Subscription(currencyPair, subscriber));
                    CheckChannelSubscriptions();
                }
            }
        }

        /// <inheritdoc />
        public void Unsubscribe(CurrencyPair currencyPair, IExchangeMonitorSubscriber subscriber)
        {
            Preconditions.CheckNotNull(currencyPair);
            Preconditions.CheckNotNull(subscriber);

            // Unsubscribe subscriber
            lock (_subscriptions)
            {
                var subscription = FindSubscription(currencyPair, subscriber);
                if (!ReferenceEquals(subscription, null))
                {
                    _subscriptions.Remove(subscription);
                    CheckChannelSubscriptions();
                }
            }
        }

        private void CheckChannelSubscriptions()
        {
            // Only check channel subscription when connected
            if (!_isConnected)
            {
                return;
            }

            lock (_subscriptions)
            {
                CheckTickerSubscription();
                CheckTradesAndOrdersSubscriptions();
            }
        }

        private void CheckTickerSubscription()
        {
            // Subscribe to ticker topic
            if (_subscriptions.Count > 0 && ReferenceEquals(_tickerSubscription, null))
            {
                _tickerSubscription = _channel.RealmProxy.Services.GetSubject(TickerTopic)
                    .Subscribe(x => ProcessTick(x.Arguments));
            }

            // Unsubscribe from ticker topic
            if (_subscriptions.Count == 0 && !ReferenceEquals(_tickerSubscription, null))
            {
                _tickerSubscription.Dispose();
                _tickerSubscription = null;
            }
        }

        private void CheckTradesAndOrdersSubscriptions()
        {
            // Subscribe to trade topic
            foreach (var currencyPair in _subscriptions.Select(s => s.CurrencyPair).Distinct()
                .Except(_tradesAndOrdersSubscription.Keys))
            {
                _tradesAndOrdersSubscription.Add(
                    currencyPair,
                    _channel.RealmProxy.Services.GetSubject(currencyPair.ToString())
                        .Subscribe(x => ProcessTradeOrOrder(x.Arguments))
                );
            }

            // Unsubscribe from trade topic
            foreach (var currencyPair in _tradesAndOrdersSubscription.Keys.Except(_subscriptions.Select(s => s.CurrencyPair)))
            {
                _tradesAndOrdersSubscription[currencyPair].Dispose();
                _tradesAndOrdersSubscription.Remove(currencyPair);
            }
        }

        private Subscription FindSubscription(CurrencyPair currencyPair, IExchangeMonitorSubscriber subscriber)
        {
            lock (_subscriptions)
            {
                return _subscriptions.FirstOrDefault(s => s.CurrencyPair.Equals(currencyPair) && s.Subscriber == subscriber);
            }
        }

        private void OnConnectionEstablised(object sender, WampSessionCreatedEventArgs e)
        {
            _isConnected = true;

            CheckChannelSubscriptions();

            Logger.Debug("Connection established to Poloniex exchange monitor");
        }

        private void OnConnectionError(object sender, WampConnectionErrorEventArgs e)
        {
            _isConnected = false;

            Restart();

            Logger.Error("Connection error in Poloniex exchange monitor", e.Exception);
        }

        private void OnConnectionBroken(object sender, WampSessionCloseEventArgs e)
        {
            _isConnected = false;

            // Allow disconnecton, but re-initialize connection in case of any other reason
            if (e.CloseType != SessionCloseType.Disconnection)
            {
                Restart();
            }
        }

        private void Restart()
        {
            Task.Run(async () =>
            {
                await Task.Delay(5000);

                // Re-initialize connection
                Stop();
                Start();
            });
        }

        private void ProcessTick(ISerializedValue[] serializedData)
        {
            // Deserialize tick data
            TickData tickData;
            try
            {
                // Check if currenct pair is known
                if (ReferenceEquals(TickConverter.GetCurrencyPair(serializedData), null))
                {
                    Logger.Debug($"Received tick of unknown currency Pair '{ serializedData[0] }', skipping tick");
                    return;
                }

                // Convert tick
                tickData = TickConverter.Convert(Exchange, serializedData);

                Logger.Debug($"Received new tick: {tickData}");
            }
            catch (Exception e)
            {
                Logger.Error("Error deserializing tick data", e);
                return;
            }

            // Notify subscribers
            NotifySubscribers(tickData.CurrencyPair, s => s.OnTick(Exchange, tickData));
        }

        private void ProcessTradeOrOrder(ISerializedValue[] arguments)
        {
            
        }

        private void NotifySubscribers(CurrencyPair currencyPair, Action<IExchangeMonitorSubscriber> notify)
        {
            lock (_subscriptions)
            {
                foreach (var subscription in _subscriptions.Where(s => s.CurrencyPair.Equals(currencyPair)))
                {
                    Task.Run(() => notify(subscription.Subscriber));
                }
            }
        }
    }
}

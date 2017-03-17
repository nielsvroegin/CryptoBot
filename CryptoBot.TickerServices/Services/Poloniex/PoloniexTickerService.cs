using System;
using System.Collections.Generic;
using CryptoBot.Utils.General;
using CryptoBot.Utils.Logging;
using CryptoBot.Utils.Assertions;
using CryptoBot.Utils.ServiceHandler;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using WampSharp.Binding;
using WampSharp.Core.Listener;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Realm;
using WampSharp.WebSocket4Net;

namespace CryptoBot.TickerServices.Services.Poloniex
{
    /// <summary>
    /// Ticker service implementation for Poloniex exchange
    /// </summary>
    public sealed class PoloniexTickerService : IManagedService, ITickerService
    {
        private static readonly ILogger Logger = ApplicationLogging.CreateLogger<PoloniexTickerService>();
        private const string ServerAddress = "wss://api.poloniex.com";
        private const string TickerTopic = "ticker";

        private readonly IList<ITickerSubscriber> _subscribers = new List<ITickerSubscriber>();
        private readonly IWampChannelFactory _wampChannelFactory;

        private IDisposable _subscription;
        private IWampChannel _channel;

        /// <summary>
        /// Corresponding exchange for Ticker service
        /// </summary>
        public Exchange Exchange => Exchange.Poloniex;

        /// <summary>
        /// Constructor
        /// </summary>
        public PoloniexTickerService() : this(new WampChannelFactory()) { }

        /// <summary>
        /// Constructor
        /// </summary>
        public PoloniexTickerService(IWampChannelFactory wampChannelFactory)
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
            _channel.Open().Wait(5000);

            // Subscribe to ticker topic
            _subscription = _channel.RealmProxy.Services.GetSubject(TickerTopic).Subscribe(x => ProcessTick(x.Arguments));

            Logger.LogInformation("Poloniex ticker service has been started");
        }

        /// <inheritdoc />
        public void Stop()
        {
            // Stop subscription and close channel
            _subscription.Dispose();
            _channel.Close();

            Logger.LogInformation("Poloniex ticker service has been stopped");
        }

        /// <inheritdoc />
        public void Subscribe(ITickerSubscriber subscriber)
        {
            Preconditions.CheckNotNull(subscriber);

            // Subscribe subscriber
            lock (_subscribers)
            {
                _subscribers.Add(subscriber);
            }
        }

        /// <inheritdoc />
        public void Unsubscribe(ITickerSubscriber subscriber)
        {
            Preconditions.CheckNotNull(subscriber);

            // Unsubscribe subscriber
            lock (_subscribers)
            {
                _subscribers.Remove(subscriber);
            }
        }

        private void OnConnectionBroken(object sender, WampSessionCloseEventArgs e)
        {
            // Allow disconnecton, but re-initialize connection in case of any other reason
            if (e.CloseType == SessionCloseType.Disconnection)
            {
                return;
            }

            // Re-initialize connection
            Stop();
            Start();
        }

        private void ProcessTick(ISerializedValue[] arguments)
        {
            // Determine applicable currency pair
            var currencyPairStr = arguments[0].Deserialize<string>();
            var currencyPair = CurrencyPair.Parse(currencyPairStr);
            if (ReferenceEquals(currencyPair, null))
            {
                Logger.LogTrace("Received tick of unknown currency Pair '{0}', skipping tick", currencyPairStr);
                return;
            }

            // Convert data to TickData
            var tickData = new TickData.Builder(currencyPair, DateTimeOffset.Now.ToUnixTimeSeconds(), arguments[1].Deserialize<decimal>())
                .LowestAsk(arguments[2].Deserialize<decimal>())
                .HighestBid(arguments[3].Deserialize<decimal>())
                .PercentChange(arguments[4].Deserialize<decimal>())
                .BaseVolume(arguments[5].Deserialize<decimal>())
                .QuoteVolume(arguments[6].Deserialize<decimal>())
                .IsFrozen(arguments[7].Deserialize<byte>() > 0)
                .DayHigh(arguments[8].Deserialize<decimal>())
                .DayLow(arguments[9].Deserialize<decimal>())
                .Build();

            Logger.LogTrace("Received new tick: {0}", tickData);

            // Notify subscribers
            lock (_subscribers)
            {
                foreach (var subscriber in _subscribers)
                {
                    subscriber.OnTick(Exchange, tickData);
                }
            }
        }
    }
}

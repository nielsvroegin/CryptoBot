using System;
using System.Collections.Generic;
using CryptoBot.Utils.ServiceHandler;

namespace CryptoBot.TickerServices
{
	public class PoloniexTickerService : ITickerService, IManagedService
	{
		readonly IList<ITickerSubscriber> _subscribers;

		public PoloniexTickerService()
		{
			_subscribers = new List<ITickerSubscriber>();
		}

		public void Start()
		{
			throw new NotImplementedException();
		}

		public void Stop()
		{
			throw new NotImplementedException();
		}

		public void Subscribe(ITickerSubscriber subscriber)
		{
			_subscribers.Add(subscriber);
		}

		public void Unsubscribe(ITickerSubscriber subscriber)
		{
			_subscribers.Remove(subscriber);
		}


	}
}

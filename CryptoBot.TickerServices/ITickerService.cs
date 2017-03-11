using System;

namespace CryptoBot.TickerServices
{
	public interface ITickerService
	{
		void Subscribe(ITickerSubscriber subscriber);
		void Unsubscribe(ITickerSubscriber subscriber);
	}
}

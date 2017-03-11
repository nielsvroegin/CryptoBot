using System;
using CryptoBot.TickerServices.Data;

namespace CryptoBot.TickerServices
{
	public interface ITickerSubscriber
	{
		void OnTick(TickData tickData);
	}
}

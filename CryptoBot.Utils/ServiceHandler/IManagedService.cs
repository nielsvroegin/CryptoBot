using System;

namespace CryptoBot.Utils.ServiceHandler
{
	public interface IManagedService
	{
		void Start();
		void Stop();
	}
}

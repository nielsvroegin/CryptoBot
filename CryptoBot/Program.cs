using System;
using CryptoBot.TickerServices;

namespace CryptoBot
{
    class Program
    {
        static void Main(string[] args)
        {
			var service = new PoloniexTickerService();
			service.Start();
        }
    }
}

using System;
using CryptoBot.TickerServices.Services.Poloniex;

namespace CryptoBot
{
    internal class Program
    {
        private static void Main(string[] args)
        {
			var service = new PoloniexTickerService();
			service.Start();

            Console.ReadLine();
        }
    }
}

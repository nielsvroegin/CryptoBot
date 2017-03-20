using System;
using CryptoBot.Utils.General;
using TicTacTec.TA.Library;
using System.Linq;

namespace CryptoBot.Instrument.Static
{
    public static class OhlcSeriesExtensions
    {
        /// <summary>
        /// Calculate EMA for last OhlcSeries value
        /// </summary>
        /// <param name="ohlcSeries">OhlcSeries to use for calcuting EMA</param>
        /// <param name="optInTimePeriod">Period to use in EMA calculation</param>
        /// <returns>EMA value of last OhlcItem</returns>
        public static double Ema(this OhlcSeries ohlcSeries, int optInTimePeriod)
        {
            int neededValues = Core.EmaLookback(optInTimePeriod) + 1;
            if (ohlcSeries.OhlcItems.Count < neededValues)
            {
                throw new OhlcCalculationException("Not enough ohlc items to perform calculation");
            }

            int outBegIdx, outNbElement;
            double[] inputClose = ohlcSeries.OhlcItems.Take(neededValues).Select(i => Convert.ToDouble(i.Close)).Reverse().ToArray();
            double[] output = new double[inputClose.Length];
            
            var res = Core.Ema(0, inputClose.Length - 1, inputClose, optInTimePeriod, out outBegIdx, out outNbElement, output);

            if (res != Core.RetCode.Success)
            {
                throw new OhlcCalculationException($"Unable to calculate EMA value, error: {res}");
            }

            return output[0];
        }
    }
}

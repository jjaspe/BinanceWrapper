using BinanceWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BinanceTester
{
    [TestClass]
    public class UnitTest1
    {
        IndicatorService indicator = new IndicatorService();

        [TestMethod]
        public void TestMethod1()
        {
            List<BinanceCandleStickData> candles = new List<BinanceCandleStickData>()
            {
                new BinanceCandleStickData(){Gain = 1, Loss = 0},
                new BinanceCandleStickData(){Gain = 0, Loss = 0.6875M},
                new BinanceCandleStickData(){Gain = 0.5M, Loss = 0},
                new BinanceCandleStickData(){Gain = 0, Loss = 2M},
                new BinanceCandleStickData(){Gain = 0, Loss = 0.6875M},
                new BinanceCandleStickData(){Gain = 0.3750M, Loss = 0},
                new BinanceCandleStickData(){Gain = 1.1250M, Loss = 0},
                new BinanceCandleStickData(){Gain = 2.0625M, Loss = 0},
                new BinanceCandleStickData(){Gain = 0, Loss = 0.25M},
                new BinanceCandleStickData(){Gain = 0, Loss = 0.5625M},
                new BinanceCandleStickData(){Gain = 0, Loss = 2.4375M},
                new BinanceCandleStickData(){Gain = 1.750M, Loss = 0},
                new BinanceCandleStickData(){Gain = 1.375M, Loss = 0},
                new BinanceCandleStickData(){Gain = 0, Loss = 1.0M},
                new BinanceCandleStickData(){Gain = 0, Loss = 1.0M},
                new BinanceCandleStickData(){Gain = 0, Loss = 2.6250M}
            };

            indicator.CalculateRSIForFreshData(candles);

            Assert.AreEqual(51.779M, Math.Round(candles[candles.Count - 3].RSI.Value, 3));
            Assert.AreEqual(48.477M, Math.Round(candles[candles.Count - 2].RSI.Value, 3));
            Assert.AreEqual(41.073M, Math.Round(candles[candles.Count - 1].RSI.Value, 3));
        }
    }
}
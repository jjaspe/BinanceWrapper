using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceWrapper
{
    public class BinanceCandleStickData
    {
        public double OpenTime { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
        public double CloseTime { get; set; }
        public decimal QuoteAssetVolume { get; set; }
        public decimal NumberOfTrades { get; set; }

        public decimal TakerBuyBaseAssetVolume { get; set; }

        public decimal? AvgGain { get;  set; }
        public decimal Gain { get;  set; }
        public decimal? AvgLoss { get;  set; }
        public decimal Loss { get;  set; }
        public decimal Change { get;  set; }
        public List<MovingAverage> MAs { get; set; } = new List<MovingAverage>();
        public decimal VolumeMA { get;  set; }
        public decimal? RSI { get { return (100 - 100 / (1 + RS)); } }
        public decimal? RS { get { return (AvgLoss.HasValue && AvgGain.HasValue) ? (AvgGain / AvgLoss) : 0; } } 

        public BinanceCandleStickData()
        {

        }
        public BinanceCandleStickData(JArray stickData)
        {
            OpenTime = (double)stickData[0];
            Open = Decimal.Parse(stickData[1].ToString());
            High = Decimal.Parse(stickData[2].ToString());
            Low = Decimal.Parse(stickData[3].ToString());
            Close = Decimal.Parse(stickData[4].ToString());
            Volume = Decimal.Parse(stickData[5].ToString());
            var change = Close - Open;
            Loss = -1*Math.Min(change, 0);
            Gain = Math.Max(change, 0);
            CloseTime = (double)stickData[6];
        }

        public override string ToString()
        {
            DateTime time = (new DateTime(1970, 1,1)).AddMilliseconds(OpenTime);
            var printOut = time + ": Open:" + Open + ", Close:" + Close + ", Volume:" + Volume;
            return printOut;
        }

        internal string ToString(bool includeRSI)
        {
            var printOut = this.ToString() + ", RSI: " +RSI;
            return printOut;
        }
    }
}
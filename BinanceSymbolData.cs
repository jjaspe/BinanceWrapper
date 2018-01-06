using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceWrapper
{
    public class BinanceSymbolData
    {
        public string Symbol { get; set; }

        public BinanceSymbolData(string symbol)
        {
            this.Symbol = symbol;
        }

        public List<BinanceCandleStickData> Periods { get; set; } = new List<BinanceCandleStickData>();
        public long TimePolled { get; internal set; }

        internal string ToString(bool includeRSI)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var period in Periods)
            {
                builder.AppendLine("Symbol:" + Symbol + "---" + period.ToString(includeRSI));
            }
            return builder.ToString();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach(var period in Periods)
            {
                builder.AppendLine(period.ToString());
            }
            return builder.ToString();
        }
    }
}
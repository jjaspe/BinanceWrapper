using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinanceWrapper
{
    public class PatternService
    {
        BinanceService binanceService;
        public PatternService()
        {
            binanceService = new BinanceService();
        }

        public List<BinanceSymbolData> GetUnderRSISymbols(decimal RSILimit, int numberOfCandles)
        {
            var symbols = binanceService.FetchPopulateBinanceSymbolData(numberOfCandles);
            foreach (var symbol in symbols)
            {
                var periods = symbol.Periods;
                Console.WriteLine(symbol.Symbol + "---" + periods[periods.Count - 3].ToString(true));
            }
            return symbols.Where(n => n.Periods[n.Periods.Count - 3].RSI < RSILimit).ToList();
        }

        public BinanceSymbolData GetUnderRSISymbol(Enums.Symbol symbol ,decimal RSILimit, int numberOfCandles)
        {
            var symbolData = binanceService.FetchPopulateBinanceSymbolData(symbol,numberOfCandles);
            var periods = symbolData.Periods;
            Console.WriteLine(symbolData.Symbol + "---" + periods[periods.Count - 3].ToString(true));
            return symbolData;
        }
    }
}

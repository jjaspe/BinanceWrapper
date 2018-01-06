using System;

namespace BinanceWrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            PatternService patternService = new PatternService();
            patternService.GetUnderRSISymbol(Enums.Symbol.ADXBTC, 30, 200);

        }
    }
}

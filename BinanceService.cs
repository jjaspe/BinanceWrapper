using BinanceExchange.API.Client;
using BinanceExchange.API.Market;
using BinanceExchange.API.Models.Request;
using BinanceExchange.API.Models.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static BinanceWrapper.Enums;

namespace BinanceWrapper
{
    public class BinanceService
    {
        HttpClient client = null;
        IndicatorService indicatorService = null;
        public BinanceService()
        {
            client = new HttpClient();
            indicatorService = new IndicatorService();
        }

        public BinanceSymbolData GetSymbolData(string symbol, string interval, string limit)
        {
            BinanceSymbolData symbolData = new BinanceSymbolData(symbol);
            string url = string.Concat(Constants.binanceBaseUrl, Constants.candleStickEndpoint, "?", "symbol=", symbol, "&interval=", interval, "&limit=", limit);

            HttpResponseMessage response = client.GetAsync(url).GetAwaiter().GetResult();
            //Console.WriteLine("Status Code:" + response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                JArray json = JsonConvert.DeserializeObject<JArray>(result);
                for (int i = 0; i < json.Count; i++)
                {
                    JArray stickData = JsonConvert.DeserializeObject<JArray>(json[i].ToString());
                    symbolData.Periods.Add(new BinanceCandleStickData(stickData));
                }
                symbolData.TimePolled = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            }

            return symbolData;
        }

        public List<BinanceSymbolData> GetAllSymbolData(string interval, string limit)
        {
            List<BinanceSymbolData> symbols = new List<BinanceSymbolData>();

            foreach (Symbol symbol in Enum.GetValues(typeof(Symbol)))
            {
                var symbolData = GetSymbolData(symbol.ToString(), interval, limit);
                indicatorService.PopulateSymbolDataListMetaData(symbolData.Periods);
                if (symbolData.Periods.Last().Open <= .0002m)
                    symbols.Add(symbolData);
            }

            return symbols;
        }

        public List<BinanceSymbolData> FetchPopulateBinanceSymbolData(int limit)
        {
            Console.WriteLine("Currently Fetching Data from Binance...");
            List<BinanceSymbolData> symbols = GetAllSymbolData("30m", limit.ToString());
            int numberOfSymbolsUpdated = symbols.Count;
            for (int i = 0; i < symbols.Count; i++)
            {
                indicatorService.PopulateSymbolDataListMetaData(symbols[i].Periods);
            }
            return symbols;
        }

        public BinanceSymbolData FetchPopulateBinanceSymbolData(Enums.Symbol symbol, int limit)
        {
            Console.WriteLine("Currently Fetching Data from Binance...");
            BinanceSymbolData symbolData = GetSymbolData(symbol.ToString(),"30m", limit.ToString());
            indicatorService.PopulateSymbolDataListMetaData(symbolData.Periods);
            return symbolData;
        }
    }
}

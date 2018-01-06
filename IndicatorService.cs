using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinanceWrapper
{
    public class IndicatorService
    {
        
        public void PopulateSymbolDataListMetaData(List<BinanceCandleStickData> binanceStickData)
        {
            List<int> movingAverageLengths = new List<int>() { 7, 25, 99 };
            CalculateRSIForFreshData(binanceStickData);
            PopulateMovingAverageForFreshData(binanceStickData, movingAverageLengths);
            CalculateVolumeMA(binanceStickData);
        }

        public decimal CalculateChange(BinanceCandleStickData periodToBeCalculated, BinanceCandleStickData previousPeriod)
        {
            return periodToBeCalculated.Close - previousPeriod.Close;
        }

        public decimal CalculateAvgGain(List<BinanceCandleStickData> periods, int periodInterval)
        {
            var previousPeriod = periods[periods.Count - 2];
            var currentPeriod = periods.Last();

            if (previousPeriod.AvgGain == null)
                return periods.Sum(x => x.Gain) / periodInterval;
            else
                return (decimal)((previousPeriod.AvgGain * 13 + currentPeriod.Gain) / periodInterval);
        }

        public decimal CalculateAvgLoss(List<BinanceCandleStickData> periods, int periodInterval)
        {
            var previousPeriod = periods[periods.Count - 2];
            var currentPeriod = periods.Last();

            if (previousPeriod.AvgLoss == null)
                return periods.Sum(x => x.Loss) / periodInterval;
            else
                return (decimal)((previousPeriod.AvgLoss * 13 + currentPeriod.Loss) / periodInterval);
        }

        public void CalculateRSIForFreshData(List<BinanceCandleStickData> binanceStickData)
        {
            int rsiPeriodInterval = 14;
            List<BinanceCandleStickData> currentCalculationSet = new List<BinanceCandleStickData>();
            for (int i = 0; i < binanceStickData.Count; i++)
            {
                if (i > 0)
                    binanceStickData[i].Change = CalculateChange(binanceStickData[i], binanceStickData[i - 1]);
                else
                    binanceStickData[i].Change = 0;
                currentCalculationSet.Add(binanceStickData[i]);

                if (currentCalculationSet.Count >= rsiPeriodInterval)
                {
                    binanceStickData[i].AvgGain = CalculateAvgGain(currentCalculationSet, rsiPeriodInterval);
                    binanceStickData[i].AvgLoss = CalculateAvgLoss(currentCalculationSet, rsiPeriodInterval);
                    currentCalculationSet.RemoveAt(0);
                }
            }
        }

        private void PopulateMovingAverageForFreshData(List<BinanceCandleStickData> periods, List<int> lengths)
        {
            List<BinanceCandleStickData> calculationSet = new List<BinanceCandleStickData>();
            foreach (var length in lengths)
            {
                for (int i = 0; i < periods.Count; i++)
                {
                    calculationSet.Add(periods[i]);
                    if (i >= length - 1)
                    {
                        periods[i].MAs.Add(CalculateMovingAverage(calculationSet, length));
                        calculationSet.RemoveAt(0);
                    }
                }
            }
        }

        private MovingAverage CalculateMovingAverage(List<BinanceCandleStickData> periods, int length)
        {
            return new MovingAverage()
            {
                Length = length,
                Average = periods.Average(x => x.Close)
            };
        }

        private void CalculateVolumeMA(List<BinanceCandleStickData> binanceStickData)
        {
            int volumeMAInterval = 20;
            List<BinanceCandleStickData> volumeMaCalculationSet = new List<BinanceCandleStickData>();
            for (int i = 0; i < binanceStickData.Count; i++)
            {
                volumeMaCalculationSet.Add(binanceStickData[i]);

                if (volumeMaCalculationSet.Count >= volumeMAInterval)
                {
                    binanceStickData[i].VolumeMA = (volumeMaCalculationSet.Sum(x => x.Volume) / volumeMAInterval);
                    volumeMaCalculationSet.RemoveAt(0);
                }
            }
        }
    }
}

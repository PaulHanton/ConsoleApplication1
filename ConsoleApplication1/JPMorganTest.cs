using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Author: Paul Hanton 
/// Description: For the JPMorgan Super Simple Stock Market Assignment.
/// Created: 14/06/2017
/// </summary>
namespace ConsoleApplicationJPM {
    /// <summary>
    /// Default enumarations
    /// </summary>
    public enum JPMSymbol { TEA, POP, ALE, GIN, JOE }
    public enum JPMTypes { Common, Preferred }
    public enum JPMSignal { Buy, Sell }
    /// <summary>
    /// JPM example Global Beverage Corporation Exchange data.
    /// </summary>
    public class JPMData {
        public JPMSymbol Symbol;
        public JPMTypes YieldType;
        public double LastDividend;
        public double FixedDividend;
        public double ParValue;
    }
    /// <summary>
    /// Trading objects.
    /// </summary>
    public class JPMTrade {
        public JPMSymbol Stock;
        public DateTime TimeStamp;
        public Int64 Qty;
        public JPMSignal Signal;
        public Decimal Price;
    }
    public class JPMEngine {
        //314.15,309.45,310.89,318.75,311.83,298.42,278.29,132.55,200.22,433.84,161.80

        /// <summary>
        /// 1.a.ii Record a trade, with timestamp, quantity of shares, buy or sell indicator and traded price
        /// </summary>
        public List<JPMTrade> Trades = new List<JPMTrade> {
            new JPMTrade() { TimeStamp = DateTime.Now.AddMinutes(01), Price = 314.15m, Qty=161, Signal = JPMSignal.Buy, Stock = JPMSymbol.ALE },
            new JPMTrade() { TimeStamp = DateTime.Now.AddMinutes(02), Price = 309.45m, Qty=100, Signal = JPMSignal.Buy, Stock = JPMSymbol.GIN },
            new JPMTrade() { TimeStamp = DateTime.Now.AddMinutes(03), Price = 310.89m, Qty=200, Signal = JPMSignal.Buy, Stock = JPMSymbol.JOE },
            new JPMTrade() { TimeStamp = DateTime.Now.AddMinutes(05), Price = 318.75m, Qty=890, Signal = JPMSignal.Sell, Stock = JPMSymbol.JOE },
            new JPMTrade() { TimeStamp = DateTime.Now.AddMinutes(07), Price = 311.83m, Qty=123, Signal = JPMSignal.Sell, Stock = JPMSymbol.GIN },
            new JPMTrade() { TimeStamp = DateTime.Now.AddMinutes(09), Price = 298.42m, Qty=332, Signal = JPMSignal.Sell, Stock = JPMSymbol.TEA },
            new JPMTrade() { TimeStamp = DateTime.Now.AddMinutes(10), Price = 278.29m, Qty=493, Signal = JPMSignal.Buy, Stock = JPMSymbol.GIN },
            new JPMTrade() { TimeStamp = DateTime.Now.AddMinutes(13), Price = 132.55m, Qty=521, Signal = JPMSignal.Buy, Stock = JPMSymbol.ALE },
            new JPMTrade() { TimeStamp = DateTime.Now.AddMinutes(15), Price = 200.22m, Qty=248, Signal = JPMSignal.Sell, Stock = JPMSymbol.ALE },
            new JPMTrade() { TimeStamp = DateTime.Now.AddMinutes(17), Price = 433.84m, Qty=774, Signal = JPMSignal.Buy, Stock = JPMSymbol.POP },
            new JPMTrade() { TimeStamp = DateTime.Now.AddMinutes(19), Price = 161.80m, Qty=314, Signal = JPMSignal.Buy, Stock = JPMSymbol.POP }
        };
        /// <summary>
        /// Table1. Sample data from the Global Beverage Corporation Exchange
        /// </summary>
        public List<JPMData> GBCEData = new List<JPMData> {
            new JPMData() { Symbol = JPMSymbol.TEA, YieldType = JPMTypes.Common,    LastDividend = 0 ,  FixedDividend = 0, ParValue = 100},
            new JPMData() { Symbol = JPMSymbol.POP, YieldType = JPMTypes.Common,    LastDividend = 8 ,  FixedDividend = 0, ParValue = 100},
            new JPMData() { Symbol = JPMSymbol.ALE, YieldType = JPMTypes.Common,    LastDividend = 23 , FixedDividend = 0, ParValue = 60},
            new JPMData() { Symbol = JPMSymbol.GIN, YieldType = JPMTypes.Preferred, LastDividend = 8 ,  FixedDividend = 2, ParValue = 100},
            new JPMData() { Symbol = JPMSymbol.JOE, YieldType = JPMTypes.Common,    LastDividend = 13 , FixedDividend = 0, ParValue = 250}
        };
        /// <summary>
        /// 1.a.i Given any price as input, calculate the dividend yield
        /// </summary>
        /// <param name="pRowNumber">The row number for the price</param>
        /// <returns>Returns a double value of the yield</returns>
        public double GetYield(int pRowNumber) {
            JPMData lData = GBCEData.Where(x => x.Symbol == Trades[pRowNumber].Stock).First();
            return
                (
                    lData.YieldType == JPMTypes.Common ?
                    (
                        lData.LastDividend == 0 ? 0 :
                        lData.LastDividend / (double)Trades[pRowNumber].Price
                    ) :
                    (
                        lData.FixedDividend == 0 ? 0 :
                        (lData.FixedDividend * lData.ParValue) / (double)Trades[pRowNumber].Price
                    )
                );
        }
        /// <summary>
        /// 1.a.ii Given any price as input, calculate the P/E Ratio
        /// </summary>
        /// <param name="pRowNumber">The row number for the price</param>
        /// <returns>Returns a double value of the ratio</returns>
        public double GetPERatio(int pRowNumber) {
            JPMData lData = GBCEData.Where(x => x.Symbol == Trades[pRowNumber].Stock).First();
            return
                (double)
                Trades[pRowNumber].Price / GetYield(pRowNumber);
        }
        /// <summary>
        /// 1.b Calculate the GBCE All Share Index using the geometric mean of prices for all stocks
        /// </summary>
        /// <returns>Returns a double value of the average</returns>
        public double GeometricMean() {
            return
                Math.Pow(
                    (double)Trades.Aggregate(0m, (x, y) => (x = (x == 0 ? 1 : x) * y.Price)),
                    (double)1 / Trades.Count);
        }
        /// <summary>
        /// 1.a.iv Calculate Volume Weighted Stock Price based on trades in past x minutes
        /// </summary>
        /// <param name="pFromDate">Defines which trades will be used when calculating the weight</param>
        /// <returns>Returns a double value of the range</returns>
        public double VolumeWeight(DateTime pFromDate) {
            List<JPMTrade> volData = new List<JPMTrade>();
            volData.AddRange(Trades.TakeWhile(x => x.TimeStamp < pFromDate));
            return
                (double)
                volData.Sum(x => x.Price * x.Qty) / volData.Sum(x => x.Qty);
        }
    }
}

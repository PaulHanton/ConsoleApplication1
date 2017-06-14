using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationJPM {
    class Program {
        static void Main(string[] args) {
            JPMEngine Engine = new JPMEngine();
            double[] res = new double[6];

            res[0] = (double)Engine.GBCEData.Count;
            res[1] = (double)Engine.Trades.Count;
            res[2] = Engine.GeometricMean();
            res[3] = Engine.GetPERatio(2);
            res[4] = Engine.GetYield(4);
            res[5] = Engine.VolumeWeight(DateTime.Now.AddMinutes(15));

            foreach(double item in res) {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
    }
}

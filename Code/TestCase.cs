using System;
using System.Collections.Generic;
using System.Text;

namespace HashTable
{
    public class TestCase
    {
        public int htSize { get; }
        public double cellar { get; }
        public int dataMin { get; }
        public int dataMax { get; }
        public int dataStep { get; }
        public int testCount { get; set; }
        public int fillCache { get; set; }

        public TestCase(int htSize, double cellar, int dataMin, int dataMax, int dataStep, int testCount, int fillSize)
        {
            this.htSize = htSize;
            this.cellar = cellar;
            this.dataMin = dataMin;
            this.dataMax = dataMax;
            this.dataStep = dataStep;
            this.testCount = testCount;
            this.fillCache = fillSize;
        }
        public TestCase(string[] args)
        {
            htSize = args.Length != 7 ? 521 : Convert.ToInt32(args[0]);
            cellar = args.Length != 7 ? 0.00 : Convert.ToDouble(args[1]);
            dataMin = args.Length != 7 ? 21 : Convert.ToInt32(args[2]);
            dataMax = args.Length != 7 ? 521 : Convert.ToInt32(args[3]);
            dataStep = args.Length != 7 ? 25 : Convert.ToInt32(args[4]);
            testCount = args.Length != 7 ? 100 : Convert.ToInt32(args[5]);
            fillCache = args.Length != 7 ? 0 : Convert.ToInt32(args[6]) * 1024 * 256;
        }
        public string ToStringFull()
        {
            string line = $"{htSize:f0}; " +
                $"{cellar:f3}; " +
                $"{dataMin:f0}; " +
                $"{dataMax:f0}; " +
                $"{dataStep:f0}; " +
                $"{testCount:f0}; " +
                $"{fillCache:f0}; " +
                $"";
            return line;
        }
    }
}

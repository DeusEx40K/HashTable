using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace DictionaryTest
{
    class Program
    {
        private static Random rnd = new Random(1234567);
        private static byte[] trash = new byte[32 * 1024 * 1024];
        private static Stopwatch insertTimer = new Stopwatch();
        private static Stopwatch includeTimer = new Stopwatch();
        private static Stopwatch excludeTimer = new Stopwatch();
        private static Stopwatch totalTimer = new Stopwatch();
        private static Stopwatch localTimer = new Stopwatch();

        public static void Main(string[] args)
        {
            totalTimer.Start();

            const int dataMin = 21;
            const int dataMax = 521;
            const int dataStep = 25;
            const int testCount = 100;
            int fillSize = (args.Length == 0 ? 0 : Convert.ToInt32(args[0])) * 1024;

            SetProcessAffinityHigh(1);
            var hashTable = new Dictionary<string, string>(dataMax);

            using (var file = new StreamWriter($"result {fillSize}.csv", false, Encoding.UTF8))
            {
                var line = $"fillSize; {fillSize}\nкоэффициент заполнения таблицы; " +
                    $"общ.время вставки, ticks; ср.время вставки, ticks; " +
                    $"общ.время успешного поиска, ticks; ср.время успешного поиска, ticks; " +
                    $"общ.время неуспешного поиска, ticks; ср.время неуспешного поиска, ticks; " +
                    $"время выполнения, ms; ";
                Console.WriteLine(line);
                file.WriteLine(line);
                file.Flush();

                for (int dataSize = dataMin; dataSize <= dataMax; dataSize += dataStep)
                {
                    insertTimer.Reset();
                    includeTimer.Reset();
                    excludeTimer.Reset();
                    GC.Collect();

                    int insertCount = 0, includeCount = 0, excludeCount = 0;
                    localTimer.Restart();
                    for (int test = 0; test < testCount; test++)
                    {
                        hashTable.Clear();
                        var keysInclude = KeyUnique(rnd, dataSize);
                        var keysExclude = KeyUnique(rnd, keysInclude);

                        for (int i = 0; i < keysInclude.Length; i++)
                        {
                            FillCache(fillSize);
                            insertTimer.Start();
                            hashTable.Add(keysInclude[i], null);
                            insertTimer.Stop();
                            insertCount++;
                        }

                        for (int i = 0; i < keysInclude.Length; i++)
                        {
                            FillCache(fillSize);
                            includeTimer.Start();
                            hashTable.ContainsKey(keysInclude[i]);
                            includeTimer.Stop();
                            includeCount++;
                        }

                        for (int i = 0; i < keysExclude.Length; i++)
                        {
                            FillCache(fillSize);
                            excludeTimer.Start();
                            hashTable.ContainsKey(keysExclude[i]);
                            excludeTimer.Stop();
                            excludeCount++;
                        }
                    }
                    localTimer.Stop();

                    line = $"{(double)dataSize / dataMax:f3}; " +
                        $"{insertTimer.ElapsedTicks:d}; {(double)insertTimer.ElapsedTicks / insertCount:f2}; " +
                        $"{includeTimer.ElapsedTicks:d}; {(double)includeTimer.ElapsedTicks / includeCount:f2}; " +
                        $"{excludeTimer.ElapsedTicks:d}; {(double)excludeTimer.ElapsedTicks / excludeCount:f2}; " +
                        $"{localTimer.ElapsedMilliseconds:d}; ";
                    Console.WriteLine(line);
                    file.WriteLine(line);
                    file.Flush();
                }

                line = $"elapsed time; {totalTimer.Elapsed.Hours:d2}:{totalTimer.Elapsed.Minutes:d2}:{totalTimer.Elapsed.Seconds:d2}";
                Console.WriteLine(line);
                file.WriteLine(line);
                file.Flush();
            }
        }

        private static void SetProcessAffinityHigh(int core)
        {
            Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(core);
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
        }
        private static void FillCache(int length)
        {
            var mask = trash.Length - 1;
            unchecked
            {
                for (int i = 0; i < length; i += 64)
                {
                    trash[i & mask]++;
                }
            }
        }
        private static string[] KeyUnique(Random rnd, int size)
        {
            var result = new string[size];
            var set = new HashSet<string>(size);
            int index = 0;

            while (index < size)
            {
                var key = rnd.Next(int.MinValue, int.MaxValue).ToString();
                if (!set.Contains(key))
                {
                    set.Add(key);
                    result[index] = key;
                    index++;
                }
            }

            return result;
        }
        private static string[] KeyUnique(Random rnd, string[] except)
        {
            var result = new string[except.Length];
            var setExcept = new HashSet<string>(except);
            var set = new HashSet<string>(except.Length);
            int index = 0;

            while (index < except.Length)
            {
                var key = rnd.Next(int.MinValue, int.MaxValue).ToString();
                if (!set.Contains(key) && !setExcept.Contains(key))
                {
                    set.Add(key);
                    result[index] = key;
                    index++;
                }
            }

            return result;
        }
    }
}

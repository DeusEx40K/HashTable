using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace HashTable
{
    public class Test
    {
        public static readonly int Seed = 1234567;
        private static Stopwatch totalTimer = new Stopwatch();
        private static Stopwatch insertTimer = new Stopwatch();
        private static Stopwatch includeTimer = new Stopwatch();
        private static Stopwatch excludeTimer = new Stopwatch();

        public void RunView(IHashTable ht, Random rnd, int size)
        {
            var keysInsert = KeyUnique(rnd, size);
            var keysInclude = new string[keysInsert.Length];

            int index = 0;
            for (int i = 0; i < keysInsert.Length; i++)
            {
                var result = ht.Insert(keysInsert[i], keysInsert[i]);
                if (result == 0)
                {
                    keysInclude[index] = keysInsert[i];
                    index++;
                }
                else
                {
                    Console.WriteLine($"Insert error {keysInsert[i]} = {result}");
                }
            }

            Array.Resize(ref keysInclude, index);
            for (int i = 0; i < keysInclude.Length; i++)
            {
                var result = ht.Search(keysInclude[i]);
                if (result != 0)
                {
                    Console.WriteLine($"Search include {keysInsert[i]} = {result}");
                }
            }

            var keysExclude = KeyUnique(rnd, keysInclude);
            for (int i = 0; i < keysExclude.Length; i++)
            {
                var result = ht.Search(keysExclude[i]);
                if (result == 0)
                {
                    Console.WriteLine($"Search exclude {keysInsert[i]} = {result}");
                }
            }
        }
        public void RunProbe(IHashTable ht, Random rnd, int size)
        {
            var keysInsert = KeyUnique(rnd, size);
            var keysInclude = new string[keysInsert.Length];

            int index = 0;
            for (int i = 0; i < keysInsert.Length; i++)
            {
                var result = ht.InsertProbe(keysInsert[i], keysInsert[i]);
                if (result.Result == 0)
                {
                    keysInclude[index] = keysInsert[i];
                    index++;
                }
                else
                {
                    Console.WriteLine($"Insert error {keysInsert[i]} = {result.Result}");
                }
            }

            Array.Resize(ref keysInclude, index);
            for (int i = 0; i < keysInclude.Length; i++)
            {
                var result = ht.SearchProbe(keysInclude[i]);
                if (result.Result != 0)
                {
                    Console.WriteLine($"Search include {keysInsert[i]} = {result.Result}");
                }
            }

            var keysExclude = KeyUnique(rnd, keysInclude);
            for (int i = 0; i < keysExclude.Length; i++)
            {
                var result = ht.SearchProbe(keysExclude[i]);
                if (result.Result == 0)
                {
                    Console.WriteLine($"Search exclude {keysInsert[i]} = {result.Result}");
                }
            }
        }
        public void RunTest(string TestName, string FileName, IHashTable ht, Random rnd, TestCase testcase)
        {
            using (var file = new StreamWriter(FileName, false, Encoding.UTF8))
            {
                totalTimer.Start();

                #region Header
                var line = testcase.ToStringFull() + $"{ht.Capacity}; " + $"{ht.Capacity * ht.EntrySize}; ";
                Console.WriteLine(line);
                file.WriteLine(line);

                var head = $"fill factor; " +
                    $"{TestName}(вставка), кол.проб; " +
                    $"{TestName}(вставка), ср.кол.проб; " +
                    $"{TestName}(вставка), кол.тиков; " +
                    $"{TestName}(вставка), ср.кол.тиков; " +
                    $"{TestName}(успеш.поиск), кол.проб; " +
                    $"{TestName}(успеш.поиск), ср.кол.проб; " +
                    $"{TestName}(успеш.поиск), кол.тиков; " +
                    $"{TestName}(успеш.поиск), ср.кол.тиков; " +
                    $"{TestName}(неусп.поиск), кол.проб; " +
                    $"{TestName}(неусп.поиск), ср.кол.проб; " +
                    $"{TestName}(неусп.поиск), кол.тиков; " +
                    $"{TestName}(неусп.поиск), ср.кол.тиков; " +
                    $"";
                Console.WriteLine(head);
                file.WriteLine(head);
                file.Flush();
                #endregion

                for (int data = testcase.dataMin; data <= testcase.dataMax; data += testcase.dataStep)
                {
                    #region Initial
                    insertTimer.Reset();
                    includeTimer.Reset();
                    excludeTimer.Reset();
                    int insertProbe = 0, insertCount = 0;
                    int includeProbe = 0, includeCount = 0;
                    int excludeProbe = 0, excludeCount = 0;
                    GC.Collect();
                    #endregion

                    for (int test = 0; test < testcase.testCount; test++)
                    {
                        ht.Clear();

                        var keysInsert = KeyUnique(rnd, data);
                        var keysInclude = new string[keysInsert.Length];

                        int index = 0;
                        for (int i = 0; i < keysInsert.Length; i++)
                        {
                            var result = ht.InsertProbe(keysInsert[i], keysInsert[i]);
                            if (result.Result == 0)
                            {
                                keysInclude[index] = keysInsert[i];
                                index++;
                                insertProbe += result.Probe;
                                insertCount++;
                            }
                            else
                            {
                                //throw new Exception($"Insert error {keysInsert[i]} = {result}");
                            }
                        }

                        Array.Resize(ref keysInclude, index);
                        for (int i = 0; i < keysInclude.Length; i++)
                        {
                            var result = ht.SearchProbe(keysInclude[i]);
                            if (result.Result != 0)
                            {
                                //throw new Exception($"Search include {keysInsert[i]} = {result}");
                            }
                            includeProbe += result.Probe;
                            includeCount++;
                        }

                        var keysExclude = KeyUnique(rnd, keysInclude);
                        for (int i = 0; i < keysExclude.Length; i++)
                        {
                            var result = ht.SearchProbe(keysExclude[i]);
                            if (result.Result == 0)
                            {
                                //throw new Exception($"Search exclude {keysInsert[i]} = {result}");
                            }
                            excludeProbe += result.Probe;
                            excludeCount++;
                        }

                        ht.Clear();

                        for (int i = 0; i < keysInclude.Length; i++)
                        {
                            Ext.FillCache(testcase.fillSize);
                            insertTimer.Start();
                            ht.Insert(keysInclude[i], keysInclude[i]);
                            insertTimer.Stop();
                        }

                        for (int i = 0; i < keysInclude.Length; i++)
                        {
                            Ext.FillCache(testcase.fillSize);
                            includeTimer.Start();
                            ht.Search(keysInclude[i]);
                            includeTimer.Stop();
                        }

                        for (int i = 0; i < keysExclude.Length; i++)
                        {
                            Ext.FillCache(testcase.fillSize);
                            excludeTimer.Start();
                            ht.Search(keysExclude[i]);
                            excludeTimer.Stop();
                        }
                    }

                    #region Writer
                    var rec = $"{(double)insertCount / ht.Capacity / testcase.testCount:f3}; " +
                        $"{insertProbe:d}; " +
                        $"{(double)insertProbe / insertCount:f2}; " +
                        $"{insertTimer.ElapsedTicks:d}; " +
                        $"{(double)insertTimer.ElapsedTicks / insertCount:f2}; " +
                        $"{includeProbe:d}; " +
                        $"{(double)includeProbe / includeCount:f2}; " +
                        $"{includeTimer.ElapsedTicks:d}; " +
                        $"{(double)includeTimer.ElapsedTicks / includeCount:f2}; " +
                        $"{excludeProbe:d}; " +
                        $"{(double)excludeProbe / excludeCount:f2}; " +
                        $"{excludeTimer.ElapsedTicks:d}; " +
                        $"{(double)excludeTimer.ElapsedTicks / excludeCount:f2}; " +
                        $"";
                    Console.WriteLine(rec);
                    file.WriteLine(rec);
                    file.Flush();
                    #endregion
                }

                totalTimer.Stop();
                var time = $"time; {totalTimer.Elapsed.Hours:d2}:{totalTimer.Elapsed.Minutes:d2}:{totalTimer.Elapsed.Seconds:d2}; {totalTimer.ElapsedMilliseconds}";
                Console.WriteLine(time);
                file.WriteLine(time);
                file.Flush();
            }
        }

        internal static string[] KeyUnique(Random rnd, int size)
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
        internal static string[] KeyUnique(Random rnd, string[] keys)
        {
            var result = new string[keys.Length];
            var setInclude = new HashSet<string>(keys);
            var setExclude = new HashSet<string>(keys.Length);
            int index = 0;

            while (index < keys.Length)
            {
                var key = rnd.Next(int.MinValue, int.MaxValue).ToString();
                if (!setExclude.Contains(key) && !setInclude.Contains(key))
                {
                    setExclude.Add(key);
                    result[index] = key;
                    index++;
                }
            }

            return result;
        } 
    }
}

using System;
using HashTable;

namespace OA_SimpleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Ext.SetProcessAffinityHigh(3);
            var rnd = new Random(Test.Seed);
            var testcase = new TestCase(args);

            var ht = new OA_Simple(testcase.htSize);
            var test = new Test();
            test.RunTest(ht.Name, $"{ht.Name} {testcase.fillSize / 1024}.csv", ht, rnd, testcase);
        }
    }
}

﻿using System;
using HashTable;

namespace SC_ObjectTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Ext.SetProcessAffinityHigh(3);
            var rnd = new Random(Test.Seed);
            var testcase = new TestCase(args);

            var ht = new SC_Object(testcase.htSize);
            var test = new Test();
            test.RunTest(ht.Name, $"{ht.Name} {testcase.fillCache / 1024}.csv", ht, rnd, testcase);

        }
    }
}

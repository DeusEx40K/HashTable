using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashTable;

namespace SC_StructView
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = 23;
            var rnd = new Random(Test.Seed);
            var ht = new SC_Struct(size);
            var test = new Test();
            //test.RunView(ht, rnd, ht.Capacity);
            test.RunProbe(ht, rnd, ht.Capacity);
            ht.Print();
            Console.Write("...");
            Console.ReadLine();
        }
    }
}

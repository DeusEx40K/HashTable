using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashTable;

namespace OA_SimpleView
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = 20;
            var rnd = new Random(Test.Seed);
            var ht = new OA_Simple(size);
            var test = new Test();
            test.RunView(ht, rnd, ht.Capacity);
            //test.RunProbe(ht, rnd, ht.Capacity);
            ht.Print();
            Console.Write("...");
            Console.ReadLine();
        }
    }
}

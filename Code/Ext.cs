using System;
using System.Diagnostics;
using System.Threading;

namespace HashTable
{
    public static class Ext
    {
        public static void SetProcessAffinityHigh(int core)
        {
            Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(core);
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
        }

        private static byte[] trash = new byte[32 * 1024 * 1024];
        public static void FillCache(int length)
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

        public static void Shuffle<T>(Random rnd, T[] arr)
        {
            for (int i = arr.Length - 1; i >= 1; i--)
            {
                int j = rnd.Next(i + 1);
                T tmp = arr[j];
                arr[j] = arr[i];
                arr[i] = tmp;
            }
        }

        public static string RandomStringEng(Random rnd, int length)
        {
            var charEng = " ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz 0123456789 ";
            var charArray = new char[length];

            for (int i = 0; i < charArray.Length; i++)
            {
                charArray[i] = charEng[rnd.Next(charEng.Length)];
            }

            return new string(charArray);
        }

    }

}

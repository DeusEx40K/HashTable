using System;

namespace HashTable
{
    /// <summary>
    ///  Separate Chaining with Object Entry
    /// </summary>
    public class SC_Object: IHashTable
    {
        private const int OnlyPositiveNumbersMask = 0x7FFFFFFF;
        internal SC_ObjectEntry[] buckets;

        public string Name { get { return "SC_Object"; } }
        public int Count { get; private set; }
        public int Capacity { get; private set; }
        public int EntrySize { get { return 16 + sizeof(int); } }

        public SC_Object(int size)
        {
            Capacity = size; // HtHelper.GetPrime(size);
            buckets = new SC_ObjectEntry[Capacity];
        }
        public void Clear()
        {
            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = null;
            }
            Count = 0;
        }
        public void Print()
        {
            Console.WriteLine("\n№; keys");
            for (int i = 0; i < buckets.Length; i++)
            {
                if (buckets[i] != null)
                {
                    Console.Write($"{i}; ");
                    var entry = buckets[i];
                    do
                    {
                        Console.Write($"{entry.key}; ");
                        entry = entry.next;
                    } while (entry != null);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"{i};");
                }
            }
            Console.WriteLine();
        }

        public ResultType Insert(string key, string value)
        {
            int hashCode = key.GetHashCode() & OnlyPositiveNumbersMask;
            int location = hashCode % buckets.Length;
            var entry = buckets[location];

            while (entry != null)
            {
                if (entry.hashCode == hashCode && string.Equals(entry.key, key))
                {
                    return ResultType.DublicateKey;
                }
                entry = entry.next;
            }

            var node = new SC_ObjectEntry(hashCode, key, value);
            node.next = buckets[location];
            buckets[location] = node;
            Count++;

            return ResultType.Okey;
        }
        public ResultItem InsertProbe(string key, string value)
        {
            ResultItem result = new ResultItem();
            int hashCode = key.GetHashCode() & OnlyPositiveNumbersMask;
            int location = hashCode % buckets.Length;
            var entry = buckets[location];

            while (true)
            {
                result.Probe++;
                if (entry == null)
                {
                    break;
                }
                if (entry.hashCode == hashCode && string.Equals(entry.key, key))
                {
                    result.Result = ResultType.DublicateKey;
                    return result;
                }
                entry = entry.next;
            }

            var node = new SC_ObjectEntry(hashCode, key, value);
            node.next = buckets[location];
            buckets[location] = node;
            Count++;

            result.Result = ResultType.Okey;
            return result;
        }
        public ResultType Search(string key)
        {
            int hashCode = key.GetHashCode() & OnlyPositiveNumbersMask;
            int location = hashCode % buckets.Length;
            var entry = buckets[location];

            while (entry != null)
            {
                if (entry.hashCode == hashCode && string.Equals(entry.key, key))
                {
                    return ResultType.Okey;
                }
                entry = entry.next;
            }

            return ResultType.NotFound;
        }
        public ResultItem SearchProbe(string key)
        {
            ResultItem result = new ResultItem();
            int hashCode = key.GetHashCode() & OnlyPositiveNumbersMask;
            int location = hashCode % buckets.Length;
            var entry = buckets[location];

            while (true)
            {
                result.Probe++;
                if (entry == null)
                {
                    break;
                }
                if (entry.hashCode == hashCode && string.Equals(entry.key, key))
                {
                    result.Result = ResultType.Okey;
                    return result;
                }
                entry = entry.next;
            }

            result.Result = ResultType.NotFound;
            return result;
        }
        public ResultType Delete(string key)
        {
            int hashCode = key.GetHashCode() & OnlyPositiveNumbersMask;
            int location = hashCode % buckets.Length;
            throw new NotImplementedException();
        }

        public ResultItem DeleteProbe(string key)
        {
            int hashCode = key.GetHashCode() & OnlyPositiveNumbersMask;
            int location = hashCode % buckets.Length;
            throw new NotImplementedException();
        }
    }

    internal class SC_ObjectEntry
    {
        public int hashCode { get; private set; }
        public SC_ObjectEntry next;
        public string key { get; private set; }
        public string value { get; private set; }

        public SC_ObjectEntry(int hashCode, string key, string value)
        {
            this.hashCode = hashCode;
            this.key = key;
            this.value = value;
        }
    }

}

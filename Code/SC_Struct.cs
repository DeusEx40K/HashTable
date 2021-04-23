using System;
using System.Runtime.InteropServices;

namespace HashTable
{
    /// <summary>
    ///  Separate Chaining with Struct Entry
    /// </summary>
    public class SC_Struct : IHashTable
    {
        private const int OnlyPositiveNumbersMask = 0x7FFFFFFF;
        private const int EntryEmpty = -1;

        internal int[] buckets;
        internal SC_StructEntry[] entries;
        internal int count;
        internal int freeList;
        internal int freeCount;

        public string Name { get { return "SC_Struct"; } }
        public int Count { get { return count - freeCount; } }
        public int Capacity { get; private set; }
        public int EntrySize { get { return Marshal.SizeOf(typeof(SC_StructEntry)) + sizeof(int); } }

        public SC_Struct(int size)
        {
            Capacity = size; // HtHelper.GetPrime(size);
            buckets = new int[Capacity];
            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = EntryEmpty;
            }
            entries = new SC_StructEntry[Capacity];
            freeList = -1;
        }
        public void Clear()
        {
            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = EntryEmpty;
            }
            entries = new SC_StructEntry[Capacity];
            freeList = -1;
            count = 0;
            freeCount = 0;
        }
        public void Print()
        {
            Console.WriteLine("\n№; index: key");
            for (int i = 0; i < buckets.Length; i++)
            {
                if (buckets[i] != EntryEmpty)
                {
                    var bucket = buckets[i];
                    if (bucket != EntryEmpty)
                    {
                        Console.Write($"{i}; ");
                        for (int index = bucket; index >= 0; index = entries[index].next)
                        {
                            Console.Write($"{index}: {entries[index].key}; ");
                        }
                        Console.WriteLine("");
                    }
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

            for (int index = buckets[location]; index >= 0; index = entries[index].next)
            {
                if (entries[index].hashCode == hashCode && string.Equals(entries[index].key, key))
                {
                    return ResultType.DublicateKey;
                }
            }

            int position;
            if (freeCount > 0)
            {
                position = freeList;
                freeList = entries[position].next;
                freeCount--;
            }
            else
            {
                if (count == entries.Length)
                {
                    return ResultType.TableExceeded;
                }
                position = count;
                count++;
            }

            entries[position].hashCode = hashCode;
            entries[position].next = buckets[location];
            entries[position].key = key;
            entries[position].value = value;
            buckets[location] = position;

            return ResultType.Okey;
        }

        public ResultItem InsertProbe(string key, string value)
        {
            ResultItem result = new ResultItem();
            int hashCode = key.GetHashCode() & OnlyPositiveNumbersMask;
            int location = hashCode % buckets.Length;

            int index = buckets[location];
            while (true)
            {
                result.Probe++;
                if (index < 0)
                {
                    break;
                }
                if (entries[index].hashCode == hashCode && string.Equals(entries[index].key, key))
                {
                    result.Result = ResultType.DublicateKey;
                    return result;
                }
                index = entries[index].next;
            } 

            int position;
            if (freeCount > 0)
            {
                position = freeList;
                freeList = entries[position].next;
                freeCount--;
            }
            else
            {
                if (count == entries.Length)
                {
                    result.Result = ResultType.TableExceeded;
                    return result;
                }
                position = count;
                count++;
            }

            entries[position].hashCode = hashCode;
            entries[position].next = buckets[location];
            entries[position].key = key;
            entries[position].value = value;
            buckets[location] = position;

            result.Result = ResultType.Okey;
            return result;
        }

        public ResultType Search(string key)
        {
            int hashCode = key.GetHashCode() & OnlyPositiveNumbersMask;
            int location = hashCode % buckets.Length;

            for (int index = buckets[location]; index >= 0; index = entries[index].next)
            {
                if (entries[index].hashCode == hashCode && string.Equals(entries[index].key, key))
                {
                    return ResultType.Okey;
                }
            }

            return ResultType.NotFound;
        }

        public ResultItem SearchProbe(string key)
        {
            ResultItem result = new ResultItem();
            int hashCode = key.GetHashCode() & OnlyPositiveNumbersMask;
            int location = hashCode % buckets.Length;

            int index = buckets[location];
            do
            {
                result.Probe++;
                if (index < 0)
                {
                    break;
                }
                if (entries[index].hashCode == hashCode && string.Equals(entries[index].key, key))
                {
                    result.Result = ResultType.Okey;
                    return result;
                }
                index = entries[index].next;
            } while (true);

            result.Result = ResultType.NotFound;
            return result;
        }

        public ResultType Delete(string key)
        {
            throw new NotImplementedException();
        }

        public ResultItem DeleteProbe(string key)
        {
            throw new NotImplementedException();
        }
    }

    internal struct SC_StructEntry
    {
        public int hashCode;
        public int next;
        public string key;
        public string value;
    }

}

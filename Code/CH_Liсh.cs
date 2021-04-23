using System;
using System.Runtime.InteropServices;

namespace HashTable
{
    /// <summary>
    /// Late Insertion Coalesced Hashing
    /// </summary>
    public class CH_Liсh : IHashTable
    {
        private const int OnlyPositiveNumbersMask = 0x7FFFFFFF;
        private const int EntryEmpty = -1;
        internal CH_LiсhEntry[] entries;
        internal int lastInsert;
        internal int size;

        public string Name { get { return "CH_Lich"; } }
        public int Count { get; private set; }
        public int Capacity { get; private set; }
        public int EntrySize { get { return Marshal.SizeOf(typeof(CH_LiсhEntry)); } }

        public CH_Liсh(int size, double cellarFactor)
        {
            this.size = size; // HtHelper.GetPrime(size);
            Capacity = this.size + (int)(this.size * cellarFactor + 0.4999);
            entries = new CH_LiсhEntry[Capacity];
            for (int i = 0; i < entries.Length; i++)
            {
                entries[i].hashCode = EntryEmpty;
            }
            lastInsert = entries.Length;
        }
        public void Clear()
        {
            entries = new CH_LiсhEntry[Capacity];
            for (int i = 0; i < entries.Length; i++)
            {
                entries[i].hashCode = EntryEmpty;
            }
            lastInsert = entries.Length;
            Count = 0;
        }
        public void Print()
        {
            Console.WriteLine("\n№; key; next");
            for (int i = 0; i < entries.Length; i++)
            {
                Console.WriteLine($"{i}; {entries[i].key}; {entries[i].next} ");
            }
            Console.WriteLine($"lastInsert: {lastInsert} \n");
        }

        public ResultType Insert(string key, string value)
        {
            int hashCode = key.GetHashCode() & OnlyPositiveNumbersMask;
            int location = hashCode % size;

            if (entries[location].hashCode != EntryEmpty)
            {
                do
                {
                    if (entries[location].hashCode == hashCode && string.Equals(entries[location].key, key))
                    {
                        return ResultType.DublicateKey;
                    }
                    if (entries[location].next == EntryEmpty)
                    {
                        break;
                    }
                    location = entries[location].next;
                } while (true);

                int index = lastInsert;
                do
                {
                    index--;
                    if (index < 0)
                    {
                        return ResultType.TableExceeded;
                    }
                } while (entries[index].hashCode != EntryEmpty);

                lastInsert = index;
                entries[location].next = index;
                location = index;
            }

            entries[location].hashCode = hashCode;
            entries[location].next = EntryEmpty;
            entries[location].key = key;
            entries[location].value = value;
            Count++;

            return ResultType.Okey;
        }

        public ResultItem InsertProbe(string key, string value)
        {
            ResultItem result = new ResultItem();
            int hashCode = key.GetHashCode() & OnlyPositiveNumbersMask;
            int location = hashCode % size;

            result.Probe++;
            if (entries[location].hashCode != EntryEmpty)
            {
                do
                {
                    if (entries[location].hashCode == hashCode && string.Equals(entries[location].key, key))
                    {
                        result.Result = ResultType.DublicateKey;
                        return result;
                    }
                    if (entries[location].next == EntryEmpty)
                    {
                        break;
                    }
                    result.Probe++;
                    location = entries[location].next;
                } while (true);

                int index = lastInsert;
                do
                {
                    result.Probe++;
                    index--;
                    if (index < 0)
                    {
                        result.Result = ResultType.TableExceeded;
                        return result;
                    }
                } while (entries[index].hashCode != EntryEmpty);

                lastInsert = index;
                entries[location].next = index;
                location = index;
            }

            entries[location].hashCode = hashCode;
            entries[location].next = EntryEmpty;
            entries[location].key = key;
            entries[location].value = value;
            Count++;

            result.Result = ResultType.Okey;
            return result;
        }

        public ResultType Search(string key)
        {
            int hashCode = key.GetHashCode() & OnlyPositiveNumbersMask;
            int location = hashCode % size;

            if (entries[location].hashCode != EntryEmpty)
            {
                do
                {
                    if (entries[location].hashCode == hashCode && string.Equals(entries[location].key, key))
                    {
                        return ResultType.Okey;
                    }
                    if (entries[location].next == EntryEmpty)
                    {
                        break;
                    }
                    location = entries[location].next;
                } while (true);
            }

            return ResultType.NotFound;
        }

        public ResultItem SearchProbe(string key)
        {
            ResultItem result = new ResultItem();
            int hashCode = key.GetHashCode() & OnlyPositiveNumbersMask;
            int location = hashCode % size;

            result.Probe++;
            if (entries[location].hashCode != EntryEmpty)
            {
                do
                {
                    if (entries[location].hashCode == hashCode && string.Equals(entries[location].key, key))
                    {
                        result.Result = ResultType.Okey;
                        return result;
                    }
                    if (entries[location].next == EntryEmpty)
                    {
                        break;
                    }
                    result.Probe++;
                    location = entries[location].next;
                } while (true);
            }

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

    internal struct CH_LiсhEntry
    {
        public int hashCode; // Lower 31 bits of hash code, -1 if unused
        public int next; // Index of next entry, -1 if last
        public string key; // Key of entry
        public string value; // Value of entry
    }

}

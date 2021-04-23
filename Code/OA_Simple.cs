using System;
using System.Runtime.InteropServices;

namespace HashTable
{
    /// <summary>
    /// Open Addressing Simple
    /// </summary>
    public class OA_Simple : IHashTable
    {
        private const int OnlyPositiveNumbersMask = 0x7FFFFFFF;
        private const int EntryEmpty = -1;
        private const int EntryDeleted = -2;
        internal OA_SimpleEntry[] entries;

        public string Name { get { return "OA_Simple"; } }
        public int Count { get; private set; }
        public int Capacity { get; private set; }
        public int EntrySize { get { return Marshal.SizeOf(typeof(OA_SimpleEntry)); } }

        public OA_Simple(int size)
        {
            Capacity = size; // HtHelper.GetPrime(size);
            entries = new OA_SimpleEntry[Capacity];
            for (int i = 0; i < entries.Length; i++)
            {
                entries[i].hashCode = EntryEmpty;
            }
        }
        public void Clear()
        {
            for (int i = 0; i < entries.Length; i++)
            {
                entries[i].hashCode = EntryEmpty;
            }
            Count = 0;
        }
        public void Print()
        {
            Console.WriteLine("\n№; PSL (probe sequence length); key");
            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].hashCode >= 0)
                {
                    int hashCode = entries[i].key.GetHashCode() & OnlyPositiveNumbersMask;
                    int location = hashCode % entries.Length;
                    Console.WriteLine($"{i}; {i - location}; {entries[i].key}; ");
                }
                else
                {
                    Console.WriteLine($"{i}; -1;; ");
                }
            }
            Console.WriteLine();
        }

        public ResultType Insert(string key, string value)
        {
            int hashCode = key.GetHashCode() & OnlyPositiveNumbersMask;
            int location = hashCode % entries.Length;

            int deleted = EntryDeleted;
            while (entries[location].hashCode != EntryEmpty)
            {
                if (entries[location].hashCode == EntryDeleted)
                {
                    if (deleted == EntryDeleted)
                    {
                        deleted = location;
                    }
                }
                else
                {
                    if (entries[location].hashCode == hashCode && string.Equals(entries[location].key, key))
                    {
                        return ResultType.DublicateKey;
                    }
                }
                location++;
                if (location == entries.Length)
                {
                    return ResultType.TableExceeded;
                }
            }

            if (deleted != EntryDeleted)
            {
                location = deleted;
            }

            entries[location].hashCode = hashCode;
            entries[location].key = key;
            entries[location].value = value;
            Count++;

            return ResultType.Okey;
        }

        public ResultItem InsertProbe(string key, string value)
        {
            ResultItem result = new ResultItem();
            int hashCode = key.GetHashCode() & OnlyPositiveNumbersMask;
            int location = hashCode % entries.Length;

            int deleted = EntryDeleted;
            do
            {
                result.Probe++;
                if (entries[location].hashCode == EntryEmpty)
                {
                    break;
                }
                if (entries[location].hashCode == EntryDeleted)
                {
                    if (deleted == EntryDeleted)
                    {
                        deleted = location;
                    }
                }
                else
                {
                    if (entries[location].hashCode == hashCode && string.Equals(entries[location].key, key))
                    {
                        result.Result = ResultType.DublicateKey;
                        return result;
                    }
                }
                location++;
                if (location == entries.Length)
                {
                    result.Result = ResultType.TableExceeded;
                    return result;
                }
            } while (true);

            if (deleted != EntryDeleted)
            {
                location = deleted;
            }

            entries[location].hashCode = hashCode;
            entries[location].key = key;
            entries[location].value = value;
            Count++;

            result.Result = ResultType.Okey;
            return result;
        }

        public ResultType Search(string key)
        {
            int hashCode = key.GetHashCode() & OnlyPositiveNumbersMask;
            int location = hashCode % entries.Length;

            do
            {
                if (entries[location].hashCode == EntryEmpty)
                {
                    break;
                }
                if (entries[location].hashCode == hashCode && string.Equals(entries[location].key, key))
                {
                    return ResultType.Okey;
                }
                location++;
            } while (location < entries.Length);

            return ResultType.NotFound;
        }

        public ResultItem SearchProbe(string key)
        {
            ResultItem result = new ResultItem();
            int hashCode = key.GetHashCode() & OnlyPositiveNumbersMask;
            int location = hashCode % entries.Length;

            do
            {
                result.Probe++;
                if (entries[location].hashCode == EntryEmpty)
                {
                    break;
                }
                if (entries[location].hashCode == hashCode && string.Equals(entries[location].key, key))
                {
                    result.Result = ResultType.Okey;
                    return result;
                }
                location++;
            } while (location < entries.Length);

            result.Result = ResultType.NotFound;
            return result;
        }

        public ResultType Delete(string key)
        {
            int hashCode = key.GetHashCode() & OnlyPositiveNumbersMask;
            int location = hashCode % entries.Length;
            throw new NotImplementedException();
        }

        public ResultItem DeleteProbe(string key)
        {
            int hashCode = key.GetHashCode() & OnlyPositiveNumbersMask;
            int location = hashCode % entries.Length;
            throw new NotImplementedException();
        }
    }

    internal struct OA_SimpleEntry
    {
        public int hashCode;  // Lower 31 bits of hash code, -1 if unused, -2 if deleted
        public string key;    // Key of entry
        public string value;  // Value of entry
    }

}



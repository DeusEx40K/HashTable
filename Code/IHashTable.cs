namespace HashTable
{
    public interface IHashTable
    {
        string Name { get; }
        int Count { get; }
        int Capacity { get; }
        int EntrySize { get; }

        ResultType Insert(string key, string value);
        ResultType Search(string key);
        ResultType Delete(string key);
        void Clear();

        ResultItem InsertProbe(string key, string value);
        ResultItem SearchProbe(string key);
        ResultItem DeleteProbe(string key);
        void Print();
    }
}

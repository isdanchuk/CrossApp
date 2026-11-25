using Core;
using Core.Abstractions;
using Core.Storage;

namespace Cli;

public static class StoreFactory
{
    public static IBookStore Create(string[] args)
    {
        bool useFile = args.Contains("--file");
        bool useCache = args.Contains("--cache");

        string dataPath = Path.Combine(AppContext.BaseDirectory, "data", "library.json");

        IBookStore store = useFile
            ? new FileBookStore(dataPath)
            : new InMemoryBookStore(SampleData.Books());

        return useCache ? new CachingBookStore(store) : store;
    }
}
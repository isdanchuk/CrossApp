using Core.Abstractions;
using Core.Domain;

namespace Core.Storage;

public sealed class CachingBookStore(IBookStore inner) : IBookStore
{
    private readonly IBookStore _inner = inner ?? throw new ArgumentNullException(nameof(inner));
    private readonly Dictionary<string, BookCopy> _byIdCache = new(StringComparer.OrdinalIgnoreCase);

    public IReadOnlyList<BookCopy> List() => _inner.List();

    public BookCopy? GetById(string id)
    {
        if (_byIdCache.TryGetValue(id, out var cached))
            return cached;

        var copy = _inner.GetById(id);
        if (copy is not null)
            _byIdCache[id] = copy;

        return copy;
    }

    public IReadOnlyList<BookCopy> Find(Func<BookCopy, bool> predicate) => _inner.Find(predicate);

    public void Add(BookCopy item)
    {
        _inner.Add(item);
        _byIdCache[item.Id] = item;
    }

    public void Update(BookCopy item)
    {
        _inner.Update(item);
        _byIdCache[item.Id] = item;
    }

    public bool Remove(string id)
    {
        var removed = _inner.Remove(id);
        if (removed) _byIdCache.Remove(id);
        return removed;
    }
}
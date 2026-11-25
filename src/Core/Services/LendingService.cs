using Core.Abstractions;
using Core.Domain;

namespace Core.Services;

public sealed class LendingService(IBookStore store)
{
    private readonly IBookStore _store = store ?? throw new ArgumentNullException(nameof(store));

    public BookCopy AddBook(string isbn, string title)
    {
        var id = "C-" + Guid.NewGuid().ToString("N")[..6].ToUpperInvariant();
        var copy = BookCopy.Create(id, isbn, title);
        _store.Add(copy);
        return copy;
    }

    public void IssueCopy(string id)
    {
        var copy = _store.GetById(id)
            ?? throw new InvalidOperationException($"Примірник з id={id} не знайдено.");
        copy.Issue();
        _store.Update(copy);
    }

    public void ReturnCopy(string id)
    {
        var copy = _store.GetById(id)
            ?? throw new InvalidOperationException($"Примірник з id={id} не знайдено.");
        copy.Return();
        _store.Update(copy);
    }

    public IReadOnlyList<BookCopy> AllBooks() => _store.List();

    public BookCopy? FindById(string id) => _store.GetById(id);

    public IReadOnlyList<BookCopy> FindAvailable() =>
        _store.Find(b => !b.IsIssued);
}
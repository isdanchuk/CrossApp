using Core.Dto;

namespace Core.Domain;

public sealed class BookCopy
{
    private bool _isIssued;

    public string Id { get; }
    public string Isbn { get; }
    public string Title { get; }
    public bool IsIssued => _isIssued;

    private BookCopy(string id, string isbn, string title, bool isIssued)
    {
        Id = id;
        Isbn = isbn;
        Title = title;
        _isIssued = isIssued;
    }

    public static BookCopy Create(string id, string isbn, string title, bool isIssued = false)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Ідентифікатор примірника обов'язковий", nameof(id));
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN не може бути порожнім", nameof(isbn));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Назва книги не може бути порожньою", nameof(title));

        return new BookCopy(id.Trim(), isbn.Trim().ToUpperInvariant(), title.Trim(), isIssued);
    }

    public void Issue()
    {
        if (_isIssued)
            throw new InvalidOperationException($"Примірник {Id} (ISBN: {Isbn}) вже виданий, повторна видача неможлива.");
        _isIssued = true;
    }

    public void Return()
    {
        if (!_isIssued)
            throw new InvalidOperationException($"Примірник {Id} не перебуває в оренді, повернення неможливе.");
        _isIssued = false;
    }

    public BookDto ToDto() => new(Id, Isbn, Title, IsIssued: _isIssued);

    public static BookCopy FromDto(BookDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return Create(dto.Id, dto.Isbn, dto.Title, dto.IsIssued);
    }

    public override string ToString() =>
        $"Примірник {Id} [{Isbn}] «{Title}» — Статус: {(_isIssued ? "Виданий" : "У бібліотеці")}";
}
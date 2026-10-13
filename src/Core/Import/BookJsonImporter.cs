using System.Text.Json;
using Core.Dto;

namespace Core.Import;

public static class BookJsonImporter
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static ImportResult<BookDto> Load(string path)
    {
        var errors = new List<string>();

        try
        {
            string json = File.ReadAllText(path);
            List<BookDto> rawItems = JsonSerializer.Deserialize<List<BookDto>>(json, JsonOptions) ?? [];

            var items = new List<BookDto>();

            for (int i = 0; i < rawItems.Count; i++)
            {
                var book = rawItems[i];
                int entryNumber = i + 1;

                if (string.IsNullOrWhiteSpace(book.Id) || string.IsNullOrWhiteSpace(book.Isbn) || string.IsNullOrWhiteSpace(book.Title))
                {
                    errors.Add($"об'єкт {entryNumber}: обов'язкові поля (Id, Isbn, Title) не можуть бути порожніми");
                    continue;
                }

                if (book.Year < 1450 || book.Year > DateTime.Now.Year)
                {
                    errors.Add($"об'єкт {entryNumber}: рік '{book.Year}' не є коректним (допустимо 1450..{DateTime.Now.Year})");
                    continue;
                }

                items.Add(book);
            }

            return new ImportResult<BookDto>(items, errors);
        }
        catch (JsonException ex)
        {
            errors.Add($"помилка структури JSON: {ex.Message}");
            return new ImportResult<BookDto>([], errors);
        }
    }
}
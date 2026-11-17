using Core.Dto;
using Core.Domain;

namespace Core.Import;

public static class BookDomainImporter
{
    public static (IReadOnlyList<BookCopy> Books, IReadOnlyList<string> Errors) ImportToDomain(ImportResult<BookDto> importResult)
    {
        var books = new List<BookCopy>();
        var errors = new List<string>(importResult.Errors); // Зберігаємо помилки самого парсингу з тижня 3

        foreach (var dto in importResult.Items)
        {
            try
            {
                // Створення через фабричний метод перевіряє всі інваріанти
                var book = BookCopy.Create(dto.Id, dto.Isbn, dto.Title);
                books.Add(book);
            }
            catch (Exception ex)
            {
                // Якщо інваріант порушено, фіксуємо помилку для конкретного запису
                errors.Add($"Рядок з ID '{dto.Id}' не пройшов доменну валідацію: {ex.Message}");
            }
        }

        return (books, errors);
    }
}
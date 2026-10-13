using Core.Dto;

namespace Core.Import;

public static class MixedCsvImporter
{
    private const char Separator = ';';

    public static MixedImportResult Load(string path)
    {
        var books = new List<BookDto>();
        var readers = new List<ReaderDto>();
        var errors = new List<string>();

        string[] lines = File.ReadAllLines(path);

        for (int i = 0; i < lines.Length; i++)
        {
            int number = i + 1;
            string line = lines[i];

            if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
                continue;

            switch (ParseLine(line))
            {
                case MixedOutcome.BookOk b:
                    books.Add(b.Value);
                    break;
                case MixedOutcome.ReaderOk r:
                    readers.Add(r.Value);
                    break;
                case MixedOutcome.Failed f:
                    errors.Add($"рядок {number}: {f.Reason}");
                    break;
            }
        }

        return new MixedImportResult(books, readers, errors);
    }

    private static MixedOutcome ParseLine(string line)
    {
        string[] parts = line.Split(Separator, StringSplitOptions.TrimEntries);

        return parts switch
        {
            // Обробка книги: B;id;isbn;title;year;[author]
            ["B", _, "", ..] or ["B", _, _, "", ..] =>
                new MixedOutcome.Failed("ISBN або назва книги порожні"),

            ["B", _, _, _, var year, ..] when !int.TryParse(year, out int y) || y < 1450 || y > DateTime.Now.Year =>
                new MixedOutcome.Failed($"рік книги '{year}' поза допустимими межами"),

            ["B", var id, var isbn, var title, var year] =>
                new MixedOutcome.BookOk(new BookDto(id, isbn, title, int.Parse(year))),

            ["B", var id, var isbn, var title, var year, var author] =>
                new MixedOutcome.BookOk(new BookDto(id, isbn, title, int.Parse(year), string.IsNullOrWhiteSpace(author) ? null : author)),

            // Обробка читача: R;id;fullName;email;[phone]
            ["R", _, "", ..] or ["R", _, _, "", ..] =>
                new MixedOutcome.Failed("ПІБ або Email читача порожні"),

            ["R", var id, var name, var email] =>
                new MixedOutcome.ReaderOk(new ReaderDto(id, name, email)),

            ["R", var id, var name, var email, var phone] =>
                new MixedOutcome.ReaderOk(new ReaderDto(id, name, email, string.IsNullOrWhiteSpace(phone) ? null : phone)),

            // Невідомий префікс або замало даних
            [var prefix, ..] when prefix is not "B" and not "R" =>
                new MixedOutcome.Failed($"невідомий тип запису '{prefix}' (очікую 'B' або 'R')"),

            _ =>
                new MixedOutcome.Failed($"некоректний формат колонок: {parts.Length}")
        };
    }

    private abstract record MixedOutcome
    {
        public sealed record BookOk(BookDto Value) : MixedOutcome;
        public sealed record ReaderOk(ReaderDto Value) : MixedOutcome;
        public sealed record Failed(string Reason) : MixedOutcome;
    }
}
using Core.Dto;
using Core.Import;

string path = args.Length > 0 ? args[0] : Path.Combine("data", "sample.csv");

if (!File.Exists(path))
{
    Console.WriteLine($"Файл не знайдено: {Path.GetFullPath(path)}");
    return 1;
}

string extension = Path.GetExtension(path).ToLowerInvariant();
string fileName = Path.GetFileName(path).ToLowerInvariant();

// Додаткове завдання 2: якщо файл mixed.csv — запускаємо змішаний імпортер
if (fileName.Contains("mixed"))
{
    MixedImportResult mixed = MixedCsvImporter.Load(path);
    int totalMixed = mixed.Books.Count + mixed.Readers.Count + mixed.Errors.Count;
    int successMixed = mixed.Books.Count + mixed.Readers.Count;
    double errorRateMixed = totalMixed > 0 ? (double)mixed.Errors.Count / totalMixed * 100 : 0;

    Console.WriteLine($"=== Змішаний імпорт ({fileName}) ===");
    Console.WriteLine($"Книг: {mixed.Books.Count}, Читачів: {mixed.Readers.Count}");
    
    foreach (var b in mixed.Books.Take(3))
        Console.WriteLine($" [Книга]  {b.Id,-6} {b.Title,-25} {b.Year}");

    foreach (var r in mixed.Readers.Take(3))
        Console.WriteLine($" [Читач]  {r.Id,-6} {r.FullName,-25} {r.Email}");

    if (mixed.Errors.Count > 0)
    {
        Console.WriteLine($"Пропущено рядків: {mixed.Errors.Count}");
        foreach (string e in mixed.Errors)
            Console.WriteLine($" ! {e}");
    }

    // Додаткове завдання 3: статистика одним рядком
    Console.WriteLine(new string('-', 65));
    Console.WriteLine($"СТАТИСТИКА: Усього: {totalMixed} | Прийнято: {successMixed} | Пропущено: {mixed.Errors.Count} | Помилок: {errorRateMixed:F1}%");
    return 0;
}

// Додаткове завдання 1: вибір імпортера за розширенням файлу через switch expression
ImportResult<BookDto> result = extension switch
{
    ".csv" => BookCsvImporter.Load(path),
    ".json" => BookJsonImporter.Load(path),
    _ => throw new NotSupportedException($"Розширення '{extension}' не підтримується. Використовуйте .csv або .json")
};

Console.WriteLine($"Завантажено записів: {result.Items.Count}");
foreach (BookDto b in result.Items.Take(5))
{
    Console.WriteLine($" {b.Id,-6} {b.Isbn,-16} {b.Title,-30} {b.Year,4}  {(b.Author ?? "-")}");
}

if (result.Errors.Count > 0)
{
    Console.WriteLine($"Пропущено рядків: {result.Errors.Count}");
    foreach (string e in result.Errors)
        Console.WriteLine($" ! {e}");
}

// Додаткове завдання 3: Статистика одним рядком (усього / прийнято / пропущено / % помилок)
int total = result.Items.Count + result.Errors.Count;
double errorRate = total > 0 ? (double)result.Errors.Count / total * 100 : 0;

Console.WriteLine(new string('-', 65));
Console.WriteLine($"СТАТИСТИКА: Усього: {total} | Прийнято: {result.Items.Count} | Пропущено: {result.Errors.Count} | Помилок: {errorRate:F1}%");

return 0;
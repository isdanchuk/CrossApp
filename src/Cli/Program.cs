using Core.Domain;

Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА 4: Доменна модель ===");
Console.WriteLine("Предметна область: Бібліотека\n");

// === Сценарій 1: Успішна робота з доменною моделлю ===
Console.WriteLine("--- Сценарій 1: Успіх ---");
try
{
    BookCopy book = BookCopy.Create("C-101", "978-5-17-08", "C# для початківців");
    Console.WriteLine(book);

    book.Issue();
    Console.WriteLine("Після видачі: " + book);

    book.Return();
    Console.WriteLine("Після повернення: " + book);
}
catch (Exception ex)
{
    Console.WriteLine($"Помилка в успішному сценарії: {ex.Message}");
}

Console.WriteLine();

// === Сценарій 2: Порушення інваріантів ===
Console.WriteLine("--- Сценарій 2: Порушення інваріантів ---");

TryDo("Повторна видача вже виданого примірника", () => {
    var b = BookCopy.Create("C-102", "978-1-49-19", "Clean Code", isIssued: true);
    b.Issue(); // Має кинути InvalidOperationException
});

TryDo("Створення примірника з порожнім ISBN", () => {
    BookCopy.Create("C-103", "   ", "Архітектура ПЗ"); // Має кинути ArgumentException
});

TryDo("Спроба повернути книгу, яка не була видана", () => {
    var b = BookCopy.Create("C-104", "978-0-13-23", "Refactoring");
    b.Return(); // Має кинути InvalidOperationException
});


// === Додаткове завдання 2: Сервіс та перевірка кількох сутностей ===
Console.WriteLine("\n--- Додаткове завдання 2: Сервіс та перевірка кількох сутностей ---");
var book1 = BookCopy.Create("C-201", "978-0-1", "Книга 1");
var existingLoans = new List<Loan>
{
    Loan.Open("L-1", "C-001", "Reader-99", DateTime.Now),
    Loan.Open("L-2", "C-002", "Reader-99", DateTime.Now),
    Loan.Open("L-3", "C-003", "Reader-99", DateTime.Now),
    Loan.Open("L-4", "C-004", "Reader-99", DateTime.Now),
    Loan.Open("L-5", "C-005", "Reader-99", DateTime.Now) // Вже 5 активних видач!
};

TryDo("Спроба видати 6-ту книгу тому ж читачу (перевищення ліміту)", () => {
    LibraryLoanService.CreateLoanSafely("L-6", book1, "Reader-99", existingLoans);
});


// === Додаткове завдання 3: Enum станів та switch переходи ===
Console.WriteLine("\n--- Додаткове завдання 3: Enum станів та switch переходи ---");
var reservation = BookReservation.Create("RES-001");
Console.WriteLine(reservation);

reservation.ChangeStatus(ReservationStatus.Active);
Console.WriteLine($"Після зміни стану: {reservation}");

TryDo("Заборонений перехід стану (з Active одразу в Draft)", () => {
    reservation.ChangeStatus(ReservationStatus.Draft);
});


// Допоміжна локальна функція для тестування винятків
static void TryDo(string title, Action action)
{
    try
    {
        action();
        Console.WriteLine($"[ПОВАЛЕННЯ ТЕСТУ] {title}: виняток НЕ спрацював, інваріант відсутній!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[УСПІХ ПЕРЕВІРКИ] {title} -> {ex.GetType().Name}: {ex.Message}");
    }
}
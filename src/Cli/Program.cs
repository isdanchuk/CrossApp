using Cli;
using Core.Abstractions;
using Core.Services;

IBookStore store = StoreFactory.Create(args);
var service = new LendingService(store);

Console.WriteLine($"=== Бібліотека (Сховище: {store.GetType().Name}) ===");

// 1. Додавання нового примірника
var created = service.AddBook("978-617-12-9876-5", "Кобзар");
Console.WriteLine($"\n[+] Додано: {created}");

// 2. Видача та повернення примірника
service.IssueCopy(created.Id);
Console.WriteLine($"[->] Після видачі: {service.FindById(created.Id)}");

service.ReturnCopy(created.Id);
Console.WriteLine($"[<-] Після повернення: {service.FindById(created.Id)}");

// 3. Пошук за предикатом (Додаткове завдання 2 — лише доступні у бібліотеці)
Console.WriteLine($"\n[?] Доступних примірників у бібліотеці: {service.FindAvailable().Count}");

// 4. Вивід усіх примірників у сховищі
Console.WriteLine($"\nУсього примірників у каталозі: {service.AllBooks().Count}");
foreach (var b in service.AllBooks())
    Console.WriteLine($"  {b.Id,-10} {b.Isbn,-16} {b.Title,-30} {(b.IsIssued ? "Виданий" : "Вільний")}");

// 5. Сценарій відмови (перехоплення винятку)
Console.WriteLine("\n[!] Перевірка сценарію відмови (видача неіснуючого примірника):");
try
{
    service.IssueCopy("C-999999");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"    Очікувана помилка: {ex.Message}");
}
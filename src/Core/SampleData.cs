using Core.Domain;

namespace Core;

public static class SampleData
{
    public static IEnumerable<BookCopy> Books() =>
    [
        BookCopy.Create("C-001", "978-0132350884", "Clean Code"),
        BookCopy.Create("C-002", "978-0132350884", "Clean Code (прим. 2)"),
        BookCopy.Create("C-003", "978-0201633610", "Design Patterns"),
        BookCopy.Create("C-004", "978-0131103627", "The C Programming Language"),
        BookCopy.Create("C-005", "978-0321125217", "Domain-Driven Design"),
        BookCopy.Create("C-006", "978-0596007126", "Head First Design Patterns"),
        BookCopy.Create("C-007", "978-0134757599", "Refactoring"),
        BookCopy.Create("C-008", "978-0135957059", "The Pragmatic Programmer"),
        BookCopy.Create("C-009", "978-1449373320", "Designing Data-Intensive Apps"),
        BookCopy.Create("C-010", "978-0321356680", "Effective Java"),
        BookCopy.Create("C-011", "978-1617294532", "C# in Depth"),
        BookCopy.Create("C-012", "978-0134494166", "Clean Architecture"),
        BookCopy.Create("C-013", "978-0262033848", "Introduction to Algorithms"),
        BookCopy.Create("C-014", "978-1491950357", "Building Microservices"),
        BookCopy.Create("C-015", "978-0137081073", "The Clean Coder")
    ];
}
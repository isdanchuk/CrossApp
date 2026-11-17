namespace Core.Domain;

public static class LibraryLoanService
{
    private const int MaxActiveLoansPerReader = 5;

    // Метод перевіряє бізнес-правило, яке охоплює і сутність примірника, і список активних видач читача
    public static Loan CreateLoanSafely(
        string loanId, 
        BookCopy bookCopy, 
        string readerId, 
        IEnumerable<Loan> existingLoans)
    {
        // 1. Інваріант примірника: чи можна взагалі видати цю книгу?
        if (bookCopy.IsIssued)
        {
            throw new InvalidOperationException($"Примірник {bookCopy.Id} вже виданий і не може бути виданий повторно.");
        }

        // 2. Міжсутносний інваріант: рахуємо активні (не закриті) видачі цього конкретного читача
        int activeLoansCount = existingLoans
            .Count(l => l.ReaderId.Equals(readerId, StringComparison.OrdinalIgnoreCase) && !l.IsClosed);

        if (activeLoansCount >= MaxActiveLoansPerReader)
        {
            throw new InvalidOperationException(
                $"Читач {readerId} вже має {activeLoansCount} активних видач (ліміт: {MaxActiveLoansPerReader}). Нова видача неможлива.");
        }

        // Якщо все добре, відкриваємо новий запис видачі
        return Loan.Open(loanId, bookCopy.Id, readerId, DateTime.Now);
    }
}
namespace Core.Domain;

public sealed class Loan
{
    public string LoanId { get; }
    public string CopyId { get; }
    public string ReaderId { get; }
    public DateTime IssuedAt { get; }
    public bool IsClosed { get; private set; }

    private Loan(string loanId, string copyId, string readerId, DateTime issuedAt, bool isClosed)
    {
        LoanId = loanId;
        CopyId = copyId;
        ReaderId = readerId;
        IssuedAt = issuedAt;
        IsClosed = isClosed;
    }

    public static Loan Open(string loanId, string copyId, string readerId, DateTime issuedAt)
    {
        if (string.IsNullOrWhiteSpace(loanId))
            throw new ArgumentException("ID видачі не може бути порожнім", nameof(loanId));
        if (string.IsNullOrWhiteSpace(copyId))
            throw new ArgumentException("ID примірника не може бути порожнім", nameof(copyId));
        if (string.IsNullOrWhiteSpace(readerId))
            throw new ArgumentException("ID читача не може бути порожнім", nameof(readerId));
        if (issuedAt > DateTime.Now.AddMinutes(1))
            throw new ArgumentOutOfRangeException(nameof(issuedAt), issuedAt, "Дата видачі не може бути в майбутньому.");

        return new Loan(loanId.Trim(), copyId.Trim(), readerId.Trim(), issuedAt, false);
    }

    public void Close()
    {
        if (IsClosed)
            throw new InvalidOperationException($"Цей запис видачі ({LoanId}) вже закритий.");
        
        IsClosed = true;
    }

public override string ToString() => 
        $"Видача {LoanId} (Примірник: {CopyId}, Читач: {ReaderId}) — {(IsClosed ? "Закрито" : "Активна")}";
}
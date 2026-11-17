namespace Core.Domain;

public enum ReservationStatus
{
    Draft,     // Чернетка (створено, але не підтверджено)
    Active,    // Активна (видано читачу)
    Closed,    // Успішно повернуто
    Cancelled  // Скасовано
}

public sealed class BookReservation
{
    public string Id { get; }
    public ReservationStatus Status { get; private set; }

    private BookReservation(string id, ReservationStatus status)
    {
        Id = id;
        Status = status;
    }

    public static BookReservation Create(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("ID резервації обов'язковий", nameof(id));

        return new BookReservation(id.Trim(), ReservationStatus.Draft);
    }

    // Зміна стану за допомогою суворого switch expression (перевірка дозволених переходів)
    public void ChangeStatus(ReservationStatus targetStatus)
    {
        // Вираз switch перевіряє поточний стан і цільовий стан
        bool isValidTransition = (Status, targetStatus) switch
        {
            (ReservationStatus.Draft, ReservationStatus.Active) => true,     // З чернетки в активні — можна
            (ReservationStatus.Draft, ReservationStatus.Cancelled) => true,  // З чернетки в скасовані — можна
            (ReservationStatus.Active, ReservationStatus.Closed) => true,    // З активних у закриті (повернуто) — можна
            (ReservationStatus.Active, ReservationStatus.Cancelled) => true, // З активних у скасовані — можна
            _ => false // Всі інші переходи суворо заборонені!
        };

        if (!isValidTransition)
        {
            throw new InvalidOperationException(
                $"Недопустимий перехід стану резервації {Id}: з '{Status}' у '{targetStatus}'.");
        }

        Status = targetStatus;
    }

    public override string ToString() => $"Резервація {Id} — Статус: {Status}";
}
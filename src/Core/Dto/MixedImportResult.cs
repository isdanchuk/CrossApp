namespace Core.Dto;

public sealed record MixedImportResult(
    IReadOnlyList<BookDto> Books,
    IReadOnlyList<ReaderDto> Readers,
    IReadOnlyList<string> Errors);
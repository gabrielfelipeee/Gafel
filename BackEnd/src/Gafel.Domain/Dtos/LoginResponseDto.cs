namespace Gafel.Domain.Dtos;

public record LoginResponseDto(
    bool Success,
    long? UserId = null,
    string? ErrorMessage = null
);

namespace Gafel.Domain.Dtos.Responses;

public record LoginResponseDto(
    bool Success,
    long? UserId = null,
    string? ErrorMessage = null
);

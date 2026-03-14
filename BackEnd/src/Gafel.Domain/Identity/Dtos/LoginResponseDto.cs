namespace Gafel.Domain.Identity.Dtos;

public record LoginResponseDto(
    bool Success,
    long? UserId = null,
    string? ErrorMessage = null
);

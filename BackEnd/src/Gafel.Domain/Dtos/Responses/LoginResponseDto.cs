namespace Gafel.Domain.Dtos.Responses;

public record LoginResponseDto(
    bool Success,
    Guid? UserId = null,
    string? ErrorMessage = null
);

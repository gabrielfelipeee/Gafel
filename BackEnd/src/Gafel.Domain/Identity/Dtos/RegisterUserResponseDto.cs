namespace Gafel.Domain.Identity.Dtos;

public record RegisterUserResponseDto(
    bool Success,
    long? UserId = null,
    Dictionary<string, string[]>? Errors = null
);

namespace Gafel.Domain.Dtos.Responses;

public record RegisterUserResponseDto(
    bool Success,
    long? UserId = null,
    Dictionary<string, string[]>? Errors = null
);

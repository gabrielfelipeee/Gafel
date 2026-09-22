namespace Gafel.Domain.Dtos.Responses;

public record RegisterUserResponseDto(
    bool Success,
    Guid? UserId = null,
    Dictionary<string, string[]>? Errors = null
);

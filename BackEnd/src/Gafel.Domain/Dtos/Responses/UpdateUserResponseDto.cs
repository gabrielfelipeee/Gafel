namespace Gafel.Domain.Dtos.Responses;

public record UpdateUserResponseDto(
    bool Success,
    Dictionary<string, string[]>? Errors = null
);

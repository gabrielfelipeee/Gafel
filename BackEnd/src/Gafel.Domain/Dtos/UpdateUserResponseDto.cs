namespace Gafel.Domain.Dtos;

public record UpdateUserResponseDto(
    bool Success,
    Dictionary<string, string[]>? Errors = null
);

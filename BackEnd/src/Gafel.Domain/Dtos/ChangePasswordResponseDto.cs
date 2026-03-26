namespace Gafel.Domain.Dtos;

public record ChangePasswordResponseDto(
    bool Success,
    Dictionary<string, string[]>? Errors = null
);

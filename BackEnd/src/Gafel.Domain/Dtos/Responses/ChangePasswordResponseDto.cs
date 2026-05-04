namespace Gafel.Domain.Dtos.Responses;

public record ChangePasswordResponseDto(
    bool Success,
    Dictionary<string, string[]>? Errors = null
);

namespace Gafel.Domain.Identity.Dtos;

public record LoginResult(bool Success, long? UserId = null, string? ErrorMessage = null)
{
    public bool Success { get; init; } = Success;
    public long? UserId { get; init; } = UserId;
    public string? ErrorMessage { get; init; } = ErrorMessage;
}

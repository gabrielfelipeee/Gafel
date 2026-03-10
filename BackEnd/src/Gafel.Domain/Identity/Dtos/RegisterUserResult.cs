namespace Gafel.Domain.Identity.Dtos;

public record RegisterUserResult(bool Success, long? UserId = null, Dictionary<string, string[]>? Errors = null)
{
    public bool Success { get; init; } = Success;
    public long? UserId { get; init; } = UserId;
    public Dictionary<string, string[]>? Errors { get; init; } = Errors;
}

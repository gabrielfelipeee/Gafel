namespace Gafel.Domain.Dtos;

public record UserDto(
    Guid Id,
    string Email,
    string UserName
);

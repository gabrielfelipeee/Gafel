namespace Gafel.Domain.Constants;

public static class Roles
{
    public const string Admin = nameof(Admin);
    public const string User = nameof(User);

    public static readonly HashSet<string> All =
    [
        Admin,
        User
    ];
}

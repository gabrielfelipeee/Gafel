namespace Gafel.Infrastructure.Migrations
{
    public abstract class DatabaseTables
    {
        public const string USERS = "users";
        public const string ROLES = "roles";
        public const string USER_ROLES = "user_roles";
        public const string USER_CLAIMS = "user_claims";
        public const string USER_LOGINS = "user_logins";
        public const string USER_TOKENS = "user_tokens";
        public const string ROLE_CLAIMS = "role_claims";

        public const string PEOPLE = "people";
        public const string DEFAULT_CATEGORIES = "default_categories";
    }
}

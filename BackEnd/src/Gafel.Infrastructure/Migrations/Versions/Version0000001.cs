using FluentMigrator;

namespace Gafel.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.CREATE_TABLES_IDENTITY, "Criar tabelas do Identity")]
public class Version0000001 : ForwardOnlyMigration
{
    private const string USER_ID = "user_id";
    private const string ROLE_ID = "role_id";
    private const string LOGIN_PROVIDER = "login_provider";


    public override void Up()
    {
        CreateUsers();
        CreateRoles();
        CreateRoleClaims();
        CreateUserClaims();
        CreateUserLogins();
        CreateUserRoles();
        CreateUserTokens();
    }

    private void CreateUsers()
    {
        Create.Table(DatabaseTables.USERS)
            .WithColumn("id").AsInt64().PrimaryKey().Identity()

            .WithColumn("user_name").AsString(60).NotNullable()
            .WithColumn("normalized_user_name").AsString(60).NotNullable()

            .WithColumn("email").AsString(60).NotNullable()
            .WithColumn("normalized_email").AsString(60).NotNullable()

            .WithColumn("email_confirmed").AsBoolean().NotNullable().WithDefaultValue(false)

            .WithColumn("password_hash").AsString(2000).NotNullable()
            .WithColumn("security_stamp").AsString(2000).Nullable()
            .WithColumn("concurrency_stamp").AsString(2000).Nullable()

            .WithColumn("phone_number").AsString(20).Nullable()
            .WithColumn("phone_number_confirmed").AsBoolean().NotNullable().WithDefaultValue(false)

            .WithColumn("two_factor_enabled").AsBoolean().NotNullable().WithDefaultValue(false)

            .WithColumn("lockout_end").AsDateTime().Nullable()
            .WithColumn("lockout_enabled").AsBoolean().NotNullable().WithDefaultValue(false)

            .WithColumn("access_failed_count").AsInt32().NotNullable().WithDefaultValue(0);

        Create.Index("ix_users_normalized_user_name")
            .OnTable(DatabaseTables.USERS)
            .OnColumn("normalized_user_name")
            .Unique();

        Create.Index("ix_users_normalized_email")
            .OnTable(DatabaseTables.USERS)
            .OnColumn("normalized_email");
    }

    private void CreateRoles()
    {
        Create.Table(DatabaseTables.ROLES)
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("name").AsString(50).NotNullable()
            .WithColumn("normalized_name").AsString(50).NotNullable()
            .WithColumn("concurrency_stamp").AsString(2000).Nullable();

        Create.Index("ix_roles_normalized_name")
            .OnTable(DatabaseTables.ROLES)
            .OnColumn("normalized_name")
            .Unique();
    }

    private void CreateRoleClaims()
    {
        Create.Table(DatabaseTables.ROLE_CLAIMS)
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn(ROLE_ID).AsInt64().NotNullable()
            .WithColumn("claim_type").AsString(256).Nullable()
            .WithColumn("claim_value").AsString(256).Nullable();

        Create.ForeignKey("fk_role_claims_roles")
            .FromTable(DatabaseTables.ROLE_CLAIMS).ForeignColumn(ROLE_ID)
            .ToTable(DatabaseTables.ROLES).PrimaryColumn("id");

        Create.Index("ix_role_claims_role_id")
            .OnTable(DatabaseTables.ROLE_CLAIMS)
            .OnColumn(ROLE_ID);
    }

    private void CreateUserClaims()
    {
        Create.Table(DatabaseTables.USER_CLAIMS)
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn(USER_ID).AsInt64().NotNullable()
            .WithColumn("claim_type").AsString(256).Nullable()
            .WithColumn("claim_value").AsString(256).Nullable();

        Create.ForeignKey("fk_user_claims_users")
            .FromTable(DatabaseTables.USER_CLAIMS).ForeignColumn(USER_ID)
            .ToTable(DatabaseTables.USERS).PrimaryColumn("id");

        Create.Index("ix_user_claims_user_id")
            .OnTable(DatabaseTables.USER_CLAIMS)
            .OnColumn(USER_ID);
    }

    private void CreateUserLogins()
    {
        Create.Table(DatabaseTables.USER_LOGINS)
            .WithColumn(LOGIN_PROVIDER).AsString(128).NotNullable()
            .WithColumn("provider_key").AsString(128).NotNullable()
            .WithColumn("provider_display_name").AsString(256).Nullable()
            .WithColumn(USER_ID).AsInt64().NotNullable();

        Create.PrimaryKey("pk_user_logins")
            .OnTable(DatabaseTables.USER_LOGINS)
            .Columns(LOGIN_PROVIDER, "provider_key");

        Create.ForeignKey("fk_user_logins_users")
            .FromTable(DatabaseTables.USER_LOGINS).ForeignColumn(USER_ID)
            .ToTable(DatabaseTables.USERS).PrimaryColumn("id");

        Create.Index("ix_user_logins_user_id")
            .OnTable(DatabaseTables.USER_LOGINS)
            .OnColumn(USER_ID);
    }

    private void CreateUserRoles()
    {
        Create.Table(DatabaseTables.USER_ROLES)
            .WithColumn(USER_ID).AsInt64().NotNullable()
            .WithColumn(ROLE_ID).AsInt64().NotNullable();

        Create.PrimaryKey("pk_user_roles")
            .OnTable(DatabaseTables.USER_ROLES)
            .Columns(USER_ID, ROLE_ID);

        Create.ForeignKey("fk_user_roles_users")
            .FromTable(DatabaseTables.USER_ROLES).ForeignColumn(USER_ID)
            .ToTable(DatabaseTables.USERS).PrimaryColumn("id");

        Create.ForeignKey("fk_user_roles_roles")
            .FromTable(DatabaseTables.USER_ROLES).ForeignColumn(ROLE_ID)
            .ToTable(DatabaseTables.ROLES).PrimaryColumn("id");

        Create.Index("ix_user_roles_role_id")
            .OnTable(DatabaseTables.USER_ROLES)
            .OnColumn(ROLE_ID);
    }

    private void CreateUserTokens()
    {
        Create.Table(DatabaseTables.USER_TOKENS)
            .WithColumn(USER_ID).AsInt64().NotNullable()
            .WithColumn(LOGIN_PROVIDER).AsString(128).NotNullable()
            .WithColumn("name").AsString(128).NotNullable()
            .WithColumn("value").AsString(256).Nullable();

        Create.PrimaryKey("pk_user_tokens")
            .OnTable(DatabaseTables.USER_TOKENS)
            .Columns(USER_ID, LOGIN_PROVIDER, "name");

        Create.ForeignKey("fk_user_tokens_users")
            .FromTable(DatabaseTables.USER_TOKENS).ForeignColumn(USER_ID)
            .ToTable(DatabaseTables.USERS).PrimaryColumn("id");
    }
}
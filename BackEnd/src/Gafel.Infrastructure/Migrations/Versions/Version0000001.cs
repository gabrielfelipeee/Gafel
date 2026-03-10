using FluentMigrator;

namespace Gafel.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.CREATE_TABLES_IDENTITY, "Criar tabelas do Identity")]
public class Version0000001 : ForwardOnlyMigration
{
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
        Create.Table("users")
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
            .OnTable("users")
            .OnColumn("normalized_user_name")
            .Unique();

        Create.Index("ix_users_normalized_email")
            .OnTable("users")
            .OnColumn("normalized_email");
    }

    private void CreateRoles()
    {
        Create.Table("roles")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("name").AsString(50).NotNullable()
            .WithColumn("normalized_name").AsString(50).NotNullable()
            .WithColumn("concurrency_stamp").AsString(2000).Nullable();

        Create.Index("ix_roles_normalized_name")
            .OnTable("roles")
            .OnColumn("normalized_name")
            .Unique();
    }

    private void CreateRoleClaims()
    {
        Create.Table("role_claims")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("role_id").AsInt64().NotNullable()
            .WithColumn("claim_type").AsString(256).Nullable()
            .WithColumn("claim_value").AsString(256).Nullable();

        Create.ForeignKey("fk_role_claims_roles")
            .FromTable("role_claims").ForeignColumn("role_id")
            .ToTable("roles").PrimaryColumn("id");

        Create.Index("ix_role_claims_role_id")
            .OnTable("role_claims")
            .OnColumn("role_id");
    }

    private void CreateUserClaims()
    {
        Create.Table("user_claims")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("user_id").AsInt64().NotNullable()
            .WithColumn("claim_type").AsString(256).Nullable()
            .WithColumn("claim_value").AsString(256).Nullable();

        Create.ForeignKey("fk_user_claims_users")
            .FromTable("user_claims").ForeignColumn("user_id")
            .ToTable("users").PrimaryColumn("id");

        Create.Index("ix_user_claims_user_id")
            .OnTable("user_claims")
            .OnColumn("user_id");
    }

    private void CreateUserLogins()
    {
        Create.Table("user_logins")
            .WithColumn("login_provider").AsString(128).NotNullable()
            .WithColumn("provider_key").AsString(128).NotNullable()
            .WithColumn("provider_display_name").AsString(256).Nullable()
            .WithColumn("user_id").AsInt64().NotNullable();

        Create.PrimaryKey("pk_user_logins")
            .OnTable("user_logins")
            .Columns("login_provider", "provider_key");

        Create.ForeignKey("fk_user_logins_users")
            .FromTable("user_logins").ForeignColumn("user_id")
            .ToTable("users").PrimaryColumn("id");

        Create.Index("ix_user_logins_user_id")
            .OnTable("user_logins")
            .OnColumn("user_id");
    }

    private void CreateUserRoles()
    {
        Create.Table("user_roles")
            .WithColumn("user_id").AsInt64().NotNullable()
            .WithColumn("role_id").AsInt64().NotNullable();

        Create.PrimaryKey("pk_user_roles")
            .OnTable("user_roles")
            .Columns("user_id", "role_id");

        Create.ForeignKey("fk_user_roles_users")
            .FromTable("user_roles").ForeignColumn("user_id")
            .ToTable("users").PrimaryColumn("id");

        Create.ForeignKey("fk_user_roles_roles")
            .FromTable("user_roles").ForeignColumn("role_id")
            .ToTable("roles").PrimaryColumn("id");

        Create.Index("ix_user_roles_role_id")
            .OnTable("user_roles")
            .OnColumn("role_id");
    }

    private void CreateUserTokens()
    {
        Create.Table("user_tokens")
            .WithColumn("user_id").AsInt64().NotNullable()
            .WithColumn("login_provider").AsString(128).NotNullable()
            .WithColumn("name").AsString(128).NotNullable()
            .WithColumn("value").AsString(256).Nullable();

        Create.PrimaryKey("pk_user_tokens")
            .OnTable("user_tokens")
            .Columns("user_id", "login_provider", "name");

        Create.ForeignKey("fk_user_tokens_users")
            .FromTable("user_tokens").ForeignColumn("user_id")
            .ToTable("users").PrimaryColumn("id");
    }
}
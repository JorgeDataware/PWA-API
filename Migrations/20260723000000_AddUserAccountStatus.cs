using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using PWA_API.Infrastructure.Persistence;

#nullable disable

namespace PWA_API.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260723000000_AddUserAccountStatus")]
public partial class AddUserAccountStatus : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "deactivated_at",
            table: "users",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "is_active",
            table: "users",
            type: "boolean",
            nullable: false,
            defaultValue: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "last_login_at",
            table: "users",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "must_change_password",
            table: "users",
            type: "boolean",
            nullable: false,
            defaultValue: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "deactivated_at", table: "users");
        migrationBuilder.DropColumn(name: "is_active", table: "users");
        migrationBuilder.DropColumn(name: "last_login_at", table: "users");
        migrationBuilder.DropColumn(name: "must_change_password", table: "users");
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banking.Api.Migrations
{
    /// <inheritdoc />
    public partial class auditableSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BankTransactions_AccountId_CreatedAt",
                table: "BankTransactions");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "BankTransactions",
                newName: "CreatedAtUtc");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "Customers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "Customers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "Customers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Customers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "BankTransactions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "BankTransactions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "BankTransactions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "bankAccounts",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "bankAccounts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "bankAccounts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "bankAccounts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "bankAccounts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_AccountId_IdempotencyKey",
                table: "BankTransactions",
                columns: new[] { "AccountId", "IdempotencyKey" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BankTransactions_AccountId_IdempotencyKey",
                table: "BankTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "BankTransactions");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "BankTransactions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "BankTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "bankAccounts");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "bankAccounts");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "bankAccounts");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "bankAccounts");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "bankAccounts");

            migrationBuilder.RenameColumn(
                name: "CreatedAtUtc",
                table: "BankTransactions",
                newName: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_AccountId_CreatedAt",
                table: "BankTransactions",
                columns: new[] { "AccountId", "CreatedAt" });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Startawy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentWorkflowToTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_transaction_user_id",
                table: "transaction");

            migrationBuilder.AddColumn<string>(
                name: "external_reference",
                table: "transaction",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "idempotency_key",
                table: "transaction",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "reference_id",
                table: "transaction",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "reference_type",
                table: "transaction",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "transaction",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_user_idempotency",
                table: "transaction",
                columns: new[] { "user_id", "idempotency_key" },
                unique: true,
                filter: "[idempotency_key] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_transaction_user_idempotency",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "external_reference",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "idempotency_key",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "reference_id",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "reference_type",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "status",
                table: "transaction");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_user_id",
                table: "transaction",
                column: "user_id");
        }
    }
}

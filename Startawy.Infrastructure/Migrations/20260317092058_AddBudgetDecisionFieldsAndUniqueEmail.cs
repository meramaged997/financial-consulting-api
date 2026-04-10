using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Startawy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBudgetDecisionFieldsAndUniqueEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "user",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "FixedCosts",
                table: "budget_analyses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ProfitStatus",
                table: "budget_analyses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "VariableCosts",
                table: "budget_analyses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_user_email",
                table: "user",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_email",
                table: "user");

            migrationBuilder.DropColumn(
                name: "FixedCosts",
                table: "budget_analyses");

            migrationBuilder.DropColumn(
                name: "ProfitStatus",
                table: "budget_analyses");

            migrationBuilder.DropColumn(
                name: "VariableCosts",
                table: "budget_analyses");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "user",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}

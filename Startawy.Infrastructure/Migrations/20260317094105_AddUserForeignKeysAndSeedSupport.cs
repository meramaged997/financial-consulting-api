using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Startawy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserForeignKeysAndSeedSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FounderUserId",
                table: "follow_up_plans",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ConsultantUserId",
                table: "follow_up_plans",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "feedback",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "GrowthRate",
                table: "cash_flow_forecasts",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_follow_up_plans_ConsultantUserId",
                table: "follow_up_plans",
                column: "ConsultantUserId");

            migrationBuilder.CreateIndex(
                name: "IX_follow_up_plans_FounderUserId",
                table: "follow_up_plans",
                column: "FounderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_feedback_UserId",
                table: "feedback",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_consultant_availability_slots_user_ConsultantUserId",
                table: "consultant_availability_slots",
                column: "ConsultantUserId",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_consultation_sessions_user_ConsultantUserId",
                table: "consultation_sessions",
                column: "ConsultantUserId",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_consultation_sessions_user_FounderUserId",
                table: "consultation_sessions",
                column: "FounderUserId",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_feedback_user_UserId",
                table: "feedback",
                column: "UserId",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_follow_up_plans_user_ConsultantUserId",
                table: "follow_up_plans",
                column: "ConsultantUserId",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_follow_up_plans_user_FounderUserId",
                table: "follow_up_plans",
                column: "FounderUserId",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_consultant_availability_slots_user_ConsultantUserId",
                table: "consultant_availability_slots");

            migrationBuilder.DropForeignKey(
                name: "FK_consultation_sessions_user_ConsultantUserId",
                table: "consultation_sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_consultation_sessions_user_FounderUserId",
                table: "consultation_sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_feedback_user_UserId",
                table: "feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_follow_up_plans_user_ConsultantUserId",
                table: "follow_up_plans");

            migrationBuilder.DropForeignKey(
                name: "FK_follow_up_plans_user_FounderUserId",
                table: "follow_up_plans");

            migrationBuilder.DropIndex(
                name: "IX_follow_up_plans_ConsultantUserId",
                table: "follow_up_plans");

            migrationBuilder.DropIndex(
                name: "IX_follow_up_plans_FounderUserId",
                table: "follow_up_plans");

            migrationBuilder.DropIndex(
                name: "IX_feedback_UserId",
                table: "feedback");

            migrationBuilder.AlterColumn<string>(
                name: "FounderUserId",
                table: "follow_up_plans",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ConsultantUserId",
                table: "follow_up_plans",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "feedback",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<decimal>(
                name: "GrowthRate",
                table: "cash_flow_forecasts",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);
        }
    }
}

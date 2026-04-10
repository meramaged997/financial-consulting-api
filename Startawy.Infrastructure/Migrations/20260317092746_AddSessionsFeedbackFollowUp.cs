using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Startawy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionsFeedbackFollowUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "session_rate",
                table: "consultant",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "ProfitStatus",
                table: "budget_analyses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "consultation_sessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FounderUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConsultantUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentTransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConsultantNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConsultantRecommendations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consultation_sessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "feedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsReviewed = table.Column<bool>(type: "bit", nullable: false),
                    ReviewedByAdminId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feedback", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "follow_up_plans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FounderUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsultantUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Goal = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TimelineStartUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimelineEndUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_follow_up_plans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "consultant_availability_slots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConsultantUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsBooked = table.Column<bool>(type: "bit", nullable: false),
                    ConsultationSessionId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consultant_availability_slots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_consultant_availability_slots_consultation_sessions_ConsultationSessionId",
                        column: x => x.ConsultationSessionId,
                        principalTable: "consultation_sessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "follow_up_steps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FollowUpPlanId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_follow_up_steps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_follow_up_steps_follow_up_plans_FollowUpPlanId",
                        column: x => x.FollowUpPlanId,
                        principalTable: "follow_up_plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_consultant_availability_slots_ConsultantUserId_StartAtUtc_EndAtUtc",
                table: "consultant_availability_slots",
                columns: new[] { "ConsultantUserId", "StartAtUtc", "EndAtUtc" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_consultant_availability_slots_ConsultationSessionId",
                table: "consultant_availability_slots",
                column: "ConsultationSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_consultation_sessions_ConsultantUserId_StartAtUtc_EndAtUtc",
                table: "consultation_sessions",
                columns: new[] { "ConsultantUserId", "StartAtUtc", "EndAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_consultation_sessions_FounderUserId_StartAtUtc_EndAtUtc",
                table: "consultation_sessions",
                columns: new[] { "FounderUserId", "StartAtUtc", "EndAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_feedback_IsReviewed_Category",
                table: "feedback",
                columns: new[] { "IsReviewed", "Category" });

            migrationBuilder.CreateIndex(
                name: "IX_follow_up_steps_FollowUpPlanId_DueAtUtc",
                table: "follow_up_steps",
                columns: new[] { "FollowUpPlanId", "DueAtUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "consultant_availability_slots");

            migrationBuilder.DropTable(
                name: "feedback");

            migrationBuilder.DropTable(
                name: "follow_up_steps");

            migrationBuilder.DropTable(
                name: "consultation_sessions");

            migrationBuilder.DropTable(
                name: "follow_up_plans");

            migrationBuilder.DropColumn(
                name: "session_rate",
                table: "consultant");

            migrationBuilder.AlterColumn<string>(
                name: "ProfitStatus",
                table: "budget_analyses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}

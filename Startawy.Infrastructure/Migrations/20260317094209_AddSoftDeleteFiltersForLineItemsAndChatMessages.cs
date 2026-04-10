using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Startawy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteFiltersForLineItemsAndChatMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_chat_sessions_ChatSessionId",
                table: "ChatMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatMessages",
                table: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_ChatSessionId",
                table: "ChatMessages");

            migrationBuilder.RenameTable(
                name: "ChatMessages",
                newName: "chat_messages");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "chat_messages",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_chat_messages",
                table: "chat_messages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_messages_ChatSessionId_SentAt",
                table: "chat_messages",
                columns: new[] { "ChatSessionId", "SentAt" });

            migrationBuilder.AddForeignKey(
                name: "FK_chat_messages_chat_sessions_ChatSessionId",
                table: "chat_messages",
                column: "ChatSessionId",
                principalTable: "chat_sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chat_messages_chat_sessions_ChatSessionId",
                table: "chat_messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_chat_messages",
                table: "chat_messages");

            migrationBuilder.DropIndex(
                name: "IX_chat_messages_ChatSessionId_SentAt",
                table: "chat_messages");

            migrationBuilder.RenameTable(
                name: "chat_messages",
                newName: "ChatMessages");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatMessages",
                table: "ChatMessages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ChatSessionId",
                table: "ChatMessages",
                column: "ChatSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_chat_sessions_ChatSessionId",
                table: "ChatMessages",
                column: "ChatSessionId",
                principalTable: "chat_sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
